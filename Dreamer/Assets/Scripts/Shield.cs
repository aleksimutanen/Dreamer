using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public ParticleSystem ps;
    ParticleSystem.EmissionModule emitter;

    void Start() {
        emitter = ps.emission;
        emitter.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ammo") {
            print("ammo hit shield");
            var b = other.GetComponent<FireBall>().targetDir = transform.forward;
            emitter.enabled = true;
        }
        //else {
        //emitter.enabled = false;
        //}
    }
}
