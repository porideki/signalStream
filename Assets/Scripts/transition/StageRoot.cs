using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRoot : SectionRoot {

    public CircuitStrage circuitStrage;

    public override void OnTransition() {
        base.OnTransition();
        this.circuitStrage.circuit.Start();

    }

    public override void OnReaved() {
        base.OnReaved();
        this.circuitStrage.circuit.Stop();
    }

}
