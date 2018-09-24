using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideEnabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GameManager.instance.glideEnabled = true;
    }
}
