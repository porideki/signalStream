using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MenuDialog : MonoBehaviour {

    public float easeTime;
    [SerializeField] private KeyCode actuateKey;
    public RectTransform startTransform;
    public RectTransform showTransform;
    public RectTransform endTransform;

    private ReactiveProperty<bool> isShow;
    private IDisposable keyListener;

    public void Awake() {

        //インスタンス
        this.isShow = new ReactiveProperty<bool>(false);

        //リスナ
        this.keyListener = Observable.EveryUpdate()
            .Where(fr => Input.GetKeyDown(this.actuateKey))
            .Subscribe(fr => {
                this.isShow.Value ^= true;
            });

        this.isShow.Subscribe(isShow => this.setShow(isShow));

    }

    public void Start() {
        this.setShow(false, 0);
    }

    private void setShow(bool isShow, float easeTime) {

        Hashtable moveProperty;

        //show
        if (isShow) {
            //初期位置
            this.transform.position = this.startTransform.position;

            moveProperty = new Hashtable() {
                {"position", this.showTransform.position},
                {"time", easeTime },
                {"easeType", iTween.EaseType.easeOutQuad }
            };

        //hide
        } else {

            moveProperty = new Hashtable() {
                {"position", this.endTransform.position},
                {"time", easeTime },
                {"easeType", iTween.EaseType.easeInQuad }
            };

        }

        iTween.MoveTo(this.gameObject, moveProperty);

    }

    private void setShow(bool isShow) {
        this.setShow(isShow, this.easeTime);
    }

    public void Show() { this.isShow.Value = true; }
    public void Hide() { this.isShow.Value = false; }

    public void OnDestroy() {

        this.keyListener.Dispose();

    }

}
