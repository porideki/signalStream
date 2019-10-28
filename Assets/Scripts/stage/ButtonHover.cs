using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour {

    public Sprite normalSprite;
    public Sprite hoverSprite;

    [SerializeField] private Image image;
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private RectTransform normalTransform;
    [SerializeField] private RectTransform hoverTransform;

    public void Awake() {

        EventTrigger eventTigger;
        if((eventTigger = this.GetComponent<EventTrigger>()) == null) {
            eventTigger = this.gameObject.AddComponent<EventTrigger>();
        }

        //イベント作成
        var mouseEnter = new EventTrigger.Entry();
        var mouseExit = new EventTrigger.Entry();
        mouseEnter.eventID = EventTriggerType.PointerEnter;
        mouseExit.eventID = EventTriggerType.PointerExit;

        //ハンドル
        mouseEnter.callback.AddListener(data => {
            this.image.sprite = this.hoverSprite;
            this.textTransform.position = this.hoverTransform.position;
        });

        mouseExit.callback.AddListener(data => {
            this.image.sprite = this.normalSprite;
            this.textTransform.position = this.normalTransform.position;
        });

        //追加
        eventTigger.triggers.Add(mouseEnter);
        eventTigger.triggers.Add(mouseExit);

    }

}
