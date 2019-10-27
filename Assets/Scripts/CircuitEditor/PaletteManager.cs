using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PaletteManager : MonoBehaviour {

    //ハンドマネージャ
    private HandManager handManager;

    //選択中パレットエレメント
    public ReactiveProperty<PaletteElement> selectedElementProperty;

    //エレメントの背景スプライト
    public Sprite noSelectedSprite;
    public Sprite selectedSprite;

    public void Awake() {

        //インスタンス化
        this.selectedElementProperty = new ReactiveProperty<PaletteElement>();  //選択中エレメント

        //ハンドマネージャ取得
        this.handManager = GameObject.Find("EditorManager").GetComponent<HandManager>();
        if (this.handManager == null) Debug.LogError("HandManager is not found.");

        //リスナ
        this.selectedElementProperty.Where(element => element != null) //nullの時は無視(主に購読登録時)
                                    .Pairwise()
                                    .Subscribe(elementPare => {
                                        Debug.Log("selectedElement is changed.");
                                        //背景スプライトを変更
                                        elementPare.Current.GetComponent<Image>().sprite = this.selectedSprite;
                                        elementPare.Previous.GetComponent<Image>().sprite = this.noSelectedSprite;
                                        //手持ちのプレハブを更新
                                        this.handManager.takingPaletteProperty.Value = elementPare.Current.prefab;
                                    });

    }

}
