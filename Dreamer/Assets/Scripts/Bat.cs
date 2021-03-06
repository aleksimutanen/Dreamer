﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BatMode {Hanging, Flying, Attacking, Returning, Animated};
//tarviiko tän olla enum?

public class Bat : MonoBehaviour, Enemy {

    Rigidbody rb;
    public Vector3 startPos;
    public float speed;
    public float steeringSpeed = 80f;
    public BatMode batMode;
    public float returnTime;
    public float returnTimeValue;
    public float attackReturnTime;

    public bool sleeping;

    public bool blockable = true;
    public float attackRadius = 10;
    RaycastHit hit2;
    public Transform playerTransform;
    Vector3 target;
    public float distToPlayer;

    public LayerMask obstacles;
    public float maxDist = 3;
    public bool vaistaa;
    float angle;

    public LayerMask floor;
    public float maxDistToFloor = 2;
    public float steerSpeedFloor = 200f;

    //public float explosionDelay = 1f;
    //float countdown;
    public GameObject explosionEffect;
    public float blastRadius = 15f;
    public float health;
    public float healthValue;
    public bool hasExploded;

    public float dmgToPlayer = -5;
    public float pwrToShield = 5;

    float i;
    public float timeSinceDeath;

    BatMode prevMode;

    public GameObject batInDream;
    public GameObject batInNightmare;
    public GameObject batSleeping;

