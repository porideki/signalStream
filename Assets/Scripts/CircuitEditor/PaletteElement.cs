using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PaletteElement : MonoBehaviour {

    public GameObject prefab;

    private PaletteManager paletteManager;
    private ReactiveProperty<bool> isSelectedProperty;

    public void Awake() {

        //インスタンス化
        this.isSelectedProperty = new ReactiveProperty<bool>(false);

        //マネージャ取得
        this.paletteManager = GameObject.Find("Palette").GetComponent<PaletteManager>();
        if (this.paletteManager == null) Debug.LogError("PaletteManager is not found.");

    }

    //
    public void OnClicked() {
        Debug.Log("Element is clicked");
        this.paletteManager.selectedElementProperty.Value = this;
    }

}
