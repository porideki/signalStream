using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.porideki.parts;
using UnityEngine;
using Assets.Scripts.porideki.gates;
using Assets.Scripts.porideki.util;

public class HorizontalMotor : GateUI {

    public double max = 1;
    public double min = -1;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject valueSocketObj;

    internal override Gate Generate() {
        base.Generate();

        var range = new Range(max, min);

        var motor = new ActionMotor<double>((value) => {
            var playerRigidBody = this.player.GetComponent<Rigidbody2D>();
            var velocoty = playerRigidBody.velocity;
            velocoty.x = Mathf.Max((float)range.min, Mathf.Min((float)value, (float)range.max));
            playerRigidBody.velocity = velocoty;
            
        });
        this.allocatedGate = motor;

        var valueSocket = this.valueSocketObj.AddComponent<ObjectAllocator>();
        valueSocket.allocatedObj = motor.inputSocket;

        return this.allocatedGate;

    }

    internal override GameObject[] GetSocketObjects() {
        return new GameObject[] { this.valueSocketObj };
    }

}
