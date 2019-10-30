using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.porideki.gates;
using Assets.Scripts.porideki.parts;

public class UnfoldGateUI : GateUI {

    [SerializeField] private GameObject valueSocketObj;
    [SerializeField] private GameObject trueValueSocketObj;
    [SerializeField] private GameObject falseValueSocketObj;
    [SerializeField] private GameObject resultSocketObj;

    internal override Gate Generate() {
        base.Generate();

        //gate割当
        var gate = new UnfoldGate();
        this.allocatedGate = gate;

        //objectAllocator割当
        var valueSocketObjAllocator = this.valueSocketObj.AddComponent<ObjectAllocator>();
        var trueValueSocketObjAllocator = this.trueValueSocketObj.AddComponent<ObjectAllocator>();
        var falseValueSocketObjAllocator = this.falseValueSocketObj.AddComponent<ObjectAllocator>();
        var resultSocketObjAllocator = this.resultSocketObj.AddComponent<ObjectAllocator>();

        //socket割当
        valueSocketObjAllocator.allocatedObj = gate.valueSocket;
        trueValueSocketObjAllocator.allocatedObj = gate.trueValueSocket;
        falseValueSocketObjAllocator.allocatedObj = gate.falseValueSocket;
        resultSocketObjAllocator.allocatedObj = gate.resultSocket;

        return this.allocatedGate;
    }

    internal override GameObject[] GetSocketObjects() {
        return new GameObject[] { valueSocketObj, trueValueSocketObj, falseValueSocketObj, resultSocketObj };
    }

}
