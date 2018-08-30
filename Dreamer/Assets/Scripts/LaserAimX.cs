using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAimX : MonoBehaviour {

    public float lowerAxisClamp;
    public float upperAxisClamp;
    public float sensitivity;
    public float range;
    public string inputAxisName;
    float yRot = 0;
    public bool active;

    CharacterMover cm;

    void Start() {
        cm = FindObjectOfType<CharacterMover>();    
    }

    void Update() {
        if (Input.GetButtonDown("Action") && Vector3.Distance(transform.position, cm.transform.position) < range) {
            active = true;
        }
        if (active) {
            yRot = Input.GetAxis(inputAxisName) * sensitivity;
            yRot = Mathf.Clamp(yRot, lowerAxisClamp, upperAxisClamp);
            transform.rotation = Quaternion.AngleAxis(yRot, Vector3.up) * transform.rotation;
            //transform.Rotate(axis, yRot * sensitivity);
            if (Vector3.Distance(transform.position, cm.transform.position) > range) {
                active = false;
            }
        }
    }
}
