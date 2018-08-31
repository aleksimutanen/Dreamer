using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BatMode {Hanging, Flying, Attacking};
//tarviiko tän olla enum?

public class Bat : MonoBehaviour {

    //miten ja millon lepakot pitää luoda? aktivoida?
    //pitääkö niiden olla jossain samassa folderissa? miks? miksei?

    Rigidbody rb;
    public float speed;
    public BatMode batm;
    public float attackRadius = 10;
    RaycastHit hit2;
    public float sphereRadius = 2;
    public float maxDist = 3;
    public Transform target;
    public float steeringSpeed = 80f;
    public float distToPlayer;
    public LayerMask obstacles;
    public bool vaistaa;
    public float angle;

    public LayerMask floor;
    public float maxDistToFloor = 2;
    public float steerSpeedFloor = 200f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        angle = (float)Random.Range(0, 8) * 45f;
    }

    void FixedUpdate () {

        // switch states?

        distToPlayer = Vector3.Distance(transform.position, target.position);

        if (distToPlayer < attackRadius) {
            batm = BatMode.Attacking;
        }

        else {
            batm = BatMode.Flying;
        }

        if (batm == BatMode.Attacking) {
            Attack();
            
        }

        // TODO: other modes

        else if (batm == BatMode.Flying) {
            if (RayCone(floor, maxDistToFloor, steerSpeedFloor)) {
                vaistaa = true;
                rb.velocity = Vector3.zero;
            }

            else if (RayCone(obstacles, maxDist, steeringSpeed)) {
                vaistaa = true;
                rb.velocity = transform.forward * speed;
            }

            else {
                var targetPoint = transform.position + transform.right * 0.2f;
                var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
                targetRotation = Quaternion.Euler(-targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, -targetRotation.eulerAngles.z);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, Time.deltaTime * steeringSpeed);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
                vaistaa = false;
                rb.velocity = transform.forward * speed;
            }


        }

    }

    void Attack() {

        RaycastHit hit;

        Physics.Raycast(transform.position, transform.forward, out hit, maxDist, obstacles);
        //Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxDist /*Mathf.Infinity*/, obstacles);
        hit2 = hit;

        //testataan onko törmäämässä maahan
        if (RayCone(floor, maxDistToFloor, steerSpeedFloor)) {
            vaistaa = true;
            rb.velocity = transform.forward * -speed;
        }

        else if (hit.collider != null && hit.distance < maxDist) {
            vaistaa = true;
            RayCone(obstacles, maxDist, steeringSpeed);
        }

        else if (RayCone(obstacles, maxDist, steeringSpeed)) {
            vaistaa = true;
        }

        else {
            vaistaa = false;
            // steer towards target
            var dir = target.position - transform.position;
            var targetRot = Quaternion.LookRotation(dir, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRot, Time.deltaTime * steeringSpeed);
        }

        rb.velocity = transform.forward * speed;

    }

    //maan ja muiden esteiden ero = eri layereilla, maahan lyhemmät rayt, jos maa pitää hidastaa mut kääntyä pois maasta nopeemmin
    bool RayCone(LayerMask lm, float md, float strSpeed) {
        RaycastHit info;
        int kerroin;
        if (lm == floor) {
            kerroin = 2;
        }
        else {
            kerroin = 4;
        }

        for (int i = 0; i < 8; i++) {

            Vector3 d = (kerroin*transform.forward + transform.up).normalized;
            //käännetään d-vektoria z-akselin ympäri
            angle = i * 45f;
            var qq = Quaternion.AngleAxis(angle, transform.forward);
            d = qq * d;
            Ray r = new Ray(transform.position, d);
            Physics.Raycast(r, out info, md, lm);
            
            if (info.collider != null) {
                Debug.DrawLine(transform.position, transform.position + d*md, Color.red);
                //ja sitten i+4 on se mihin suuntaan käännytään
                if (angle > 3) {
                    angle = (i - 3) * 45f;
                }
                else {
                    angle = (i + 3) * 45f;
                }
                qq = Quaternion.AngleAxis(angle, transform.forward);
                d = qq * (transform.forward + transform.up).normalized;
                var q = Quaternion.LookRotation(d, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, q, Time.deltaTime * strSpeed);
                //rb.rotation = q * rb.rotation;

                return true;
            }
            else {
                Debug.DrawLine(transform.position, transform.position + d*md, Color.blue);
            }
        }
        return false;
    }

    public void Explode() {
        //kun pelaaja lyö, räjähdä
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == 10) {
            GameManager.instance.ChangeToddlerHealth(-1);
        }

        //TODO: lepakon osuminen kilpeen
    }

    private void OnDrawGizmosSelected() {
        //Gizmos.color = Color.red;
        //Debug.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
    }
}
//            Vector3 d2 = ((transform.position + Vector3.forward) + (transform.position + Vector3.right)) - transform.position;
//Vector3 d3 = 