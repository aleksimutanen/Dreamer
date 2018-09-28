using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GameManager.instance.TeleportToCheckPoint(gameObject.layer != LayerMask.NameToLayer("NightmareLayer"));
    }
}
