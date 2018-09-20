using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Transform vertRot;
    public float camDist = 5;

    public float minDist;
    public float maxDist;
    float castSize = .2f;
    LayerMask layerMask;

    float normalCamDist;



    void Start() {
        normalCamDist = camDist;
        layerMask = 1 << LayerMask.NameToLayer("Map");
    }

    void FixedUpdate() {

        //Vector3 desiredCameraPos = vertRot.transform.position -vertRot.forward * maxDist;

        RaycastHit hit;

        if(Physics.SphereCast(vertRot.transform.position, castSize, -vertRot.forward, out hit, Mathf.Infinity, layerMask)) {
            camDist = Mathf.Clamp(hit.distance - 1, minDist, maxDist);

            //print(hit.collider);
        } else {
            camDist = maxDist;
        }

        Debug.DrawLine(vertRot.transform.position, hit.point, Color.green, 0.1f);


        if (GameManager.instance.lookEnabled) {
            if (!WorldSwitch.instance.transitionOut && !WorldSwitch.instance.transitionIn) {
                transform.rotation = vertRot.rotation;
                transform.position = vertRot.position + transform.forward * -camDist;
                //camDist = normalCamDist;
            } else if (WorldSwitch.instance.transitionOut) {
                if (WorldSwitch.instance.state == AwakeState.Dream) {
                    transform.rotation = vertRot.rotation;
                    transform.position = vertRot.position + transform.forward * -camDist;
                    camDist -= Time.deltaTime * 1.715f * 2.5f/*1.55f * 2.5f*/;
                } else if (WorldSwitch.instance.state == AwakeState.NightMare) {
                    transform.rotation = vertRot.rotation;
                    transform.position = vertRot.position + transform.forward * -camDist;
                    camDist -= Time.deltaTime * 1.595f * 2.5f/*1.45f * 2.5f*/;
                }
            } else if (WorldSwitch.instance.transitionIn) {
                transform.rotation = vertRot.rotation;
                transform.position = vertRot.position + transform.forward * -camDist;
                camDist += Time.deltaTime * /*1.55f*/1.705f * 2.5f * 2;
            }
        }


	}
}