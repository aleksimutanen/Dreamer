﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Transform vertRot;
    public float camDist = 5;

	void FixedUpdate () {
        if(GameManager.instance.lookEnabled){
            transform.rotation = vertRot.rotation;
            transform.position = vertRot.position + transform.forward * -camDist;
        }

	}
}
