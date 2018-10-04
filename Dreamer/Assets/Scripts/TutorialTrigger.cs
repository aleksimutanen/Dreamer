using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Player") {

            if(gameObject.name == "TutorialJumpTrigger") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[3], 5); 
                GameManager.instance.jumpEnabled = true;
                GameManager.instance.SetCheckpoint();
                gameObject.SetActive(false);
            }
            if(gameObject.name == "TutorialCrystalTrigger") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[4], 5);
                GameManager.instance.switchEnabled = true;
                GameManager.instance.SetCheckpoint();
                gameObject.SetActive(false);
            }
            if(gameObject.name == "TutorialSwitchTrigger") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[5], 5);
                GameManager.instance.SetCheckpoint();
                gameObject.SetActive(false);
            }
            if(gameObject.name == "TutorialCrystalTrigger2") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[6], 5);
                GameManager.instance.SetCheckpoint();
                gameObject.SetActive(false);
            }
            if(gameObject.name == "BashEnabler") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[7], 5);
                GameManager.instance.bashEnabled = true;
                gameObject.SetActive(false);
            }
            if(gameObject.name == "GlideEnabler") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[8], 5);
                GameManager.instance.glideEnabled = true;
                gameObject.SetActive(false);
            }
            if(gameObject.name == "ReflectionEnabler") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[9], 5);
                GameManager.instance.reflectionEnabled = true;
                gameObject.SetActive(false);
            }
            if(gameObject.name == "TutorialDoorCrystalInfo" && GameManager.instance.crystalAmount<10) {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[10], 5);
                GameManager.instance.SetCheckpoint();
                GameManager.instance.sleepingBat.sleeping = false;
                gameObject.SetActive(false);
            }
            if(gameObject.name == "CheckpointFirstTree" && GameManager.instance.crystalAmount < 10) {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[12], 5);
                GameManager.instance.SetCheckpoint();

                GameManager.instance.shieldEnabled = true;
                GameManager.instance.firingEnabled = true;
                gameObject.SetActive(false);
            }
            if(gameObject.name == "CheckpointFirstBat" && GameManager.instance.crystalAmount < 10) {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[13], 5);
                GameManager.instance.SetCheckpoint();
                GameManager.instance.sleepingBat.sleeping = false;
                gameObject.SetActive(false);
            }
        }
    }
}
