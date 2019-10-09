using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class iTweenTest : MonoBehaviour {

    public Transform[] transforms;

    private int positionIndex;
    private bool isStop = true;

    // Start is called before the first frame update
    void Start() {

        Observable.EveryUpdate()
            .Select(_ => Input.inputString)
            .Where(xs => Input.GetKeyDown(KeyCode.Space) && this.isStop)
            .Subscribe(xs => this.move());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void move() {

        this.isStop = false;

        iTween.MoveTo(this.gameObject, iTween.Hash(
                "position", this.transforms[this.positionIndex].position,
                "time", 3f,
                "easeType", iTween.EaseType.easeInOutQuad,
                "oncomplete" , "completefunc"
            ));

        //this.positionIndex = (this.positionIndex + 1) % this.transforms.Length;

    }

    void completefunc() {

        this.isStop = true;
        this.positionIndex = (this.positionIndex + 1) % this.transforms.Length;

    }

}
