using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.porideki.circuit;

public class CircuitStrage : MonoBehaviour {

    public Circuit circuit; 
    
    public void Awake() {

        this.circuit = new Circuit();

    }

}
