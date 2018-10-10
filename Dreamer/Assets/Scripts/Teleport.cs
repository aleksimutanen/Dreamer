using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    public bool previous;
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