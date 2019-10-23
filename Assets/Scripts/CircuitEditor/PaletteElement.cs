using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PaletteElement : MonoBehaviour {

    public GameObject prefab;

    private PaletteManager paletteManager;

    public void Awake() {

        //マネージャ取得
        this.paletteManager = GameObject.Find("Palette").GetComponent<PaletteManager>();
        if (this.paletteManager == null) Debug.LogError("PaletteManager is not found.");

    }

    //
    public void OnClicked() {
        this.paletteManager.selectedElementProperty.Value = this;
    }

}
