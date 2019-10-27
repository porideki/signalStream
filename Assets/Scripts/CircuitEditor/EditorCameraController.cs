using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraController : MonoBehaviour {

    public Transform backgroundTransform;

    public void MoveCamera(Vector3 delta) {

        //カメラ移動
        Camera.main.transform.position += delta;
        //背景移動
        var cameraPos = Camera.main.transform.position;
        cameraPos.z = 0;
        this.backgroundTransform.position = cameraPos;

    }

}
