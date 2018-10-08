using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalNag : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        GameManager.instance.ChangeStatusText(GameManager.instance.tutorialTexts[15], 5);
    }
}
