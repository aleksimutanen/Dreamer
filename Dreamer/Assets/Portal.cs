using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    GameObject player;
    public Transform caveStart;

    private void Start() {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        
    }

    private void Update() {
        if (GameManager.instance.bashEnabled) {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            player = other.GetComponentInParent<CharacterMover>().gameObject;
            player.SetActive(false);
            player.transform.position = caveStart.position;
            player.SetActive(true);
        }
    }
}