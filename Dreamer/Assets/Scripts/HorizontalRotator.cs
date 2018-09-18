using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRotator : MonoBehaviour {

    // Rotator script for camera control

    public Transform player;
    float yRot;
    public float horSens = 5; // Input sensitivity on the horzontal axis

	void Update () {
        transform.position = new Vector3 (player.position.x, player.position.y + 2, player.position.z); // Move this rotator object to player position
        yRot = Input.GetAxis("CameraX") * horSens;
        transform.rotation = Quaternion.AngleAxis(yRot, Vector3.up) * transform.rotation; // Change this objects rotation same as players
	}
    public void ResetRotation(){
        transform.rotation = player.transform.rotation;
    }
}
