using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamCrystalScript : MonoBehaviour {
    public GameObject crystalNagger;
    // Crystal collection
    private void OnTriggerEnter(Collider other) {
        GameManager.instance.ChangeDreamPower(1);
        gameObject.SetActive(false);
        if(crystalNagger!=null){
            crystalNagger.gameObject.SetActive(false);
        }
    }
}
