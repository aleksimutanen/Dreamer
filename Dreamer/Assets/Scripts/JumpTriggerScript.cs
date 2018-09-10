using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Player"){
            GameManager.instance.SetCheckpoint();
            GameManager.instance.jumpEnabled = true;

            if (GameManager.instance.tutorialIndex == 3 /*&& GameManager.instance.statusTextTimer < 0/* && GameManager.instance.statusTextEmpty*/){
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[GameManager.instance.tutorialIndex], 5);
                GameManager.instance.tutorialIndex++;
                GameManager.instance.statusTextEmpty = false;
                gameObject.SetActive(false);
            }
        }
    }
}
