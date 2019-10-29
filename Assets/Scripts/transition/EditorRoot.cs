using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorRoot : SectionRoot {

    public override void OnTransition() {
        base.OnTransition();

        Debug.Log("->Editor");

    }

}
