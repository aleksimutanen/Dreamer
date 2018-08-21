using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour {

    // Crystal collection
    private void OnTriggerEnter(Collider other) {
        GameManager.instance.ChangeDreamPower(1);
        gameObject.SetActive(false);
    }
}
