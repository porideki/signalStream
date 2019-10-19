using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class HandManager : MonoBehaviour {

    //プレハブ
    public GameObject linePrefab;

    [HideInInspector]public GameObject takingObject;    //掴んでいるオブジェクト
    [HideInInspector]public ReactiveProperty<GameObject> takingPaletteProperty; //パレットで選択中のGateオブジェクト
    [HideInInspector] public ReactiveProperty<GameObject> takingSocketProperty; //選択中のソケット

    private GameObject takingLineObject;

    private GameObject pointerTracer;
    private Vector3 mouseWorldPos {
        get { return this.pointerTracer.transform.position; }
    }

    public void Awake() {

        //インスタンス化
        this.takingPaletteProperty = new ReactiveProperty<GameObject>();
        this.takingSocketProperty = new ReactiveProperty<GameObject>();
        this.pointerTracer = new GameObject("PointerTracer");

        //リスナ登録
        this.takingPaletteProperty.Where(paleteObject => paleteObject != null)
                                .Subscribe(paletteObject => Debug.Log(paletteObject.name));

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
        Debug.Log(pointerEventData.pointerEnter.gameObject.tag + "\nButton: " + pointerEventData.button);
        PointerEventData.InputButton button = pointerEventData.button;

        //タグ別動作
        switch (pointerEventData.pointerEnter.tag) {
            //背景
            case "Background":
                //左クリック時
                if(button == PointerEventData.InputButton.Left && this.takingPaletteProperty.Value != null) {
                    //ビューにインスタンス化
                    GameObject instantiatedObject = Instantiate(this.takingPaletteProperty.Value);
                    //マウスポインタ位置に合わせる
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = 10.0f;
                    instantiatedObject.transform.position = this.mouseWorldPos;
                }
                break;

            case "Gate":
                if (button == PointerEventData.InputButton.Right) {
                    Debug.Log("Destroy!");
                    GameObject.Destroy(pointerEventData.pointerEnter.gameObject);
                }
                break;

            case "InputSocket":
            case "OutputSocket":
                if(button == PointerEventData.InputButton.Left) {   //左クリック
                    if (this.takingSocketProperty.Value == null) {  //新規ライン

                    } else if(this.takingSocketProperty.Value != pointerEventData.pointerEnter.gameObject){ //ライン設定

                    }
                }else if(button == PointerEventData.InputButton.Right) {    //右クリック
                    if (this.takingSocketProperty.Value == null) {

                    } else {

                    }
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

}
