using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRotator : MonoBehaviour {

    public float lowerAxisClamp; 
    public float upperAxisClamp;
    public float vertSens = 5; // Input sensitivity on vertical axis
    float xRot = 0;

    void Update() {
        xRot += -Input.GetAxis("Mouse Y") * vertSens;
        xRot = Mathf.Clamp(xRot, lowerAxisClamp, upperAxisClamp);
        transform.localRotation = Quaternion.AngleAxis(xRot, Vector3.right);
    }
}
