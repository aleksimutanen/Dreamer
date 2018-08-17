using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour {

    public Transform target;
    public Vector3 jump;
    public float jumpForce;
    public float movingSpeed;
    public float turnSpeed;
    public float inputAcceleration;
    public float groundCheckDepth;
    public float groundCheckSize;
    public float gravity;
    public float maxFallSpeed;
    public LayerMask map;
    float vert;
    float horiz;
    Rigidbody rb;

    public bool onGround;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate() {

        var input = vert * transform.forward + horiz * transform.right;
        input = Vector3.ClampMagnitude(input, 1);

        var flatVelocity = input * movingSpeed; // speed
        var b = rb.velocity;
        b.x = flatVelocity.x; b.z = flatVelocity.z;

        if(horiz > .2f || vert > .2f||horiz < -.2f||vert < -.2f) {
            rb.rotation = Quaternion.RotateTowards(rb.rotation, target.rotation, turnSpeed * Time.deltaTime);
        } else {

        }
                
        //rb.AddForce(input * inputAcceleration * Time.deltaTime, ForceMode.Acceleration);
        //if(input.magnitude > 0.2f) {
        //    var newRot = Quaternion.LookRotation(input);
        //    rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRot, turnSpeed * Time.deltaTime));
        //    // clamp velocity...?
        //}

        var colliders = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize, map);
        onGround = colliders.Length > 0;
        if (!onGround) {
            b += gravity * Vector3.down * Time.deltaTime;
            b.y = Mathf.Max(b.y, - maxFallSpeed);
        } else if (rb.velocity.y < 0) {
            b.y = 0f;
        }
        rb.velocity = b;

        //RaycastHit hit;
        //Ray ray;
        //if (Physics.Raycast(rb.position, Vector3.down, out hit, 1f, map)) {
        //    Physics.gravity
        //}
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize);
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && onGround) {
            Jump();
        }
        vert = Input.GetAxis("Vertical");
        horiz = Input.GetAxis("Horizontal");
    }

    void Jump() {
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
    }
}
