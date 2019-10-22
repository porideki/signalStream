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

        //回路
        var circuit = new Circuit();
        var circuitProcessor = new CircuitProcessor(circuit);

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

        //回路登録
        circuitProcessor.AddGate(censorGate);
        circuitProcessor.AddGate(gate);
        circuitProcessor.AddGate(unfoGate);
        circuitProcessor.AddGate(motorGate0);
        circuitProcessor.AddGate(motorGate1);

        //コネクション生成
        CircuitProcessor.MakeConnection(gate.valueSocket, censorGate.outputSocket);
        CircuitProcessor.MakeConnection(unfoGate.valueSocket, gate.resultSocket);
        CircuitProcessor.MakeConnection(motorGate1.inputSocket, unfoGate.resultSocket);
        CircuitProcessor.MakeConnection(motorGate0.inputSocket, censorGate.outputSocket);

        Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.E))
            .Subscribe(_ => {
                if (censorGate.IsRun) {
                    Debug.Log("Stop");
                    circuitProcessor.Stop();
                } else {
                    Debug.Log("Start");
                    circuitProcessor.Start();
                }
                
            });

    }

}
