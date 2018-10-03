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
            GameManager.instance.switchEnabled = false;
            EarCompass.instance.FindCrystals();
            if(WorldSwitch.instance.state == AwakeState.Nightmare){
                WorldSwitch.instance.switchNow = true;
            }
        }
    }
}
