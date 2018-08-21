using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyCrystalScript : MonoBehaviour {

    // Crystal collection
    private void OnTriggerEnter(Collider other) {
        GameManager.instance.ChangeBuddyPower(1);
        gameObject.SetActive(false);
    }
}