    //sillai että lepakon täytyy päästä puuhun takas unessa

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
       // countdown = explosionDelay;
        rb = GetComponent<Rigidbody>();
        angle = (float)Random.Range(0, 8) * 45f;
        if(startPos == Vector3.zero) {
            startPos = transform.position;
        }
        target = playerTransform.position;
        //20 % chance of an unblockable attack
        i = Random.Range(0f, 1f);
        returnTime = 0;
        health = healthValue;
    }

    void Update() {
        if (health <= 0 && !hasExploded) {
            Explode();
        }
        if (hasExploded)
            timeSinceDeath += Time.deltaTime;

        if (batMode == BatMode.Returning) {
            returnTime -= Time.deltaTime;
        }

        if (distToPlayer > 20 && timeSinceDeath > 15) {
            Respawn();
        }

        if(!sleeping && !batInDream.activeSelf) {
            batInDream.SetActive(true);
            batInNightmare.SetActive(true);
            batSleeping.SetActive(false);
        }
    }

    void FixedUpdate() {

        distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        // switch states?
        if (WorldSwitch.instance.state == AwakeState.Nightmare && !sleeping) {

            if (distToPlayer < attackRadius && (batMode != BatMode.Returning || returnTime <= returnTimeValue - attackReturnTime)) {
                batMode = BatMode.Attacking;

                if (i < 0.2f) {
                    //animaatio/muu indikaatio että on tulossa unblockable
                    var newExplosion = Instantiate(explosionEffect, transform.position, transform.rotation); //placeholder
                    Destroy(newExplosion, 2);
                    blockable = false;
                } else {
                    blockable = true;
                }
            }
            else if (rb.position != startPos && returnTime > 0) {
                batMode = BatMode.Returning;
                i = Random.Range(0f, 1f);
            } else {
                batMode = BatMode.Flying;
            }
        }  else { 
            batMode = BatMode.Hanging;
        }

        if (batMode != BatMode.Hanging && batMode != BatMode.Animated) {
            if (batMode == BatMode.Attacking) {
                target = playerTransform.position;
                returnTime = returnTimeValue;
            }
            else if (batMode == BatMode.Returning) {
                target = startPos;

            }
            Fly();
        } else {
            rb.velocity = Vector3.zero;
        }

        prevMode = batMode;
    }

    void Fly() {

        RaycastHit hit;

        Physics.Raycast(transform.position, transform.forward, out hit, maxDist, obstacles);
        //Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxDist /*Mathf.Infinity*/, obstacles);
        hit2 = hit;

        //testataan onko törmäämässä maahan
        if (RayCone(floor, maxDistToFloor, steerSpeedFloor)) {
            vaistaa = true;
            rb.velocity = transform.forward * -speed;
        }

        else if (hit.collider != null && hit.distance < maxDist) {
            vaistaa = true;
            RayCone(obstacles, maxDist, steeringSpeed);
        }

        else if (RayCone(obstacles, maxDist, steeringSpeed)) {
            vaistaa = true;
        }

        else if (batMode == BatMode.Flying) {
            var targetPoint = transform.position + transform.right * 0.2f;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            targetRotation = Quaternion.Euler(-targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, -targetRotation.eulerAngles.z);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, Time.deltaTime * steeringSpeed);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
            vaistaa = false;
            rb.velocity = transform.forward * speed;
        }

        else {
            vaistaa = false;
            // steer towards target
            var dir = target - transform.position;
            var targetRot = Quaternion.LookRotation(dir, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRot, Time.deltaTime * steeringSpeed);
        }

        rb.velocity = transform.forward * speed;

    }

    //maan ja muiden esteiden ero = eri layereilla, maahan lyhemmät rayt, jos maa pitää hidastaa mut kääntyä pois maasta nopeemmin
    bool RayCone(LayerMask lm, float md, float strSpeed) {
        RaycastHit info;
        int kerroin;
        if (lm == floor) {
            kerroin = 2;
        }
        else {
            kerroin = 4;
        }

        for (int i = 0; i < 8; i++) {

            Vector3 d = (kerroin * transform.forward + transform.up).normalized;
            //käännetään d-vektoria z-akselin ympäri
            angle = i * 45f;
            var qq = Quaternion.AngleAxis(angle, transform.forward);
            d = qq * d;
            Ray r = new Ray(transform.position, d);
            Physics.Raycast(r, out info, md, lm);

            if (info.collider != null) {
                Debug.DrawLine(transform.position, transform.position + d * md, Color.red);
                //ja sitten i+4 on se mihin suuntaan käännytään
                if (angle > 3) {
                    angle = (i - 3) * 45f;
                }
                else {
                    angle = (i + 3) * 45f;
                }
                qq = Quaternion.AngleAxis(angle, transform.forward);
                d = qq * (transform.forward + transform.up).normalized;
                var q = Quaternion.LookRotation(d, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, q, Time.deltaTime * strSpeed);
                //rb.rotation = q * rb.rotation;

                return true;
            }
            else {
                Debug.DrawLine(transform.position, transform.position + d * md, Color.blue);
            }
        }
        return false;
    }

    public void Explode() {
        //kun pelaaja lyö, räjähdä
        hasExploded = true;
        //räjähdysefekti
        Instantiate(explosionEffect, transform.position, transform.rotation);
        //kerrotaan lähellä oleville objekteille että niidenkin pitää räjähtää
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObj in nearbyObjects) {
            print(nearbyObj.tag);
            if (nearbyObj.tag == "Cavestone") {
                var e = Instantiate(explosionEffect, nearbyObj.transform.position, nearbyObj.transform.rotation);
                var expl = nearbyObj.GetComponent<Explodable>();
                if (expl != null) {
                    expl.SetExplosion();
                }
                GameObject.Destroy(e, 2);
            }
        }
        //jos pelaaja lähellä, damagea pelaajaan / kilven latausta jos kilpi ylhäällä?

        //gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
        sleeping = true;
        timeSinceDeath = 0;
    }

    private void OnTriggerEnter(Collider collision) {

        if (WorldSwitch.instance.state == AwakeState.Nightmare) {
            if (collision.gameObject.name == "Shield" && blockable) {
                print("blocked");
                GameManager.instance.ChangeBuddyPower(pwrToShield);
                //blocked --> return
                batMode = BatMode.Returning;
            }
            else if (collision.gameObject.name == "Ammo(Clone)") {
                var ammo = collision.GetComponent<EnergyAmmo>();
                TakeDamage(ammo.ammoDamage);
                KickBack(ammo.dir, 500);
                ammo.gameObject.SetActive(false);
                print("ammo hit");
            }
            else if (collision.gameObject.layer == 13) {
                print("bat hit player");
                GameManager.instance.ChangeToddlerHealth(dmgToPlayer);
                batMode = BatMode.Returning;
                returnTime = returnTimeValue;
            }
        }
    }

    public void KickBack(Vector3 dir, float force) {
        rb.AddForce(dir * force, ForceMode.Impulse);
        //vaihda lepakon liikkumismode 
        batMode = BatMode.Animated;
    }
    //kun kilvellä ammutaan

    private void OnDrawGizmosSelected() {
        //Gizmos.color = Color.red;
        //Debug.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }

    public void TakeDamage(float damage) {
        if (health <= 0) return;
        health -= damage;
    }

    public void Respawn() {
        //jotain?
        //että jos kuolemasta on mennyt tarpeeksi kauan tai jotain niin respawn?
        GetComponent<MeshRenderer>().enabled = true;
        hasExploded = false;
        sleeping = false;
        health = healthValue;
    }
}