using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OuterTrigger : MonoBehaviour {
    public GameObject mountain;
    public GameObject cave;

    public ParticleSystem caveParticles;
    ParticleSystem.EmissionModule caveEmitter;

    private void Start() {
        caveEmitter = caveParticles.emission;
        caveEmitter.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            caveEmitter.enabled = false;
            GameManager.instance.SetCheckpoint();
            mountain.gameObject.SetActive(true);
            cave.gameObject.SetActive(false);
            GameManager.instance.switchEnabled = true;
            EarCompass.instance.FindCrystals();
        }

    }
}
