using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {

    public float activeTime;
    public float chargeTime;
    public GameObject floatPiece;
    public GameObject shield;
    public GameObject bashCollider;
    CharacterMover cm;
    bool floater;
    bool bashing;
    bool charged;

    public bool shieldUnlocked;
    public bool glideUnlocked;
    public bool bashUnlocked;

    void Start() {
        cm = FindObjectOfType<CharacterMover>();
    }
    private void FixedUpdate() {
        if (bashUnlocked) {
            Bash();
        }
        if (bashing) {
            activeTime -= Time.deltaTime;
            cm.Bash();
            if (activeTime < 0) {
                bashing = false;
                bashCollider.SetActive(false);
            }
        }
    }

    void Update() {
        if (glideUnlocked) {
            Glide();
        }
        if (shieldUnlocked) {
            Shield();
        }
    }

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

    public void Bash() {
        if (Input.GetButton("Bash") && Input.GetButton("Bash2") && GameManager.instance.buddyPower == 100) {
            chargeTime -= Time.deltaTime;
            print("charging");
            if (chargeTime < 0) {
                charged = true;
            }
        } else {
            //chargeTime = 2f;
        }
        if (chargeTime < 0 && !Input.GetButton("Bash") && !Input.GetButton("Bash2") && charged) {
            //cm.Bash();
            bashCollider.SetActive(true);
            GameManager.instance.ChangeBuddyPower(-100f);
            print("bashed");
            chargeTime = 2f;
            bashing = true;
            charged = false;
        }
    }
}


