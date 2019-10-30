using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.porideki.parts;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.porideki.gates;

public class ConstantgateUI : GateUI {

    public double initialValue;
    [SerializeField] private GameObject outputSocketObj;
    [SerializeField] private InputField valueField;

    private double constValue;

    internal override Gate Generate() {
        base.Generate();

        //gate割当
        var gate = new FunctionSensor<double>(() => {
            return this.constValue;
        });
        this.allocatedGate = gate;

        //objctAllocator割当
        var outputSocket = this.outputSocketObj.AddComponent<ObjectAllocator>();

        //object割当
        outputSocket.allocatedObj = gate.outputSocket;

        //constvalue初期化
        this.constValue = this.initialValue;
        //InpuField初期化
        this.valueField.SetTextWithoutNotify(this.constValue.ToString());

        return this.allocatedGate;
    }

    internal override GameObject[] GetSocketObjects() {
        return new GameObject[] { this.outputSocketObj };
    }

    public void OnValueChanged(string inValue) {

        var strValue = this.valueField.text;

        Debug.Log("strValue: " + strValue);

        double parseValue;
        if(double.TryParse(strValue, out parseValue)) {
            this.constValue = parseValue;
            Debug.Log("parse successed: " + this.constValue);
        } else {
            this.constValue = 0;
            Debug.Log("parse failed: " + this.constValue);
        }

    }

}
