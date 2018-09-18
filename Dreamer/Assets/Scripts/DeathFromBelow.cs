using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFromBelow : MonoBehaviour {

    private void OnTriggerEnter(Collider other){
        print(other.name);
        if (other.gameObject.tag == "Player"){
                GameManager.instance.ALiveLost();
        }

    }
}
