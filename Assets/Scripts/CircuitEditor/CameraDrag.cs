using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public class CameraDrag : MonoBehaviour, IDragHandler {

    public EditorCameraController editorCameraController;

    private IDisposable scrollListerner;

    public void Awake() {

        this.scrollListerner = Observable.EveryFixedUpdate()
            .Subscribe(fr => {
                this.editorCameraController.PinchCameraDelta(Input.mouseScrollDelta.y * -0.5f);
            });

    }

    public void OnDrag(PointerEventData pointerEventData) {

        if(pointerEventData.button == PointerEventData.InputButton.Middle) {

            Vector3 delta = pointerEventData.delta;
            delta.z = 0;
            //Screen座標スケールに変換
            delta.x /= Screen.width;
            delta.y /= Screen.height;
            //World画面サイズ
            var leftTopPosition = Camera.main.ScreenToWorldPoint(Vector3.zero);
            var rightBottomPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            var worldScreenSize = rightBottomPosition - leftTopPosition;
            //World座標系に変換
            delta.x *= worldScreenSize.x;
            delta.y *= worldScreenSize.y;
            //反転
            delta.x *= -1;
            delta.y *= -1;

            //カメラ・背景移動
            this.editorCameraController.MoveCameraDelta(delta);

        }

    }

    public void OnDestroy() {
        this.scrollListerner.Dispose();
    }
    
}
