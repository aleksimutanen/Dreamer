using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {

    public GameObject floatPiece;
    public GameObject shield;
    CharacterMover cm;
    bool floater;

    public bool shieldUnlocked;
    public bool glideUnlocked;

    void Start() {
        cm = FindObjectOfType<CharacterMover>();
    }

    void Update() {
        if (glideUnlocked) {
            Glide();
        }
        if (shieldUnlocked) {
            Shield();
        }
    }

    //bool Shield() {
    //    if (Input.GetAxis("Shield") > 0.3) {
    //        shield.SetActive(true);
    //        return true;
    //    } else {
    //        shield.SetActive(false);
    //        return false;
    //    }
    //}

    //bool LessGravity() {
    //    //if (Input.GetButtonDown("Jump KB") && !cm.onGround) {
    //    //    floater = true;
    //    //}
    //    //if (Input.GetButton("") && !cm.onGround /*&& floater*/) {
    //    //    cm.gravity = cm.normalGravity / 2;
    //    //    floatPiece.SetActive(true);
    //    //    return true;
    //    //}
    //    if (Input.GetAxis("LessGravity") > 0 && !cm.onGround) {
    //        cm.gravity = cm.normalGravity / 2;
    //        floatPiece.SetActive(true);
    //        return true;
    //    } else {
    //        cm.gravity = cm.normalGravity;
    //        floatPiece.SetActive(false);
    //        //floater = false;
    //        return false;
    //    }
    //}

    public bool Shield() {
        if (Input.GetAxis("Shield") > 0.3 && GameManager.instance.buddyPower > 0) {
            shield.SetActive(true);
            GameManager.instance.ChangeBuddyPower(-1f * Time.deltaTime);
            return true;
        } else {
            shield.SetActive(false);
            return false;

        }
    }

    public bool Glide() {
        if (Input.GetAxis("LessGravity") > 0 && !cm.onGround && GameManager.instance.buddyPower > 0) {
            cm.gravity = cm.normalGravity / 2;
            floatPiece.SetActive(true);
            GameManager.instance.ChangeBuddyPower(-1f * Time.deltaTime);
            return true;
        } else {
            cm.gravity = cm.normalGravity;
            floatPiece.SetActive(false);
            return false;
        }
    }
}


