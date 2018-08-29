using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAimY : MonoBehaviour {

    public string inputAxisName;
    public float lowerAxisClamp;
    public float upperAxisClamp;
    public float vertSens = 5; // Input sensitivity on vertical axis
    float xRot = 0;
    public float range;
    public bool active;

    CharacterMover cm;

    private void Start() {
        cm = FindObjectOfType<CharacterMover>();
    }

    void Update() {
        if (Input.GetButtonDown("Action") && Vector3.Distance(transform.position, cm.transform.position) < range) {
            active = true;
        }
        if (active) {
            xRot += Input.GetAxis(inputAxisName) * vertSens;
            xRot = Mathf.Clamp(xRot, lowerAxisClamp, upperAxisClamp);
            transform.localRotation = Quaternion.AngleAxis(xRot, Vector3.right);
            if (Vector3.Distance(transform.position, cm.transform.position) > range) {
                active = false;
            }
        }
    }
}

