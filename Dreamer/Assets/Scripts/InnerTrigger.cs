using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerTrigger : MonoBehaviour {

    public GameObject mountain;
    public GameObject cave;
    private void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "Player") {
            mountain.gameObject.SetActive(false);
            cave.gameObject.SetActive(true);
            GameManager.instance.switchEnabled = true;
            EarCompass.instance.FindCrystals();
        }
    }
}
