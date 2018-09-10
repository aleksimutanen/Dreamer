using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
    public GameObject fireBall;
    public float firingInterval = 1;

    private void Update() {
        firingInterval -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other){
        print("Tähtäimessä");
        if(fireBall.gameObject.activeSelf == false && firingInterval < 0){
            if (other.gameObject.tag == "Player"){
                fireBall.GetComponent<FireBall>().lifeTime = 2;
                fireBall.SetActive(true);
                fireBall.transform.position = gameObject.transform.position;
                fireBall.GetComponent<FireBall>().targetDir = other.transform.position - fireBall.transform.position;
                firingInterval = 1;
            }
        }
    }
}