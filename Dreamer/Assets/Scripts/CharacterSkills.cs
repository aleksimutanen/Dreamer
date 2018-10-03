using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour {

    CharacterMover cm;

    public GameObject glider;
    public GameObject shield;
    public GameObject powerSphereBall;
    public GameObject bashCollider;
    public GameObject ammo;
    public GameObject ammoFolder;

    public float glideTimer;
    public float maxGlideTimer;

    bool floater;
    bool bashing;
    bool charged;
    public bool shieldHit;
    bool powerSphereActive;

    public float activeTime;
    
    public bool shieldActive;
    public float shieldDuration;
    public float maxShieldDuration;

    public float powerSphereRadius;
    public float powerSpherePushForce;
    public float powerSphereDamage;
    public float powerSphereInterval;
    float lastPowerSphere;
    float powerSphereActiveTimer = 0.5f;

    public float buddyPowerPerAmmo;
    public float firingInterval;
    float lastShot;

    public LayerMask enemy;

    Vector3 fireOffset = new Vector3(0, 2, 0);

    public ParticleSystem charge;
    ParticleSystem.EmissionModule chargeEmitter;

    public ParticleSystem fire;
    ParticleSystem.EmissionModule fireEmitter;

    public ParticleSystem powerSphere;
    ParticleSystem.EmissionModule sphereEmitter;

    public ParticleSystem bounce;
    ParticleSystem.EmissionModule bounceEmitter;

    void Start() {
        cm = FindObjectOfType<CharacterMover>();
        sphereEmitter = powerSphere.emission;
        sphereEmitter.enabled = false;
        chargeEmitter = charge.emission;
        chargeEmitter.enabled = false;
        fireEmitter = fire.emission;
        fireEmitter.enabled = false;
        bounceEmitter = bounce.emission;
        bounceEmitter.enabled = false;
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
        if (Time.time > firingInterval + lastShot && (Input.GetAxis("Fire") > 0.5f|| Input.GetButtonDown("Fire4")) && GameManager.instance.buddyPower > 0) {
                Fire();
        } else {
            fireEmitter.enabled = false;
        }
        if (powerSphereActive && powerSphereActiveTimer > 0) {
            powerSphereActiveTimer -= Time.deltaTime;
            powerSphereBall.transform.localScale += new Vector3(100,100,100) * Time.deltaTime;
            powerSphereBall.SetActive(true);
        } else {
            powerSphereActiveTimer = 0.2f;
            powerSphereActive = false;
            powerSphereBall.transform.localScale = new Vector3(1, 1, 1);
            powerSphereBall.SetActive(false);
        }

        if(Input.GetButtonDown("Shield")){
            shieldHit = false;
            shieldDuration = 2;
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
            GameObject go = Instantiate(ammo, transform.position + fireOffset, transform.rotation);
            go.transform.parent = ammoFolder.transform;
            lastShot = Time.time;
            fireEmitter.enabled = true;
            GameManager.instance.ChangeBuddyPower(-buddyPowerPerAmmo);
        }
    }

    public void ReleasePower() {
        if (GameManager.instance.powerBallEnabled) {
            var powerSphere = Physics.OverlapSphere(transform.position, powerSphereRadius, enemy);
            bool hit = powerSphere.Length > 0;
            if (Time.time > powerSphereInterval + lastPowerSphere) {
                if (Input.GetAxisRaw("PowerSphere") > 0.1f && hit) {
                    powerSphereActive = true;
                    sphereEmitter.enabled = true;
                    print("hit an enemy");
                    lastPowerSphere = Time.time;
                    foreach (Collider enemy in powerSphere) {
                        enemy.gameObject.GetComponentInParent<Enemy>().TakeDamage(powerSphereDamage);
                        if (enemy.gameObject.GetComponent<Bat>() != null) {
                            enemy.GetComponent<Bat>().KickBack(-enemy.transform.forward, powerSpherePushForce);
                        }
                    }
                } else if (Input.GetAxis("PowerSphere") > 0.1f && !hit) {
                    powerSphereActive = true;
                    sphereEmitter.enabled = true;
                    lastPowerSphere = Time.time;
                }
            } else {
                sphereEmitter.enabled = false;
                powerSphereBall.SetActive(false);
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, powerSphereRadius);
    }

    public void ChargePower() {
        if (Input.GetButton("Charge")) {
            GameManager.instance.ChangeBuddyPower(20f * Time.deltaTime);
            chargeEmitter.enabled = true;
        } else {
            chargeEmitter.enabled = false;
        }
    }


    public bool Shield() {
        if(Input.GetButton("Shield") && shieldDuration > 0 && GameManager.instance.toddlerMoving == false && shieldHit == false) {
            shieldDuration -= Time.deltaTime;
            shield.SetActive(true);
            shieldActive = true;
            return true;
        } else {
            bounceEmitter.enabled = false;
            shield.SetActive(false);
            shieldActive = false;
            return false;
        }
    }

    public void HitShield(){
        shieldHit = true;

    }

    public bool Glide() {
        if (Input.GetButtonDown("Jump") && /*!cm.onGround*/ !cm.canJump) {
            floater = true;
        }
        if (Input.GetButton("Jump") && /*!cm.onGround*/ !cm.canJump && floater && glideTimer > 0) {
            glideTimer -= Time.deltaTime;
            cm.gravity = cm.normalGravity / 2;
            glider.SetActive(true);
            return true;
        } else {
            floater = false;
            cm.gravity = cm.normalGravity;
            glider.SetActive(false);
            return false;
        }
    }

    public void Bash() {
        if (Input.GetButton("Bash")) {
            bashCollider.SetActive(true);
            GameManager.instance.ChangeBuddyPower(-100f);
            print("bashed");
            bashing = true;
        }
    }
}


