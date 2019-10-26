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

    public void Awake() {

        //インスタンス化
        this.bindedStartGameObject = new ReactiveProperty<GameObject>();
        this.bindedEndGameObject = new ReactiveProperty<GameObject>();

        //LineRenderer取得
        this.lineRenderer = this.GetComponent<LineRenderer>();

        //始点終点位置合わせ(Transform)
        Observable.EveryFixedUpdate()
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
        Observable.EveryFixedUpdate()
            .Subscribe(fr => {
                this.lineRenderer.SetPositions(new Vector3[] {
                    this.startTransform.position,
                    this.endTransform.position
                });
            });

    }

}
