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

    Rigidbody rb;
    public float steeringSpeed = 2;

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
        var dir = player.position - transform.position;
        //var targetRot
            rb.rotation = Quaternion.LookRotation(dir, Vector3.up);
        //rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRot, Time.deltaTime * steeringSpeed);

        if (Time.time > attackInterval + lastAttack) {
            var b = cs.Shield();
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
