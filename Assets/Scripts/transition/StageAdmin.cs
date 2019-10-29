using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAdmin : MonoBehaviour {

    public GameObject[] roots;

    public void Transition(int i) {

        //とりあえず全部非アクティブ化
        foreach(GameObject root in this.roots) {
            root.SetActive(false);
        }

        //指定rootをアクティブ
        if(0 <= i && i < this.roots.Length) {
            this.roots[i].SetActive(true);
            SectionRoot sectionRoot;
            if((sectionRoot = this.roots[i].GetComponent<SectionRoot>()) != null) {
                sectionRoot.OnTransition();
            }
        } else {
            Debug.LogError("指定が範囲外です(length: " + this.roots.Length + ", i: " + i + ")");
        }
    }

}
