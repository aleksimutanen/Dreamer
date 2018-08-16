using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRotator : MonoBehaviour {

    public Transform target;
    float yRot;
    public float horSens = 5;

	void Update () {
        transform.position = target.position;
        yRot = Input.GetAxis("Mouse X") * horSens;
        transform.rotation = Quaternion.AngleAxis(yRot, Vector3.up) * transform.rotation;
	}
}
