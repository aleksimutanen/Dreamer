using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    //public string character;
    public float attackInterval;
    float lastAttack;
    CharacterSkills cs;

    void Start () {
        cs = FindObjectOfType<CharacterSkills>();
    }
	
	void Update () {
		
	}

    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "NightmareCollider") {
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
            }
        }
    }
}
