using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.porideki.circuit;
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

        //入力ゲート
        var censorGate = new FunctionSensor<double>(() => {
            return this.boxTranceform.position.y;
        });

        //出力ゲート
        //double
        var motorGate0 = new ActionMotor<double>((value) => {
            this.positionText.text = "-2.5 < " + value + " < 0";
        });

        //bool
        var motorGate1 = new ActionMotor<bool>((value) => {
            this.addText.text = value.ToString();
        });

        //コネクション生成
        CircuitProcessor.MakeConnection(gate.valueSocket, censorGate.outputSocket);
        CircuitProcessor.MakeConnection(motorGate1.inputSocket, gate.resultSocket);
        CircuitProcessor.MakeConnection(motorGate0.inputSocket, censorGate.outputSocket);
        
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
