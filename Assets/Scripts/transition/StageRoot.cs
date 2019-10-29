using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRoot : SectionRoot {

    public override void OnTransition() {
        base.OnTransition();

        Debug.Log("-> Stage");

    }

}
