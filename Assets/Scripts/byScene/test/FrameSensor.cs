using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.porideki.parts;
using UnityEngine;
using Assets.Scripts.porideki.gates;

public class FrameSensor : GateUI {

    [SerializeField]private GameObject resultSocketObj;

    internal override Gate Generate() {
        base.Generate();

        var funcGate = new FunctionSensor<double>(() => {
            return (double)Time.frameCount;
        });
        this.allocatedGate = funcGate;

        var allocatedResultSocketObj = this.resultSocketObj.AddComponent<ObjectAllocator>();
        allocatedResultSocketObj.allocatedObj = funcGate.outputSocket;

        return this.allocatedGate;

    }

    internal override GameObject[] GetSocketObjects() {
        return new GameObject[] { this.resultSocketObj };
    }

}
