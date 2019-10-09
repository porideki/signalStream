using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PropertyTest : MonoBehaviour {

    public Transform boxTranceform;
    public Text text;

    void Awake() {

        boxTranceform
            .ObserveEveryValueChanged(transform => transform.position)
            .Subscribe(position => this.text.text = position.ToString());

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
