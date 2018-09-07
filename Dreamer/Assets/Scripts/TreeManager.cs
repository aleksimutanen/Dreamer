using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour, Enemy {

    public List<GameObject> trees = new List<GameObject>();

    public float health;
    public float groundCheckDepth;
    public float groundCheckSize;

    public float attackInterval;
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

    void Update() {
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
    }

    void FixedUpdate() {
        if (!ded) {
            var colliders = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize, character);
            target = colliders.Length > 0;
            if (target) {
                Attack(colliders[0].transform);
            }
        }
    }

    public void Attack(Transform player) {

        TwigSwish(); //tarviiks?

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

    private void TwigSwish() {
        //ANIMATE twig ? anything else?
        //var maxRotation = 90f;
        //var speed = 2f;
        //twig.rotation = Quaternion.Euler(maxRotation * Mathf.Sin(Time.time * speed), 0f, 0f);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize);
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
