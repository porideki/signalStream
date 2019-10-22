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

        //入力センサ
        var censorGate = new FunctionSensor<double>(() => {
            return this.boxTranceform.position.y;
        });

        //中間ゲート
        var gate = new ThresholdGate(-2.5, 0);  //二値化
        var unfoGate = new UnfoldGate();    //展開

        //出力モータ
        //yスループット
        var motorGate0 = new ActionMotor<double>((value) => {
            this.positionText.text = "-2.5 < " + value + " < 0";
        });

        //範囲内表示
        var motorGate1 = new ActionMotor<double>((value) => {
            this.addText.text = value.ToString();
        });

        //コネクション生成
        CircuitProcessor.MakeConnection(gate.valueSocket, censorGate.outputSocket);
        CircuitProcessor.MakeConnection(unfoGate.valueSocket, gate.resultSocket);
        CircuitProcessor.MakeConnection(motorGate1.inputSocket, unfoGate.resultSocket);
        CircuitProcessor.MakeConnection(motorGate0.inputSocket, censorGate.outputSocket);
        
    }

}
