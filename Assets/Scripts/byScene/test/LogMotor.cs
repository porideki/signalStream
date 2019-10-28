using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.porideki.parts;
using Assets.Scripts.porideki.gates;

public class LogMotor : GateUI {

    [SerializeField] private GameObject inputSocketObj;

    internal override Gate Generate() {
        base.Generate();

        var functionGate = new ActionMotor<double>(value => {
            Debug.Log("LogMotor> " + value);
        });
        this.allocatedGate = functionGate;

        var allocatedInputSocketObj = this.inputSocketObj.AddComponent<ObjectAllocator>();
        allocatedInputSocketObj.allocatedObj = functionGate.inputSocket;

        return this.allocatedGate;

    }

    internal override GameObject[] GetSocketObjects() {
        return new GameObject[] { this.inputSocketObj };
    }

}
