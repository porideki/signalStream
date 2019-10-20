using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.porideki.gates;
using Assets.Scripts.porideki.parts;
using Assets.Scripts.porideki.util;

public class PropertyTest : MonoBehaviour {

    public Transform boxTranceform;
    public Text positionText;
    public Text addText;

    void Awake() {

        //二値化ゲート
        var gate = new ThresholdGate();
        //範囲指定
        gate.maxSocket.Set(0);
        gate.minSocket.Set(-2.5);

        //ソケット作成
        var outputSocket = new OutputSocket<double>();
        var inputSocket = gate.valueSocket;

        //プロセス　インスタンス部分作成
        Observable.EveryUpdate().Subscribe(_ => {
            this.positionText.text = gate.minSocket.Get() + " < " + this.boxTranceform.position.y.ToString() + " < " + gate.maxSocket.Get();
            this.addText.text = gate.resultSocket.Get().ToString();
            //gate.valueSocket.Set(this.boxTranceform.position.y);
            outputSocket.Set(this.boxTranceform.position.y);    //y座標をソケットに設定
        });

        //コネクション
        CircuitProcessor.MakeConnection(inputSocket, outputSocket);

        Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(_ => {
            Debug.Log("Remove connection");
            CircuitProcessor.RemoveConnection(inputSocket);
        });
        Observable.Timer(TimeSpan.FromSeconds(10)).Subscribe(_ => {
            Debug.Log("Make connection");
            CircuitProcessor.MakeConnection(inputSocket, outputSocket);
        });
        
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
