using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {

    public float activeTime;
    public float chargeTime;

    public GameObject floatPiece;
    public GameObject shield;
    public GameObject bashCollider;
    public GameObject ammo;
    public GameObject ammoFolder;

    CharacterMover cm;

    bool floater;
    bool bashing;
    bool charged;
    bool active;

    public float lastShield;
    public float shieldInterval;
    public float shieldDuration;

    public float powerSphereRadius;
    public float powerSpherePushForce;
    public float powerSphereDamage;

    public float firingInterval;
    float lastShot;

    public LayerMask enemy;

    Vector3 fireOffset = new Vector3(0, 2, 0);

    public ParticleSystem ps;

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
        if (Input.GetButtonDown("Bash")) {
            Fire();
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

    public void Fire() {
        if (GameManager.instance.firingEnabled) {
            if (Time.time > firingInterval + lastShot) {
                GameObject go = Instantiate(ammo, transform.position + fireOffset, transform.rotation);
                go.transform.parent = ammoFolder.transform;
                lastShot = Time.time;
            }
        }
    }

    public void ReleasePower() {
        if (GameManager.instance.powerBallEnabled) {
            var powerSphere = Physics.OverlapSphere(transform.position, powerSphereRadius, enemy);
            bool hit = powerSphere.Length > 0;
            if (Input.GetButtonDown("Action") && hit) {
                print("hit an enemy");
                foreach (Collider enemy in powerSphere) {
                    enemy.gameObject.GetComponentInParent<Enemy>().TakeDamage(powerSphereDamage);
                    if (enemy.gameObject.GetComponent<Bat>() != null) {
                        enemy.GetComponent<Bat>().KickBack(-enemy.transform.forward, powerSpherePushForce);
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, powerSphereRadius);
    }

    public void ChargePower() {
        var em = ps.emission;
        if (Input.GetButton("Charge") /*&& Input.GetButton("Charge2"*/) {
            GameManager.instance.ChangeBuddyPower(20f * Time.deltaTime);
            //ps.emission.rateOverTime = 10;
            //ps.gameObject.SetActive(true);
            em.enabled = true;
            //ps.Play();
        } else {
            em.enabled = false;
            //ps.gameObject.SetActive(false);
            //ps.Stop();
        }
    }

    public bool Shield() {
        if (Time.time > shieldInterval + lastShield) {
            if(Input.GetAxis("Shield") > 0.3 && GameManager.instance.buddyPower > 0) {
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
        if (Input.GetButton("Bash")) {
            chargeTime -= Time.deltaTime;
            print("charging");
            if (chargeTime < 0) {
                charged = true;
            }
        } else {
            //chargeTime = 2f;
        }
        if (chargeTime < 0 && !Input.GetButton("Bash") && !Input.GetButton("Bash2") && charged) {
            bashCollider.SetActive(true);
            GameManager.instance.ChangeBuddyPower(-100f);
            print("bashed");
            chargeTime = 2f;
            bashing = true;
            charged = false;
        }
    }
}


