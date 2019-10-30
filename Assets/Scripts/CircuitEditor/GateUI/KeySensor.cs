using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.porideki.parts;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.porideki.gates;

public class KeySensor : GateUI {

    public string keyCode;
    [SerializeField] private GameObject resultSocketObj;
    [SerializeField] private InputField keyCodeField;

    internal override Gate Generate() {
        base.Generate();

        //Gate割当
        var gate = new FunctionSensor<bool>(() => {
            return Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), this.keyCode));
        });
        this.allocatedGate = gate;

        //Socket割当
        var resultSocket = this.resultSocketObj.AddComponent<ObjectAllocator>();
        resultSocket.allocatedObj = gate.outputSocket;

        //初期化
        this.keyCodeField.SetTextWithoutNotify(this.keyCode);

        return this.allocatedGate;

    }

    internal override GameObject[] GetSocketObjects() {
        return new GameObject[] { this.resultSocketObj };
    }

    public void OnKeyCodeChanged(string keyCode) {
        Debug.Log(keyCode);
        this.keyCode = keyCode;
    }

}
