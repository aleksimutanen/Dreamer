using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Player") {
            GameManager.instance.SetCheckpoint();


            if(gameObject.name == "TutorialJumpTrigger") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[3], 5); 
                gameObject.SetActive(false);
                GameManager.instance.jumpEnabled = true;
            }
            if(gameObject.name == "TutorialCrystalTrigger") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[4], 5);
                gameObject.SetActive(false);
                GameManager.instance.switchEnabled = true;
            }
            if(gameObject.name == "TutorialSwitchTrigger") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[5], 5);
                gameObject.SetActive(false);
            }
            if(gameObject.name == "TutorialCrystalTrigger2") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[6], 5);
                gameObject.SetActive(false);
            }
            if(gameObject.name == "BashEnabler") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[7], 5);
                GameManager.instance.bashEnabled = true;
            }
            if(gameObject.name == "GlideEnabler") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[8], 5);
                GameManager.instance.glideEnabled = true;
            }
            if(gameObject.name == "ReflectionEnabler") {
                GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[9], 5);
                GameManager.instance.reflectionEnabled = true;
            }





        }
    }
}
