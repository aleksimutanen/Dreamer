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
    public float maxDist = 2;
    public Transform target;
    public float steeringSpeed = 20f;
    float distToPlayer;
    public LayerMask obstacles;
    public bool vaistaa;
    public float angle;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        angle = (float)Random.Range(0, 8) * 45f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        //for (int i = 0; i < 8; i++) {
        //    RaycastHit info;
        //    print(transform.position + "+ forward " + (transform.position + Vector3.forward));
        //    Debug.DrawLine(transform.position, (transform.position + Vector3.forward));

        //    print("+ up" + (transform.position + Vector3.up));
        //    Debug.DrawLine(transform.position, (transform.position + Vector3.up));

        //    Vector3 d = (Vector3.forward + Vector3.up).normalized;
        //    //käännetään d-vektoria z-akselin ympäri
        //    var angle = i * 45f;
        //    print("angle " + angle);
        //    var qq = Quaternion.AngleAxis(angle, transform.forward);
        //    d = qq * d;
        //    Ray r = new Ray(transform.position, d);
        //    Physics.Raycast(r, out info, maxDist, obstacles);
        //    print(transform.position + " " + " " + d);
        //    Debug.DrawLine(transform.position, transform.position + d);
        //}
        // switch states?
        distToPlayer = Vector3.Distance(transform.position, target.position);

        if (distToPlayer < attackRadius) {
            batm = BatMode.Attacking;
        }


        if (batm == BatMode.Attacking)
            Attack();
        // TODO: other modes

        
        //else if (batm == BatMode.Flying) {
        //    print("kääntyy");
        //    rb.velocity = transform.forward * speed;
        //    var targetPoint = transform.position + transform.right * 0.2f;
        //    var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
        //}

        //sit ne raycastit

	}

    void Attack() {

        RaycastHit hit;

        Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit, maxDist /*Mathf.Infinity*/, obstacles);
        hit2 = hit;

        if (hit.collider != null && hit.distance < maxDist) {
            vaistaa = true;
            RayCone();
        }

        else if (RayCone() != Quaternion.identity) {
            print(RayCone());
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

    //void Update() {
    //    transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    //    transform.position = startingPosition + Vector3.up * Mathf.Sin(Time.time * 2) * floatHeight;
    //}

    Quaternion RayCone() {
        RaycastHit info;

        for (int i = 0; i < 8; i++) {

            //Debug.DrawLine(transform.position, (transform.position + transform.forward));

            //Debug.DrawLine(transform.position, (transform.position + transform.up));

            Vector3 d = (transform.forward + transform.up).normalized;
            //käännetään d-vektoria z-akselin ympäri
            angle = i * 45f;
            var qq = Quaternion.AngleAxis(angle, transform.forward);
            d = qq * d;
            Ray r = new Ray(transform.position, d);
            Physics.Raycast(r, out info, maxDist, obstacles);

            if (info.collider != null) {
                Debug.DrawLine(transform.position, transform.position + d);
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
                rb.rotation = Quaternion.RotateTowards(rb.rotation, q, Time.deltaTime * steeringSpeed);
                //rb.rotation = q * rb.rotation;

                return q;
            }
        }
        return Quaternion.identity;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 10) {
            GameManager.instance.ChangeToddlerHealth(-1);
        }
        //TODO: lepakon osuminen kilpeen
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * hit2.distance, sphereRadius);
    }
}
//            Vector3 d2 = ((transform.position + Vector3.forward) + (transform.position + Vector3.right)) - transform.position;
//Vector3 d3 = 