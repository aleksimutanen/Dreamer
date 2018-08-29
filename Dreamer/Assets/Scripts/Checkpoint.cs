using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
        {
        print("Checkpoint");
        if (other.gameObject.tag == "Player")
            GameManager.instance.SetCheckpoint();
        gameObject.SetActive(false);
        }
    }
