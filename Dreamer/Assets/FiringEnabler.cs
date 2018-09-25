using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringEnabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GameManager.instance.firingEnabled = true;
    }
}
