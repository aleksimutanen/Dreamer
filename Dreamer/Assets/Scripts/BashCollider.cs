using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashCollider : MonoBehaviour {

    public GameObject bash;
    bool hit;
    float activeTime = 5f;
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (hit) {
            activeTime -= Time.deltaTime;
            if (activeTime < 0) {
                gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject == bash) {
            rb.isKinematic = false;
            hit = true;
        }
    }

    //void OnTriggerEnter(Collider other) {
    //    if (other.gameObject.name == "BashCollider") {
    //        rb.isKinematic = false;
    //    }    
    //}
}
