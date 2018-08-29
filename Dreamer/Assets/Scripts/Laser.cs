using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public Transform beamStart;
    public Transform beam;
    LaserTrigger lt;
    public LayerMask trigger;

    public bool hit;

    void Start() {
        lt = FindObjectOfType<LaserTrigger>();
    }

    void Update() {
        RaycastHit hitInfo;
        if (Physics.Raycast(beamStart.position, beamStart.forward, out hitInfo)) {
            beam.gameObject.SetActive(true);
            beam.position = (beamStart.position + hitInfo.point) / 2;
            beam.localScale = new Vector3(1, 1, hitInfo.distance / 2);
            if (Physics.Raycast(beamStart.position, beamStart.forward, out hitInfo, Mathf.Infinity, trigger)) {
                lt.AddLaserToList(this);
            } else {
                lt.RemoveLaserFromList(this);
            }
        } else {
            beam.gameObject.SetActive(false);
        }
    }
}


