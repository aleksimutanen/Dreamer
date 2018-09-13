using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public Transform vertRot;
    public float camDist = 5;
    float normalCamDist;

    void Start() {
        normalCamDist = camDist;    
    }
    void FixedUpdate() {
        if (GameManager.instance.lookEnabled) {
            if (!WorldSwitch.instance.transitionOut && !WorldSwitch.instance.transitionIn) {
                transform.rotation = vertRot.rotation;
                transform.position = vertRot.position + transform.forward * -camDist;
                camDist = normalCamDist;
            } else if (WorldSwitch.instance.transitionOut) {
                //if (WorldSwitch.instance.state == AwakeState.Dream) {
                //    print("faster transition");
                //    transform.rotation = vertRot.rotation;
                //    transform.position = vertRot.position + transform.forward * -camDist;
                //    camDist += Time.deltaTime * 1.60f * 1.5f;
                //} else if (WorldSwitch.instance.state == AwakeState.NightMare) {
                //    transform.rotation = vertRot.rotation;
                //    transform.position = vertRot.position + transform.forward * -camDist;
                //    camDist += Time.deltaTime * 1.60f;
                //}
                transform.rotation = vertRot.rotation;
                transform.position = vertRot.position + transform.forward * -camDist;
                camDist -= Time.deltaTime * 1.55f;
            } else if (WorldSwitch.instance.transitionIn) {
                //if (WorldSwitch.instance.state == AwakeState.NightMare) {
                //    print("faster transition");
                //    transform.rotation = vertRot.rotation;
                //    transform.position = vertRot.position + transform.forward * -camDist;
                //    camDist += Time.deltaTime * 1.60f * 2 * 1.5f;
                //} else if (WorldSwitch.instance.state == AwakeState.Dream) {
                transform.rotation = vertRot.rotation;
                transform.position = vertRot.position + transform.forward * -camDist;
                camDist += Time.deltaTime * 1.55f * 2;
            }
        }
<<<<<<< HEAD

	}
}
=======
    }
}
>>>>>>> c000d422f7a8d614b34a77a0e306a02fea709430
