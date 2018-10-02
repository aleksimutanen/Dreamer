using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OuterTrigger : MonoBehaviour {
    public GameObject mountain;
    public GameObject cave;

    private void OnTriggerEnter(Collider other){

        if (other.gameObject.tag == "Player"){
            GameManager.instance.SetCheckpoint();
            mountain.gameObject.SetActive(true);
            cave.gameObject.SetActive(false);
            GameManager.instance.switchEnabled = true;
            EarCompass.instance.FindCrystals();
        }

    }
}
