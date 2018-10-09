using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringEnabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
            GameManager.instance.firingEnabled = true;
    }
}
