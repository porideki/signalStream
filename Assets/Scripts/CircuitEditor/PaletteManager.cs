using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PaletteManager : MonoBehaviour {

    private HandManager handManager;

    //選択中パレットエレメント
    public ReactiveProperty<PaletteElement> selectedElementProperty;

    public void Awake() {

        //インスタンス化
        this.selectedElementProperty = new ReactiveProperty<PaletteElement>();

        //オブジェクト取得
        this.handManager = GameObject.Find("EditorManager").GetComponent<HandManager>();
        if (this.handManager == null) Debug.LogError("HandManager is not found.");

        //リスナ
        this.selectedElementProperty.Where(element => element != null) //nullの時は無視(主に購読登録時)
                                    .Subscribe(element => {
                                        Debug.Log("selectedElement is changed.");
                                        this.handManager.takingPaletteProperty.Value = element.prefab;
                                    }); //手持ちのプレハブ変更

    }

}
