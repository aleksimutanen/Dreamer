﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour, Enemy {

    public List<GameObject> trees = new List<GameObject>();

    public GameObject explosionEffect;
    float lastAttack;
    public float attackTriggerHeight;
    public Vector3 attackTriggerSize;
    public Animator anim;
    public float attackInterval;
    public float middleShift;
    public float forwardShift;
    Vector3 boxLocation1;
    Vector3 boxLocation2;
    Vector3 boxLocation3;

    CharacterSkills cs;
    public LayerMask character;
    float dmgToPlayer = -20;
    float pwrToShield = 50;

    public float health;
    public bool playerCanHit;
    public ParticleSystem deathEffect;
    public bool ded;
    public GameObject dedInDream;
    public bool blockable = true;

    bool transitionIn;

    void Start() {
        cs = FindObjectOfType<CharacterSkills>();
    }

    void Update() {
        // haluaisin laittaa aktiiviseksi vihollispuun painajaisessa
        // ja ei-vihollispuun unessa
        transitionIn = WorldSwitch.instance.transitionIn;

        if (ded && WorldSwitch.instance.state == AwakeState.Dream  /* && !dedInDream.activeSelf*/) {
            dedInDream.SetActive(true);
        }
        else if (!ded && WorldSwitch.instance.state == AwakeState.Dream && transitionIn /*&& !transitionOut/*&& (dreamTree || !nightmareTree)*/) {
            trees[0].SetActive(true);
            //rb = trees[0].GetComponent<Rigidbody>();
            trees[1].SetActive(false);
        } else if (!ded && WorldSwitch.instance.state == AwakeState.Nightmare && transitionIn /*&& !transitionOut /*&& (!dreamTree || nightmareTree)*/) {
            trees[1].SetActive(true);
            trees[0].SetActive(false);
        }
        
    }

    void FixedUpdate() {

        if(!ded && Time.time > attackInterval + lastAttack){
            //var colliders = Physics.OverlapSphere(transform.position - Vector3.up * attackTriggerHeight, attackTriggerSize, character);
            boxLocation1 = transform.position + transform.right * attackTriggerSize.x * 2 + transform.forward * forwardShift;
            boxLocation2 = transform.position + transform.forward * middleShift * forwardShift;
            boxLocation3 = transform.position - transform.right * attackTriggerSize.x * 2 + transform.forward * forwardShift;
            var colliders1 = Physics.OverlapBox(boxLocation1, attackTriggerSize, Quaternion.identity, character);
            var colliders2 = Physics.OverlapBox(boxLocation2, attackTriggerSize, Quaternion.identity, character);
            var colliders3 = Physics.OverlapBox(boxLocation3, attackTriggerSize, Quaternion.identity, character);

            if(colliders1.Length > 0) {
                anim.Play("AttackLeft");
                blockable = true;
                Attack();
            }
            else if(colliders2.Length > 0) {

                var juttu = Random.value;
                if (juttu < .2f) {
                    anim.Play("Indicator");
                    blockable = false;
                }
                else if (juttu < .6) {
                    anim.Play("AttackMiddle2");
                    blockable = true;
                } else {
                    anim.Play("AttackMiddle1");
                    blockable = true;
                }
                Attack();
            }
            else if(colliders3.Length > 0) {
                anim.Play("AttackRight");
                blockable = true;
                Attack();
            } else {
                playerCanHit = false;
            }

        }
    }
    

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position - Vector3.up * attackTriggerHeight, attackTriggerSize);

        Gizmos.DrawWireCube(boxLocation1, attackTriggerSize * 2);
        Gizmos.DrawWireCube(boxLocation2, attackTriggerSize * 2);
        Gizmos.DrawWireCube(boxLocation3, attackTriggerSize * 2);
    }

    public void Attack() {
        playerCanHit = true;
        if (Time.time > attackInterval + lastAttack) {

            var shieldActive = cs.Shield();
            //TODO: jos oksa osuu
            if (shieldActive && blockable) {
                GameManager.instance.ChangeBuddyPower(pwrToShield);
                lastAttack = Time.time;
                print("not");
                return;
            }
            else {
                GameManager.instance.ChangeToddlerHealth(dmgToPlayer);
                print("attacking");
                lastAttack = Time.time;
            }
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Ammo(Clone)") {
            var ammo = collision.gameObject.GetComponent<EnergyAmmo>();
            TakeDamage(ammo.ammoDamage);
            ammo.gameObject.SetActive(false);
            Instantiate(deathEffect, transform.position, transform.rotation); //tai esim että puu välähtää punaisena?
            print("ammo hit tree");
        }
        //else if (collision.gameObject.layer == 10 || collision.gameObject.layer == 12) {
        //    print("bat hit player");
        //    GameManager.instance.ChangeToddlerHealth(dmgToPlayer);
        //}
    }

    public void TakeDamage(float damage) {
        if (health <= 0) return;
        if (playerCanHit)
            health -= damage;
        if (health <= 0) {
            print("tree ded");
            Instantiate(deathEffect, transform.position, transform.rotation);
            ded = true;
            trees[0].SetActive(false);
        }
    }

}
