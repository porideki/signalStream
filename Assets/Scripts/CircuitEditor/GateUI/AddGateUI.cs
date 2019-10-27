using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.porideki.gates;
using Assets.Scripts.porideki.parts;

public class AddGateUI : GateUI {

    //ソケットを割り当てるGameObject
    [SerializeField] private GameObject addendSocketObj0;
    [SerializeField] private GameObject addendSocketObj1;
    [SerializeField] private GameObject resultSocketObj;

    internal override Gate Generate() {
        base.Generate();

        //ゲート作成
        var sumGate = new SumGate();
        this.allocatedGate = sumGate;   //メンバに割当

        //コンポーネント割当
        var addendSocket0 = this.addendSocketObj0.AddComponent<ObjectAllocator>();
        var addendSocket1 = this.addendSocketObj1.AddComponent<ObjectAllocator>();
        var resultSocket = this.resultSocketObj.AddComponent<ObjectAllocator>();

        //オブジェクト割当コンポーネントにオブジェクト割当(日本語)
        addendSocket0.allocatedObj = sumGate.addendSocket0;
        addendSocket1.allocatedObj = sumGate.addendSocket1;
        resultSocket.allocatedObj = sumGate.resultSocket;

        return this.allocatedGate;
        
    }

    internal override GameObject[] GetSocketObjects() {
        return new GameObject[] { this.addendSocketObj0, this.addendSocketObj1, this.resultSocketObj };
    }

}
