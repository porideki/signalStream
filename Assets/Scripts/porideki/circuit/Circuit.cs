using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using Assets.Scripts.porideki.parts;

public class Circuit {

    internal ReactiveCollection<Gate> Gates;
    internal ReactiveCollection<object> inputScokets;
    internal ReactiveCollection<object> outputSockets;

    public Circuit() {

        //インスタンス
        this.Gates = new ReactiveCollection<Gate>();
        this.inputScokets = new ReactiveCollection<object>();
        this.outputSockets = new ReactiveCollection<object>();

    }

}
