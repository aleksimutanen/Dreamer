using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour, Enemy {

    public List<GameObject> trees = new List<GameObject>();

    public float health;
    public float attackTriggerHeight;
    public Vector3 attackTriggerSize;
    public Animator anim;
    public float attackInterval;
    public float middleShift;
    public float forwardShift;
    Vector3 boxLocation1;
    Vector3 boxLocation2;
    Vector3 boxLocation3;

    float lastAttack;
    CharacterSkills cs;

    public LayerMask character;

    public bool target;

    // tarviiks? Rigidbody rb;

    Quaternion startingRot;

    public float dmgToPlayer = -5;
    public float pwrToShield = 5;

    public ParticleSystem deathEffect;
    public bool ded;
    public GameObject dedInDream;

    bool nightmareTree;
    bool dreamTree;



    void Start() {
        cs = FindObjectOfType<CharacterSkills>();
    }

    /*void Update() {
        // haluaisin laittaa aktiiviseksi vihollispuun painajaisessa
        // ja ei-vihollispuun unessa
        nightmareTree = trees[0].activeSelf;
        dreamTree = trees[1].activeSelf;

        if (ded && WorldSwitch.instance.state == AwakeState.Dream && !dedInDream.activeSelf) {
            dedInDream.SetActive(true);
        }
        else if (!ded && WorldSwitch.instance.state == AwakeState.NightMare && (dreamTree || !nightmareTree)) {
            trees[0].SetActive(true);
            //rb = trees[0].GetComponent<Rigidbody>();
            trees[1].SetActive(false);
        } else if (!ded && WorldSwitch.instance.state == AwakeState.Dream && (!dreamTree || nightmareTree)) {
            trees[1].SetActive(true);
            trees[0].SetActive(false);
        }      
    }*/

    void FixedUpdate() {
        if (!ded) {

            if(Time.time > attackInterval + lastAttack){
                //var colliders = Physics.OverlapSphere(transform.position - Vector3.up * attackTriggerHeight, attackTriggerSize, character);
                boxLocation1 = transform.position + transform.right * attackTriggerSize.x * 2 + transform.forward * forwardShift;
                boxLocation2 = transform.position + transform.forward * middleShift * forwardShift;
                boxLocation3 = transform.position - transform.right * attackTriggerSize.x * 2 + transform.forward * forwardShift;
                var colliders1 = Physics.OverlapBox(boxLocation1, attackTriggerSize, Quaternion.identity, character);
                var colliders2 = Physics.OverlapBox(boxLocation2, attackTriggerSize, Quaternion.identity, character);
                var colliders3 = Physics.OverlapBox(boxLocation3, attackTriggerSize, Quaternion.identity, character);

                if(colliders1.Length > 0) {
                    anim.Play("AttackMiddle1");
                    Attack();
                }
                if(colliders2.Length > 0) {

                    if (Random.value < .5f)
                        anim.Play("AttackMiddle1");
                    else
                        anim.Play("AttackMiddle2");
                    Attack();
                }
                if(colliders3.Length > 0) {
                    anim.Play("AttackMiddle2");
                    Attack();
                }
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

        if (Time.time > attackInterval + lastAttack) {

            var b = cs.Shield();
            //TODO: jos oksa osuu
            if (b) {
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


    public void TakeDamage(float damage) {
        if (health <= 0) return;
        health -= damage;
        if (health <= 0) {
            print("tree ded");
            //TODO: death animation yms
            deathEffect.Play();
            ded = true;
            trees[0].SetActive(false);
        }
    }

    public void Respawn() {
        //jotain?
    }
}
