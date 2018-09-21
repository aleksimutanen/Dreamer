using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashCollider : MonoBehaviour {


    void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "Bashable") {
            collision.gameObject.SetActive(false);
        }
    }
}
