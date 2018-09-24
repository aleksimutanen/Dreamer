using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoader : MonoBehaviour {

    void SetChildrenActive(bool active) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(active);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 10) {
            print("moi");
            //SetChildrenActive(true);
            //tag? layer won't work
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == 10) {
            print("heihei");
            SetChildrenActive(false);
        }
    }
}
