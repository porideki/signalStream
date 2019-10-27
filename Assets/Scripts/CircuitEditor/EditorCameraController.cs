using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraController : MonoBehaviour {

    public Transform backgroundTransform;

    private Vector3 initialBackgroundScale;
    private float initislScreenSize;

    public void Awake() {

        this.initialBackgroundScale = this.backgroundTransform.localScale;
        this.initislScreenSize = Camera.main.orthographicSize;

    }

    public void MoveCameraDelta(Vector3 delta) {

        var position = Camera.main.transform.position;
        position += delta;

        this.MoveCamera(position);
    }

    public void MoveCamera(Vector3 position) {

        //カメラ移動
        Camera.main.transform.position = position;
        //背景移動
        var cameraPos = Camera.main.transform.position;
        cameraPos.z = this.backgroundTransform.position.z;
        this.backgroundTransform.position = cameraPos;

    }

    public void PinchCameraDelta(float delta) {

        var size = Camera.main.orthographicSize;
        size += delta;

        this.PinchCamera(size);

    }

    public void PinchCamera(float size) {

        size = Mathf.Max(1, size);

        //カメラサイズ
        Camera.main.orthographicSize = size;

        //背景サイズ
        var screenScale = Camera.main.orthographicSize / this.initislScreenSize;
        var backgroundScale = this.initialBackgroundScale * screenScale;
        initialBackgroundScale.z = 0;
        this.backgroundTransform.localScale = backgroundScale;

    }

}
