using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    public CharacterMover target;//the target object
    private Vector3 point;//the coord to the point where the camera looks at

    void Start() {//Set up things on the start method
        point = target.transform.position;//get target's coords
        transform.LookAt(point);//makes the camera look to it
    }

    void Update() {//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier

        var vert = Input.GetAxis("Mouse X");
        var horiz = Input.GetAxis("Mouse Y");
        var input = vert * Vector3.up + horiz * Vector3.right;
        input.Normalize();

        transform.RotateAround(point, input, 20 * Time.deltaTime);
    }
}

