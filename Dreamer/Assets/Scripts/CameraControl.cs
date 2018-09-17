using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Transform vertRot;
    public float camDist = 5;

    public float minDist;
    public float maxDist;
    public float distance;


    float normalCamDist;



    void Start() {
        normalCamDist = camDist;    
    }

    void FixedUpdate() {

        Vector3 desiredCameraPos = vertRot.transform.position -vertRot.forward * maxDist;

        RaycastHit hit;

        if(Physics.Raycast(vertRot.transform.position, -vertRot.forward, out hit)) {
            camDist = Mathf.Clamp(hit.distance, minDist, maxDist);
        } else {
            camDist = maxDist;
        }

        Debug.DrawLine(vertRot.transform.position, -vertRot.forward * maxDist, Color.yellow, 0.1f);


        if (GameManager.instance.lookEnabled) {
            if (!WorldSwitch.instance.transitionOut && !WorldSwitch.instance.transitionIn) {
                transform.rotation = vertRot.rotation;
                transform.position = vertRot.position + transform.forward * -camDist;
                //camDist = normalCamDist;
            } else if (WorldSwitch.instance.transitionOut) {
                if (WorldSwitch.instance.state == AwakeState.Dream) {
                    transform.rotation = vertRot.rotation;
                    transform.position = vertRot.position + transform.forward * -camDist;
                    camDist -= Time.deltaTime * 1.55f;
                } else if (WorldSwitch.instance.state == AwakeState.NightMare) {
                    transform.rotation = vertRot.rotation;
                    transform.position = vertRot.position + transform.forward * -camDist;
                    camDist -= Time.deltaTime * 1.45f;
                }
            } else if (WorldSwitch.instance.transitionIn) {
                transform.rotation = vertRot.rotation;
                transform.position = vertRot.position + transform.forward * -camDist;
                camDist += Time.deltaTime * 1.55f * 2;
            }
        }


	}
}