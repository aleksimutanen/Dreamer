using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {

    public GameObject floatPiece;
    public GameObject shield;
    CharacterMover cm;
    bool floater;

    void Start() {
        cm = FindObjectOfType<CharacterMover>();
    }

    void Update() {
        LessGravity();
        Shield();
    }

    bool Shield() {
        if (Input.GetAxis("Shield") > 0.3) {
            shield.SetActive(true);
            return true;
        } else {
            shield.SetActive(false);
            return false;
        }
    }

    bool LessGravity() {
        //if (Input.GetButtonDown("Jump KB") && !cm.onGround) {
        //    floater = true;
        //}
        //if (Input.GetButton("") && !cm.onGround /*&& floater*/) {
        //    cm.gravity = cm.normalGravity / 2;
        //    floatPiece.SetActive(true);
        //    return true;
        //}
        if (Input.GetAxis("LessGravity") > 0 && !cm.onGround) {
            cm.gravity = cm.normalGravity / 2;
            floatPiece.SetActive(true);
            return true;
        } else {
            cm.gravity = cm.normalGravity;
            floatPiece.SetActive(false);
            //floater = false;
            return false;
        }
    }
}


