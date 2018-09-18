using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Player") {
            GameManager.instance.SetCheckpoint();


            if(GameManager.instance.tutorialIndex == 3 /*&& GameManager.instance.statusTextTimer < 0/* && GameManager.instance.statusTextEmpty*/) {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[GameManager.instance.tutorialIndex], 5); 
                GameManager.instance.statusTextEmpty = false;
                gameObject.SetActive(false);
                GameManager.instance.jumpEnabled = true;
            }
            if(GameManager.instance.tutorialIndex == 4 /*&& GameManager.instance.statusTextTimer < 0/* && GameManager.instance.statusTextEmpty*/) {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[GameManager.instance.tutorialIndex], 5);
                GameManager.instance.statusTextEmpty = false;
                gameObject.SetActive(false);

            }
            if(GameManager.instance.tutorialIndex == 5 /*&& GameManager.instance.statusTextTimer < 0/* && GameManager.instance.statusTextEmpty*/) {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[GameManager.instance.tutorialIndex], 5);
                GameManager.instance.statusTextEmpty = false;
                gameObject.SetActive(false);
                GameManager.instance.switchEnabled = true;
            }
            if(GameManager.instance.tutorialIndex == 6 /*&& GameManager.instance.statusTextTimer < 0/* && GameManager.instance.statusTextEmpty*/) {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[GameManager.instance.tutorialIndex], 5);
                GameManager.instance.statusTextEmpty = false;
                gameObject.SetActive(false);
            }
            GameManager.instance.tutorialIndex++;
        }
    }
}
