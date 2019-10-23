using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.porideki.gates;
using Assets.Scripts.porideki.parts;
using Assets.Scripts.porideki.circuit;

public abstract class GateUI : MonoBehaviour {

    internal Gate allocatedGate;

    internal abstract Gate Generate();

}
