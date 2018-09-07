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
    bool active;

    public float lastShield;
    public float shieldInterval;
    public float shieldDuration;
    public float powerSphereRadius;
    public float powerSphereDamage;

    public LayerMask enemy;

    void Start() {
        cm = FindObjectOfType<CharacterMover>();
    }

    private void FixedUpdate() {
        if (GameManager.instance.bashEnabled) {
            Bash();
        }
        if (bashing) {
            activeTime -= Time.deltaTime;
            cm.Bash();
            if (activeTime < 0) {
                bashing = false;
                bashCollider.SetActive(false);
                activeTime = 0.2f;
            }
        }
        ReleasePower();
    }

    void Update() {
        if (GameManager.instance.glideEnabled) {
            Glide();
        }
        if (GameManager.instance.shieldEnabled) {
            Shield();
        }
        ChargePower();
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
    public void ReleasePower() {

        var powerSphere = Physics.OverlapSphere(transform.position, powerSphereRadius, enemy);
        bool hit = powerSphere.Length > 0;
        if (Input.GetButtonDown("Action")) {
            print("action button pressed");
        }
        if (Input.GetButtonDown("Action") && hit) {
            print("hit an enemy");
            foreach(Collider enemy in powerSphere) {
                enemy.gameObject.GetComponentInParent<Enemy>().TakeDamage(powerSphereDamage);
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, powerSphereRadius);
    }

    public void ChargePower() {
        if (Input.GetButton("Charge") && Input.GetButton("Charge2")) {
            GameManager.instance.ChangeBuddyPower(20f * Time.deltaTime);
        }
    }

    public bool Shield() {
        if (Time.time > shieldInterval + lastShield) {
            if (Input.GetAxis("Shield") > 0.3 && GameManager.instance.buddyPower > 0) {
                active = true;
            }
            if (active) {
                if (shieldDuration > 0) {
                    shieldDuration -= Time.deltaTime;
                    shield.SetActive(true);
                    GameManager.instance.ChangeBuddyPower(-1f * Time.deltaTime);
                    return true;
                } else {
                    active = false;
                    shield.SetActive(false);
                    shieldDuration = 1f;
                    lastShield = Time.time;
                    return false;
                }
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    public bool Glide() {
        if (Input.GetButtonDown("Jump") && !cm.onGround) {
            floater = true;
        }
        if (Input.GetButton("Jump") && !cm.onGround && GameManager.instance.buddyPower > 0 && floater) {
            cm.gravity = cm.normalGravity / 2;
            floatPiece.SetActive(true);
            GameManager.instance.ChangeBuddyPower(-1f * Time.deltaTime);
            return true;
        } else {
            floater = false;
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


