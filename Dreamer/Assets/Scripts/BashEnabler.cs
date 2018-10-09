using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashEnabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
            GameManager.instance.bashEnabled = true;
    }
}
