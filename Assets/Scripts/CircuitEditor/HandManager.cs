using Assets.Scripts.porideki.circuit;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandManager : MonoBehaviour {

    //プレハブ
    public GameObject linePrefab;

    LineRenderer lr;

    [HideInInspector] public GameObject takingObject;    //掴んでいるオブジェクト
    [HideInInspector] public ReactiveProperty<GameObject> takingPaletteProperty; //パレットで選択中のGateオブジェクト
    [HideInInspector] public ReactiveProperty<GameObject> takingInputSocketProperty; //選択中のinソケット
    [HideInInspector] public ReactiveProperty<GameObject> takingOutputSocketProperty;   //選択中のoutソケット

    private List<GameObject> streamLines;
    private GameObject sockToMouseStreamLine;

    //
    private GameObject circuitStrage {
        get { return GameObject.Find("CircuitStrage"); }
    }
    //回路オブジェクト
    private Circuit circuit {
        get { return this.circuitStrage.GetComponent<CircuitStrage>().circuit; }
    }

    //
    private GameObject takingLineObject;

    private GameObject pointerTracer;
    private Vector3 mouseWorldPos {
        get { return this.pointerTracer.transform.position; }
    }

    public void Awake() {

        //インスタンス化
        this.takingPaletteProperty = new ReactiveProperty<GameObject>();
        this.takingInputSocketProperty = new ReactiveProperty<GameObject>();
        this.takingOutputSocketProperty = new ReactiveProperty<GameObject>();
        this.pointerTracer = new GameObject("PointerTracer");
        this.streamLines = new List<GameObject>();

        //リスナ登録
        //クリック時のログ表示
        this.takingPaletteProperty.Where(paleteObject => paleteObject != null)
                                .Subscribe(paletteObject => Debug.Log(paletteObject.name));
        //ソケットクリック時のコネクション確率
        this.takingInputSocketProperty.Subscribe(_ => this.TryConnection());
        this.takingOutputSocketProperty.Subscribe(_ => this.TryConnection());

        //ポインタのワールド座標トレース
        Observable.EveryFixedUpdate()
            .Select(_ => {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = 10.0f;
                return Camera.main.ScreenToWorldPoint(mousePosition);
            })
            .Subscribe(mousePosition => this.pointerTracer.transform.position = mousePosition);

    }

    public void OnCkickedHub(PointerEventData pointerEventData) {

        //コンソール表示
        Debug.Log(pointerEventData.pointerEnter + "(" + pointerEventData.pointerEnter.gameObject.tag + ")\nButton: " + pointerEventData.button);
        PointerEventData.InputButton button = pointerEventData.button;

        //タグ別動作
        switch (pointerEventData.pointerEnter.tag) {
            //背景
            case "Background":
                //左クリック時
                if(button == PointerEventData.InputButton.Left && this.takingPaletteProperty.Value != null) {

                    //ビューにインスタンス化
                    GameObject instantiatedObject = Instantiate(this.takingPaletteProperty.Value);
                    instantiatedObject.transform.parent = this.circuitStrage.transform; //親登録
                    //マウスポインタ位置に合わせる
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = 10.0f;
                    instantiatedObject.transform.position = this.mouseWorldPos;
                    //回路に追加
                    var gate = instantiatedObject.GetComponent<GateUI>().Generate();
                    this.circuit.AddGate(gate);

                }else if(button == PointerEventData.InputButton.Right) {

                    //マウス追従ライン削除
                    GameObject.Destroy(this.sockToMouseStreamLine);

                }
                break;

            case "Gate":
                if (button == PointerEventData.InputButton.Right) {
                    Debug.Log("Destroyed " + pointerEventData.pointerEnter.name);
                    this.RemoveGate(pointerEventData.pointerEnter);
                }
                break;

            case "InputSocket":
                if(button == PointerEventData.InputButton.Left) {
                    if(this.streamLines.Count(lineObject => {
                        var lineController = lineObject.GetComponent<LineController>();
                        return lineController.IsBindTo(pointerEventData.pointerEnter.GetComponent<ObjectAllocator>().allocatedObj);
                    }) == 0) {
                        this.takingInputSocketProperty.Value = pointerEventData.pointerEnter;
                    }
                } else if(button == PointerEventData.InputButton.Right) {
                    this.RemoveConnection(pointerEventData.pointerEnter);
                }
                break;

            case "OutputSocket":
                if (button == PointerEventData.InputButton.Left) {
                    this.takingOutputSocketProperty.Value = pointerEventData.pointerEnter;
                } else if (button == PointerEventData.InputButton.Right) {
                    this.RemoveConnection(pointerEventData.pointerEnter);
                }
                break;

            default:
                break;
        }

        if(button == PointerEventData.InputButton.Left) {

        }else if(button == PointerEventData.InputButton.Right) {
            if (this.takingLineObject != null) {
                var buf = this.takingLineObject;
                this.takingLineObject = null;
                GameObject.Destroy(buf);
            }
        }

    }

    private void TryConnection() {

        //マウス追従のLineを削除
        GameObject.Destroy(this.sockToMouseStreamLine);

        object inputSocket, outputSocket;

        //両方notNull
        if (((inputSocket = this.takingInputSocketProperty.Value?.GetComponent<ObjectAllocator>()?.allocatedObj) != null)
            && ((outputSocket = this.takingOutputSocketProperty.Value?.GetComponent<ObjectAllocator>()?.allocatedObj) != null)
            && (!Circuit.HasConnection(inputSocket, outputSocket))) {

            //コネクション作成
            Circuit.MakeConnection(inputSocket, outputSocket);
            //Line作成
            var lineControllerObj = GameObject.Instantiate(this.linePrefab);
            lineControllerObj.transform.parent = this.circuitStrage.transform;  //親設定
            var lineController = lineControllerObj.GetComponent<LineController>();  //LineControllser取得
            lineController.bindedStartGameObject.Value = this.takingInputSocketProperty.Value;  //追従Transform設定
            lineController.bindedEndGameObject.Value = this.takingOutputSocketProperty.Value;
            //Line一覧に追加
            this.streamLines.Add(lineControllerObj);
            //手持ち割当削除
            this.takingInputSocketProperty.Value = null;
            this.takingOutputSocketProperty.Value = null;

            Debug.Log("Dane Connect");

        //片方Null
        } else if (((inputSocket = this.takingInputSocketProperty.Value?.GetComponent<ObjectAllocator>()?.allocatedObj) != null)
            ^ ((outputSocket = this.takingOutputSocketProperty.Value?.GetComponent<ObjectAllocator>()?.allocatedObj) != null)) {

            //インスタンス生成
            this.sockToMouseStreamLine = GameObject.Instantiate(this.linePrefab);
            //LineController取得
            var lineController = this.sockToMouseStreamLine.GetComponent<LineController>();
            //バインド設定
            if (inputSocket == null) {  //mouse -> outsocket
                lineController.bindedStartGameObject.Value = this.pointerTracer;
                lineController.bindedEndGameObject.Value = this.takingOutputSocketProperty.Value;
            } else {    //insocket -> mouse
                lineController.bindedStartGameObject.Value = this.takingInputSocketProperty.Value;
                lineController.bindedEndGameObject.Value = this.pointerTracer;
            }

        } else {
            Debug.Log("Failed Connect");
        }

    }

    private void RemoveConnection(GameObject socketObj) {

        Debug.Log(socketObj.name);

        object socket;  //RemoveConnectionするSocket
        if((socket = socketObj.GetComponent<ObjectAllocator>().allocatedObj) != null) {
            if (Circuit.RemoveConnection(socket)) {
                //削除対象Line(GameObject)を別リストに対比(foreach内で回転元のリストを編集出来ないため)
                var removeLines = this.streamLines
                    .Where(gameObject => (gameObject.GetComponent<LineController>() != null)
                                        && gameObject.GetComponent<LineController>().IsBindTo(socket))
                    .ToList();
                //削除
                removeLines.ForEach(gameObject => {
                    this.streamLines.Remove(gameObject);
                    GameObject.Destroy(gameObject);
                });
                //Log表示
                Debug.Log("Connection Removed");
            } else {
                Debug.Log("Removing Connection failed");
            }
        }
        

    }

    private void RemoveGate(GameObject gateUIObj) {

        GateUI gateUI;
        if((gateUI = gateUIObj.GetComponent<GateUI>()) != null) {

            var socketObjects = new List<GameObject>(gateUI.GetSocketObjects());    //list変換
            socketObjects.ForEach(socketObject => { //コネクション+インジケータ削除
                this.RemoveConnection(socketObject);
            });
            //回路から削除
            this.circuit.RemoveGate(gateUI.allocatedGate);
            //UI削除
            GameObject.Destroy(gateUIObj);

        }

    }

}
