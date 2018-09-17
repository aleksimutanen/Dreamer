using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour {

    public bool explodable;
    public GameObject explosionEffect;
    public float countdown = 0.5f;
    Transform[] stones;

    private void Start() {
        stones = GetComponentsInChildren<Transform>();
    }

    void Update () {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && explodable) {
            Explode();
        }
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
            Instantiate(explosionEffect, stone.GetComponent<Renderer>().bounds.center, stone.rotation);
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
