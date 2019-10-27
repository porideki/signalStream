using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets.Scripts.porideki.gates;
using Assets.Scripts.porideki.parts;
using Assets.Scripts.porideki.circuit;

public abstract class GateUI : MonoBehaviour {

    [SerializeField] protected InputField Label;
    public string gateName;
    internal Gate allocatedGate;

    internal virtual Gate Generate() {
        this.Label.text = this.gateName;
        return null;
    }

    internal virtual GameObject[] GetSocketObjects() {
        return new GameObject[0];
    }

}
