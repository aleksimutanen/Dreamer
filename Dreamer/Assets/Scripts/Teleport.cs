using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public bool previous;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
            GameManager.instance.TeleportToCheckPoint(gameObject.layer != LayerMask.NameToLayer("NightmareLayer")||previous, previous);
    }
}