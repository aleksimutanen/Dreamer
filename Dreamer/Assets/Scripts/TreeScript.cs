﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    public float groundCheckDepth;
    public float groundCheckSize;

    public float attackInterval;
    float lastAttack;
    CharacterSkills cs;

    public LayerMask character;

    public bool target;

    void Start () {
        cs = FindObjectOfType<CharacterSkills>();
    }

    void FixedUpdate() {
        var colliders = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize, character);
        target = colliders.Length > 0;
        if (target) {
            Attack();
        }
    }

    public void Attack() {
        //if (other.gameObject.name == "NightmareCollider") {
        if (Time.time > attackInterval + lastAttack) {
            var b = cs.Shield();
            if (b) {
                lastAttack = Time.time;
                print("not");
                return;
            } else {
                GameManager.instance.ChangeToddlerHealth(-1f);
                print("attacking");
            }
            lastAttack = Time.time;
            //}
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize);
    }
}
