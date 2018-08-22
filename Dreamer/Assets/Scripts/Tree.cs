using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public string character;
    public float attackInterval;
    float lastAttack;
    CharacterSkills cs;

	void Start () {
        cs = FindObjectOfType<CharacterSkills>();
	}
	
	void Update () {
		
	}

    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == character) {
            if (Time.time > attackInterval + lastAttack) {
                //dosomething
                //if (cs.)
                GameManager.instance.ChangeToddlerHealth(1f);
                lastAttack = Time.time;
            }
        }    
    }
}
