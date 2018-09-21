using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashEnabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GameManager.instance.bashEnabled = true;
    }
}
