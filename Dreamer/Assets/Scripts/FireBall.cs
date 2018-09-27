using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

    public Vector3 targetDir;
    public float fireballSpeed;
    public float lifeTime;
    public LayerMask mask;
    public ParticleSystem ps;
    ParticleSystem.EmissionModule emitter;

    void Start() {
        emitter = ps.emission;
        emitter.enabled = false;
    }

    private void Update() {
        if (lifeTime < 0) {
            gameObject.SetActive(false);
        }
        lifeTime -= Time.deltaTime;
        transform.position += targetDir.normalized * Time.deltaTime * fireballSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        //print(other.gameObject.name);
        if (((1 << other.gameObject.layer) & mask) != 0) {
            if (other.gameObject.tag == "Player") {
                var b = other.GetComponentInParent<CharacterSkills>();
                if (b.Shield()) {
                    //var a = gameObject.GetComponent<Shield>();
                    targetDir = b.transform.forward;
                    print("bounce");
                    emitter.enabled = true;
                } else {
                    GameManager.instance.ChangeToddlerHealth(-10);
                    gameObject.SetActive(false);
                }
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}
