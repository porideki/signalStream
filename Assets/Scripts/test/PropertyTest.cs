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

        var gate = new ThresholdGate();
        gate.maxSocket.Set(0);
        gate.minSocket.Set(-2.5);
        Observable.EveryUpdate().Subscribe(_ => {
            this.positionText.text = gate.minSocket.Get() + " < " + this.boxTranceform.position.y.ToString() + " < " + gate.maxSocket.Get();
            gate.valueSocket.Set(this.boxTranceform.position.y);
        });

        InputSocket<bool> inputSocket = new InputSocket<bool>(true);
        gate.resultProperty.inputSockets.Add(inputSocket);
        inputSocket.Subscribe(v => this.addText.text = v.ToString());

    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
