using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour {

    public Transform dreamCollider;
    public Transform nightmareCollider;

    public Transform horizontalRotator;
    public Vector3 jump;
    public Vector3 bash;
    public float jumpForce;
    public float bashForce;
    public float movingSpeed;
    public float turnSpeed;
    public float inputAcceleration;
    public float groundCheckDepth;
    public float groundCheckSize;

    public float groundCheckDepth2;
    public float groundCheckSize2;

    public float gravity;
    public float normalGravity;
    public float maxFallSpeed;
    
    bool hasToJump;
    Rigidbody rb;

    public bool onGround;
    public bool canJump;

    void Start() {
        rb = GetComponent<Rigidbody>();
        normalGravity = gravity;
        nightmareCollider.gameObject.SetActive(false);
        rb.MovePosition(GameManager.instance.gameStartPoint.position);
    }

    private void FixedUpdate() {
        if (GameManager.instance.walkEnabled)
        {
            // Input reading for movement
            var vert = Input.GetAxis("Vertical");
            var horiz = Input.GetAxis("Horizontal");

            var input = vert * transform.forward + horiz * transform.right;
            input = Vector3.ClampMagnitude(input, 1);

            // Speed
            var flatVelocity = input * movingSpeed;
            var b = rb.velocity;
            b.x = flatVelocity.x; b.z = flatVelocity.z;



            // If there is movement input, start to rotate camera towards players forward direction
            if (horiz > .2f || vert > .2f || horiz < -.2f || vert < -.2f)
            {
                rb.rotation = Quaternion.RotateTowards(rb.rotation, horizontalRotator.rotation, turnSpeed * Time.deltaTime);
            }


            // Ground check and gravity
            //var furtherSphere = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth2, groundCheckSize2, WorldSwitch.instance.map);
            var closerSphere = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize, WorldSwitch.instance.map);
            //canJump = furtherSphere.Length > 0;
            onGround = closerSphere.Length > 0;
            if (/*(*/!onGround /*&& canJump) || (!onGround && !canJump)*/) {
                b += gravity * Vector3.down * Time.deltaTime;
                b.y = Mathf.Max(b.y, -maxFallSpeed);
            } else if (rb.velocity.y < 0) {
                b.y = 0f;
            }
            rb.velocity = b;
        }

        //RaycastHit hit;
        //Ray ray;
        //if (Physics.Raycast(rb.position, Vector3.down, out hit, 1f, map)) {
        //    Physics.gravity
        //}

        if (hasToJump) {
            Jump();
            hasToJump = false;
        }
    }

    // Debug sphere
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize);
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth2, groundCheckSize2);
    }

    void Update() {
        // Input reading for jump
        if(GameManager.instance.jumpEnabled){
            if (Input.GetButtonDown("Jump") && /*canJump*/ onGround) {
                hasToJump = true;
            }
        }
    }

    // Player jump movement of rigidbody
    void Jump() {
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        //Fabric.EventManager.Instance.PostEvent("Jump");
    }

    public void Bash() {
        //rb.AddForce(transform.forward * bashForce * Time.deltaTime,/*rb.position,*/ ForceMode.Impulse);
        rb.AddForce(transform.forward * bashForce, ForceMode.Impulse);
        //rb.position += transform.forward * 5;
        //Vector3.MoveTowards(rb.position, bash, 50 * Time.deltaTime);
    }

    public void EnterDream() {
        dreamCollider.gameObject.SetActive(true);
        nightmareCollider.gameObject.SetActive(false);
    }

    public void EnterNightmare() {
        nightmareCollider.gameObject.SetActive(true);
        dreamCollider.gameObject.SetActive(false);
    }
}
