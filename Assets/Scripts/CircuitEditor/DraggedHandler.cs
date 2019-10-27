using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggedHandler : MonoBehaviour, IDragHandler {

    public void OnDrag(PointerEventData pointerEventData) {

        //ポインタ位置取得
        Vector3 draggedPos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        draggedPos.z = 0;
        this.transform.position = draggedPos;

    }
    
}
