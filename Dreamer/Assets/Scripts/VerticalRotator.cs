using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRotator : MonoBehaviour {

    public Transform target;
    Quaternion maxRot;
    public float vertSens = 5;
    float xRot = 0;

    void Update() {
        xRot += -Input.GetAxis("Mouse Y") * vertSens;
        xRot = Mathf.Clamp(xRot, 5f, 60f);
        //transform.localRotation = Quaternion.AngleAxis(Mathf.Clamp(xRot, 10, 45), Vector3.right) * transform.localRotation;
        transform.localRotation = Quaternion.AngleAxis(xRot, Vector3.right);
    }
}
