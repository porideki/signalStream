using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageAdmin : MonoBehaviour {

    public int startSectionIndex;
    public GameObject[] roots;

    private int? currentSection;

    public void Start() {

        this.currentSection = null;
        this.Transition(this.startSectionIndex);

    }

    public void Transition(int i) {

        //とりあえず全部非アクティブ化
        foreach(GameObject root in this.roots) {
            root.SetActive(false);
        }

        if(0 <= i && i < this.roots.Length) {
            SectionRoot sectionRoot;
            //アクティベート
            this.roots[i].SetActive(true);
            if((sectionRoot = this.roots[i].GetComponent<SectionRoot>()) != null) {
                sectionRoot.OnTransition();
            }
            //ディアクティベート
            if(this.currentSection != null) {
                if ((sectionRoot = this.roots[(int)this.currentSection].GetComponent<SectionRoot>()) != null) {
                    sectionRoot.OnReaved();
                }
            }
            this.currentSection = i;
        } else {
            Debug.LogError("指定が範囲外です(length: " + this.roots.Length + ", i: " + i + ")");
        }
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene("sceneName");
    }

}
