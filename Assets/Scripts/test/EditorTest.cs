using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.porideki.circuit;

public class EditorTest : MonoBehaviour {

    [SerializeField] private CircuitStrage circuitStrage;
    private Circuit circuit {
        get { return this.circuitStrage.circuit; }
    }

    private bool isCircuitRun = false;

    void Awake() {

        Observable.EveryUpdate().Where(fl => Input.GetKeyDown(KeyCode.E))
            .Subscribe(fl => {
                if (this.isCircuitRun) this.circuit.Stop();
                else this.circuit.Start();

                this.isCircuitRun ^= true;
            });

    }

}
