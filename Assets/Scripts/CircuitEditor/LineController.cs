using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class LineController : MonoBehaviour {

    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    public float lineZ = 0.2f;  //

    [HideInInspector] public ReactiveProperty<GameObject> bindedStartGameObject;
    [HideInInspector] public ReactiveProperty<GameObject> bindedEndGameObject;

    private LineRenderer lineRenderer;

    private IDisposable transformFitObservable;
    private IDisposable lineFitObservable;

    public void Awake() {

        //インスタンス化
        this.bindedStartGameObject = new ReactiveProperty<GameObject>();
        this.bindedEndGameObject = new ReactiveProperty<GameObject>();

        //LineRenderer取得
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.lineRenderer.enabled = false;

        //始点終点位置合わせ(Transform)
        this.transformFitObservable = Observable.EveryFixedUpdate()
            .Where(fr => {
                return this.bindedStartGameObject.HasValue
                && this.bindedEndGameObject.HasValue;
            })
            .Subscribe(fr => {
                //startPos
                var startPos = this.bindedEndGameObject.Value.transform.position;
                startPos.z = this.lineZ;
                this.startTransform.position = startPos;
                //endPos
                var endPos = this.bindedStartGameObject.Value.transform.position;
                endPos.z = this.lineZ;
                this.endTransform.position = endPos;
            });

        //終点始点位置合わせ(LineRenderer)
        this.lineFitObservable = Observable.EveryFixedUpdate()
            .Subscribe(fr => {
                this.lineRenderer.SetPositions(new Vector3[] {
                    this.startTransform.position,
                    this.endTransform.position
                });
            });

        //表示非表示設定
        Func<GameObject, bool> isBothSocketBinded = gameObject => this.bindedStartGameObject.HasValue && this.bindedEndGameObject.HasValue;  //両端がバインドされている
        this.bindedStartGameObject.Where(isBothSocketBinded).Subscribe(gameObject => this.lineRenderer.enabled = true );
        this.bindedEndGameObject.Where(isBothSocketBinded).Subscribe(gameObject => this.lineRenderer.enabled = true);

    }

    public void OnDestroy() {

        //購読中止
        this.transformFitObservable.Dispose();
        this.lineFitObservable.Dispose();

        //参照破棄
        this.bindedStartGameObject.Value = null;
        this.bindedEndGameObject.Value = null;

    }

    public bool IsBindTo(object socket) {

        object allocatedObj;
        bool inBindTo = false;
        bool outBindTo = false;
        //inputSocket
        if ((allocatedObj = this.bindedStartGameObject.Value.GetComponent<ObjectAllocator>().allocatedObj) != null) {
            inBindTo = allocatedObj.Equals(socket);
        } else inBindTo = false;
        //outputSocket
        if ((allocatedObj = this.bindedEndGameObject.Value.GetComponent<ObjectAllocator>().allocatedObj) != null) {
            outBindTo = allocatedObj.Equals(socket);
        } else outBindTo = false;

        return inBindTo || outBindTo;
    }

}
