using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour {

    public Transform dreamCollider;
    public Transform nightmareCollider;

    public Transform horizontalRotator;
    public Animator anim;
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

    public float fallingDeathThreshold;
    public float gravity;
    public float normalGravity;
    public float maxFallSpeed;
    
    bool hasToJump;
    Rigidbody rb;

    public bool onGround;
    public bool canJump;

    Vector3 fallPoint;
    CharacterSkills cs;

    public Vector3[] directions;
    public float maxDistance;
    public bool rcHit;

    void Start() {
        cs = FindObjectOfType<CharacterSkills>();
        rb = GetComponent<Rigidbody>();
        normalGravity = gravity;
        nightmareCollider.gameObject.SetActive(false);
        rb.MovePosition(GameManager.instance.gameStartPoint.position);
    }

    private void LateUpdate() {
        var b = rb.velocity;

        // Input reading for movement
        var vert = Input.GetAxis("Vertical");
        var horiz = Input.GetAxis("Horizontal");

        var input = vert * transform.forward + horiz * transform.right;
        input = Vector3.ClampMagnitude(input, 1);

        if (GameManager.instance.walkEnabled) {

            // Speed
            var flatVelocity = input * movingSpeed;
            b.x = flatVelocity.x; b.z = flatVelocity.z;

            // If there is movement input, start to rotate camera towards players forward direction
            if (horiz > .2f || vert > .2f || horiz < -.2f || vert < -.2f) {
                GameManager.instance.toddlerMoving = true;
                anim.Play("Walk");
                rb.rotation = Quaternion.RotateTowards(rb.rotation, horizontalRotator.rotation, turnSpeed * Time.deltaTime);
            } else {
                GameManager.instance.toddlerMoving = false;
            }

            // Ground check and gravity
            var furtherSphere = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth2, groundCheckSize2, WorldSwitch.instance.map);
            var closerSphere = Physics.OverlapSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize, WorldSwitch.instance.map);
            canJump = furtherSphere.Length > 0;
            onGround = closerSphere.Length > 0;
            if ((!onGround && canJump) || (!onGround && !canJump) /*canJump*/) {
                b += gravity * Vector3.down * Time.deltaTime;
                b.y = Mathf.Max(b.y, -maxFallSpeed);
                if ((fallPoint.y - rb.position.y) > fallingDeathThreshold) {
                    fallPoint = GameManager.instance.checkpoint;
                    GameManager.instance.ALiveLost();
                    print("die");
                }
            } else if (rb.velocity.y < 0) {
                b.y = 0f;
                cs.glideTimer = cs.maxGlideTimer;
                fallPoint = rb.position;
            }
        }

        if (hasToJump) {
            Jump();
            hasToJump = false;
        }

        RaycastHit hit;
        for (int i = 0; i < directions.Length; i++) {
            Vector3 worldDir = transform.rotation * directions[i].normalized;
            Debug.DrawLine(rb.position + new Vector3(0, 2, 0), rb.position + new Vector3(0, 2, 0) + worldDir * maxDistance);
            if (Physics.Raycast(rb.position + new Vector3(0, 2, 0), worldDir, out hit, maxDistance, WorldSwitch.instance.map)) {
                print(directions[i]);
                if (Vector3.Angle(b, worldDir) < 90) {
                    print("something");
                    Vector3 proj = Vector3.Project(b, worldDir);
                    b -= proj;
                    input += -proj;
                }
            }
        }
        rb.velocity = b;
    }

    // Debug sphere
    private void OnDrawGizmosSelected() {
        //for (int i = 0; i < directions.Length; i++) {
        //    Gizmos.DrawLine(transform.position, transform.position + directions[i]);
        //}
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth, groundCheckSize);
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDepth2, groundCheckSize2);
    }

    void Update() {
        // Input reading for jump
        if (GameManager.instance.jumpEnabled && WorldSwitch.instance.state == AwakeState.Dream) {
            if (Input.GetButtonDown("Jump") && canJump /*onGround*/) {
                hasToJump = true;
            }
        }
    }

    // Player jump movement of rigidbody
    void Jump() {
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        Fabric.EventManager.Instance.PostEvent("Jump");
    }

    public void Bash() {
        //rb.AddForce(transform.forward * bashForce, ForceMode.Impulse);
        rb.position += transform.forward * bashForce * Time.deltaTime;
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
