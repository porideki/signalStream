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

        var divGate = new DivisionGate();
        InputSocket<double> inputSocket = new InputSocket<double>(0);
        inputSocket.readOnlyValueProperty.Subscribe(v => {
            this.addText.text = divGate.dividendSocket.Get().ToString() + " % " + divGate.divisorSocket.Get() 
                            + "\n= " + v.ToString();
            });
        divGate.remainderSocket.inputSockets.Add(inputSocket);

        boxTranceform
            .ObserveEveryValueChanged(transform => transform.position)
            .Subscribe(position => {
                this.positionText.text = position.ToString();
                divGate.dividendSocket.Set(position.x);
                divGate.divisorSocket.Set(position.y);
            });

        var range = new Range(100, 0);
        Debug.Log("[" + range.min + ", " + range.max + "]");

    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
