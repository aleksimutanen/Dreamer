using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public bool previous;

    private void Start() {
        if (gameObject.name == "Portal") {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Update() {
        if (gameObject.name == "Portal" && GameManager.instance.bashEnabled) {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (gameObject.name == "puzzle1_nightmareTeleport") {
                if (WorldSwitch.instance.state == AwakeState.Dream) {
                    return;
                }
                else {
                    print(other.gameObject.name);
                    GameManager.instance.TeleportToCheckPoint(gameObject.layer != LayerMask.NameToLayer("NightmareLayer") || previous, previous);
                }
            }
            print(other.gameObject.name);
            GameManager.instance.TeleportToCheckPoint(gameObject.layer != LayerMask.NameToLayer("NightmareLayer") || previous, previous);
        }
    }
}