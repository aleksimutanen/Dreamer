using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour {

    public Vector3 jump;
    public float jumpForce;
    public float movingSpeed;
    public float turnSpeed;
    public float inputAcceleration;
    public float groundCheckDepth;
    public float groundCheckSize;
    public float maxFallSpeed;
    public LayerMask map;

    Rigidbody rb;

    public bool onGround;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        var vert = Input.GetAxis("Vertical");
        var horiz = Input.GetAxis("Horizontal");
        var input = vert * Vector3.forward + horiz * Vector3.right;
        input.Normalize();
        var flatVelocity = input * movingSpeed; // speed
        var b = rb.velocity;
        b.x = flatVelocity.x; b.z = flatVelocity.z;

        //rb.AddForce(input * inputAcceleration * Time.deltaTime, ForceMode.Acceleration);

        if (input.magnitude > 0.2f) {
            var newRot = Quaternion.LookRotation(input);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, newRot, turnSpeed * Time.deltaTime));
            // clamp velocity...?
        }

        var colliders = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize, map);
        onGround = colliders.Length > 0;
        if (!onGround) {
            b += 9.81f * Vector3.down * Time.deltaTime;
            b.y = Mathf.Max(b.y, maxFallSpeed);
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
    }

    void Jump() {
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
    }
}
