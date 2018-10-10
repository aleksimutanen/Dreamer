using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour {

    public bool explodable;
    public GameObject explosionEffect;
    public float countdown = 0.5f;
    Transform[] stones;

    public ParticleSystem stoneExplosion;
    ParticleSystem.EmissionModule stoneEmitter;

    private void Start() {
        stones = GetComponentsInChildren<Transform>();
        stoneEmitter = stoneExplosion.emission;
        stoneEmitter.enabled = false;
    }

    void Update () {
        if (explodable) {
            countdown -= Time.deltaTime;
            stoneEmitter.enabled = true;
            if (countdown <= 0) {
                Explode();
            }
        }
        //countdown -= Time.deltaTime;
        //if (countdown <= 0 && explodable) {
        //    stoneEmitter.enabled = true;
        //    Explode();
        //}
    }

    public void SetExplosion () {
        explodable = true;
        foreach (Transform stone in stones) {
            //stone.SetParent(null);
            //var rb = stone.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.None;
            //rb.velocity = rb.velocity.normalized;
            //stone.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, blastRadius);
        }
    }

    void Explode() {
        explodable = false;
        //räjähdysanimaatio
        foreach (Transform stone in stones) {
            if (stone.tag == "Cavestone") {
                if (stone.GetComponent<Renderer>() != null) {
                    //Instantiate(explosionEffect, stone.GetComponent<Renderer>().bounds.center, stone.rotation);
                    stoneEmitter.enabled = false;
                }
            }
            stone.gameObject.SetActive(false);
        }
    }

    //foreach (Collider nearbyObj in nearbyObjects) {
    //        Rigidbody rb = nearbyObj.GetComponent<Rigidbody>();
    //        if (rb != null) {
    //            nearbyObj.transform.SetParent(null);
    //            rb.constraints = RigidbodyConstraints.None;
    //            rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
    //            Instantiate(explosionEffect, nearbyObj.transform.position, nearbyObj.transform.rotation);
    //var expl = nearbyObj.GetComponent<Explodable>();
    //            if (expl != null) {
    //                expl.SetExplosion();
    //            }
    //            //millon tän voi tehdä? tai tän vois tehä sillee et niillä objekteilla on oma scripti mikä odottaa ja sitten setactivefalse
    //        }
    //    }

    //private void OnCollisionEnter(Collision collision) {
    //    var next = collision.gameObject.GetComponent<Explodable>();
    //    if (next != null) {
    //        next.SetExplosion();
    //    }
    //}
}
