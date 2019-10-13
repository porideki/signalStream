using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.porideki.gates;
using Assets.Scripts.porideki.parts;

public class PropertyTest : MonoBehaviour {

    public Transform boxTranceform;
    public Text positionText;
    public Text addText;

    void Awake() {

        var divGate = new DivisionGate();
        InputSocket<double> inputSocket = new InputSocket<double>(0);
        inputSocket.readOnlyValueProperty.Subscribe(v => this.addText.text = v.ToString());
        divGate.resultSocket.inputSockets.Add(inputSocket);

        boxTranceform
            .ObserveEveryValueChanged(transform => transform.position)
            .Subscribe(position => {
                this.positionText.text = position.ToString();
                divGate.dividendSocket.Set(position.x);
                divGate.divisorSocket.Set(position.y);
            });

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
