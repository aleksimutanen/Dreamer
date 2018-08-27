using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashCollider : MonoBehaviour {

    public LayerMask breakableWall;

    void OnCollisionEnter(Collision collision) {
        print("hit");
        if (collision.gameObject.layer == breakableWall) {
            var b = collision.gameObject.GetComponent<Rigidbody>();
            b.isKinematic = false;
            print("hit");
        }
    }
}
