using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAmmo : MonoBehaviour {

    CharacterMover cm;

    public float timer;
    public float ammoDamage;
    public float ammoSpeed;
    public float ammoGrowingSpeed;

    public LayerMask enemy;

    public Vector3 dir;
    public Vector3 scale;

	void Start () {
        cm = FindObjectOfType<CharacterMover>();
        dir = cm.transform.forward;
	}
	
	void Update () {
        transform.position += dir * Time.deltaTime * ammoSpeed;
        transform.localScale += scale * Time.deltaTime * ammoGrowingSpeed;
        timer -= Time.deltaTime;
        if (timer < 0) {
            gameObject.SetActive(false);
        }
	}

    //public void DealDamage(Bat bat) {
    //    bat.GetComponent<Enemy>().TakeDamage(ammoDamage);
    //    bat.KickBack(dir, pushForce);
    //    gameObject.SetActive(false);
    //}
    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.gameObject.layer == enemy) {
    //        print("ammo hit");
    //        collision.gameObject.GetComponent<Enemy>().TakeDamage(ammoDamage);
    //    }
    //}
}
