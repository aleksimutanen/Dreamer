using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAmmo : MonoBehaviour {

    CharacterMover cm;

    public float timer;
    public float ammoDamage;
    public float ammoSpeed;
    public float pushForce;

    public Vector3 dir;

	void Start () {
        cm = FindObjectOfType<CharacterMover>();
        dir = cm.transform.forward;
	}
	
	void Update () {
        transform.position += dir * Time.deltaTime * ammoSpeed;
        timer -= Time.deltaTime;
        if (timer < 0) {
            gameObject.SetActive(false);
        }
	}

}
