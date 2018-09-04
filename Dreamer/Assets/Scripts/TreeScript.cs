using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour, Enemy {

    public float health;
    public float groundCheckDepth;
    public float groundCheckSize;

    public float attackInterval;
    float lastAttack;
    CharacterSkills cs;

    public LayerMask character;

    public bool target;

    public Transform treeRotator;

    Rigidbody rb;
    public float steeringSpeed = 2;

    Quaternion startingRot;
    public Transform twig;

    void Start () {
        cs = FindObjectOfType<CharacterSkills>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        var colliders = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize, character);
        target = colliders.Length > 0;
        if (target) {
            Attack(colliders[0].transform);
        }

    }

    public void Attack(Transform player) {
        treeRotator.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        var targetRot = Quaternion.LookRotation(Vector3.up, treeRotator.forward);
        //tämä ei tule toimimaan koska se haluaa kääntää puun forwardin kohti targetRotin forwardia...
        rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRot, Time.deltaTime * steeringSpeed);

        TwigSwish();

        if (Time.time > attackInterval + lastAttack) {
            var b = cs.Shield();
            //TODO: jos oksa osuu
            if (b) {
                GameManager.instance.ChangeBuddyPower(+5);
                lastAttack = Time.time;
                print("not");
                return;
            } else {
                GameManager.instance.ChangeToddlerHealth(-1f);
                print("attacking");
                lastAttack = Time.time;
            }
        }
    }

    private void TwigSwish () {
        var maxRotation = 90f;
        var speed = 2f;
        twig.rotation = Quaternion.Euler(maxRotation * Mathf.Sin(Time.time * speed), 0f, 0f);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize);
    }

    public void TakeDamage(float damage) {
        if (health <= 0) return;
        health -= damage;
        if (health <= 0) {
            gameObject.SetActive(false);
        }
    }

    public void Respawn() {
        //jotain?
    }
}
