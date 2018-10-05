using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoader : MonoBehaviour {
    public GameObject enableThese;
    public GameObject disableThese;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            print("moi");
            enableThese.gameObject.SetActive(true);
            //SetChildrenActive(true);
            //tag? layer won't work
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            print("heihei");
            disableThese.gameObject.SetActive(false);
        }
    }
}
