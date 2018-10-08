using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoader : MonoBehaviour {
    public List<GameObject> enableThese = new List<GameObject>();
    public List<GameObject> disableThese = new List<GameObject>();

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (enableThese.Count > 0) {
                foreach (GameObject obj in enableThese) {
                    obj.SetActive(true);
                }

            }
            if (disableThese.Count > 0) {
                foreach (GameObject obj in disableThese) {
                    obj.SetActive(false);
                }
                //SetChildrenActive(true);
                //tag? layer won't work
            }
        }
    }
}