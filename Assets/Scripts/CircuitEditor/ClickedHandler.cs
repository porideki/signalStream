using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

public class ClickedHandler : MonoBehaviour, IPointerClickHandler {

    private HandManager handManager;

    public void Awake() {
        //ハブ取得
        this.handManager = GameObject.Find("EditorManager").GetComponent<HandManager>();
    }
    
    public void OnPointerClick(PointerEventData pointerEventData) {
        this.handManager.OnCkickedHub(pointerEventData);
    }

}
