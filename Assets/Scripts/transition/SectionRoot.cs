using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionRoot : MonoBehaviour　{

    public Camera SectionCamera;
    
    public virtual void OnTransition() {
        this.SectionCamera.enabled = true;
    }

    public virtual void OnReaved() {
        this.SectionCamera.enabled = false;
    }

}
