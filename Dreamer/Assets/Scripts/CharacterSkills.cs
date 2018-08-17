using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {

    public GameObject floatPiece;
    public GameObject shield;
    CharacterMover cm;

    void Start() {
        cm = FindObjectOfType<CharacterMover>();
        //Input.GetKeyDown(KeyCode.j)
    }

    // Update is called once per frame
    void Update() {
        LessGravity();
        Shield();
    }

    bool Shield() {
        if (Input.GetAxis("Shield") > 0.3) {
            print("shield");
            shield.SetActive(true);
            return true;
        } else {
            shield.SetActive(false);
            return false;
        }
    }

    bool LessGravity() {
        if (Input.GetAxis("LessGravity") > 0 && cm.onGround) {
            cm.gravity = cm.normalGravity / 2;
            floatPiece.SetActive(true);
            return true;
        } else {
            cm.gravity = cm.normalGravity;
            floatPiece.SetActive(false);
            return false;
        }
    }
}


