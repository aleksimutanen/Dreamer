using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BatMode {Hanging, Flying, Attacking, Returning};
//tarviiko tän olla enum?

public class Bat : MonoBehaviour, Enemy {

    //miten ja millon lepakot pitää luoda? aktivoida?
    //pitääkö niiden olla jossain samassa folderissa? miks? miksei?

    Rigidbody rb;
    Vector3 startPos;
    public float speed;
    public BatMode batm;
    public float attackRadius = 10;
    RaycastHit hit2;
    public float maxDist = 3;

    public float steeringSpeed = 80f;

    public Transform playerTransform;
    Vector3 target;
    public float distToPlayer;
    public LayerMask obstacles;
    public bool vaistaa;
    public float angle;

    public LayerMask floor;
    public float maxDistToFloor = 2;
    public float steerSpeedFloor = 200f;

    public float explosionDelay = 1f;
    float countdown;
    public GameObject explosionEffect;
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public float health = 2f;
    public bool hasExploded;

    public float dmgToPlayer = -5;
    public float pwrToShield = 5;

    public bool sleeping;
    //muuta gamemanagerissa
    
    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        countdown = explosionDelay;
        rb = GetComponent<Rigidbody>();
        angle = (float)Random.Range(0, 8) * 45f;
        startPos = transform.position;
        target = playerTransform.position;
    }

    void Update() {
        if(health <= 0)
            countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded) {
            Explode();
        }
    }

    void FixedUpdate() {

        // switch states?
        if (WorldSwitch.instance.state == AwakeState.NightMare && !sleeping) {
            distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distToPlayer < attackRadius) {
                batm = BatMode.Attacking;
            }
        } else {
            batm = BatMode.Hanging;
        }

        if (batm != BatMode.Hanging) {
            if (batm == BatMode.Attacking) {
                target = playerTransform.position;
            } else if (batm == BatMode.Returning) {
                target = startPos;
            }
            Fly();
        }
        
    }

    //void Fly() {
    //    if (RayCone(floor, maxDistToFloor, steerSpeedFloor)) {
    //        vaistaa = true;
    //        rb.velocity = Vector3.zero;
    //    }

    //    else if (RayCone(obstacles, maxDist, steeringSpeed)) {
    //        vaistaa = true;
    //        rb.velocity = transform.forward * speed;
    //    }
    //}

    void Fly() {

        if (batm == BatMode.Attacking && distToPlayer > attackRadius) {
            batm = BatMode.Returning;
        }

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

        else if (batm == BatMode.Flying) {
            var targetPoint = transform.position + transform.right * 0.2f;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            targetRotation = Quaternion.Euler(-targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, -targetRotation.eulerAngles.z);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, Time.deltaTime * steeringSpeed);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
            vaistaa = false;
            rb.velocity = transform.forward * speed;
        }

        else {
            vaistaa = false;
            // steer towards target
            var dir = target - transform.position;
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

            Vector3 d = (kerroin * transform.forward + transform.up).normalized;
            //käännetään d-vektoria z-akselin ympäri
            angle = i * 45f;
            var qq = Quaternion.AngleAxis(angle, transform.forward);
            d = qq * d;
            Ray r = new Ray(transform.position, d);
            Physics.Raycast(r, out info, md, lm);

            if (info.collider != null) {
                Debug.DrawLine(transform.position, transform.position + d * md, Color.red);
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
                Debug.DrawLine(transform.position, transform.position + d * md, Color.blue);
            }
        }
        return false;
    }

    public void Explode() {
        //kun pelaaja lyö, räjähdä
        hasExploded = true;
        //räjähdysanimaatio
        Instantiate(explosionEffect, transform.position, transform.rotation);
        //kerrotaan lähellä oleville objekteille että niidenkin pitää räjähtää

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObj in nearbyObjects) {
            Rigidbody rb = nearbyObj.GetComponent<Rigidbody>();
            if (rb != null) {
                //rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
                Instantiate(explosionEffect, nearbyObj.transform.position, nearbyObj.transform.rotation);
                var expl = nearbyObj.GetComponent<Explodable>();
                if (expl != null) {
                    expl.SetExplosion();
                }
                //millon tän voi tehdä? tai tän vois tehä sillee et niillä objekteilla on oma scripti mikä odottaa ja sitten setactivefalse
            }
        }
        //jos pelaaja lähellä, damagea pelaajaan / kilven latausta jos kilpi ylhäällä?

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision) {

        if (collision.gameObject.name == "Shield") {
            print("blocked");
            GameManager.instance.ChangeBuddyPower(pwrToShield);
            //blocked --> return

        } else if (collision.gameObject.name == "Ammo(Clone)") {
            collision.GetComponent<EnergyAmmo>().DealDamage(this);
            print("ammo hit");
        } else if (collision.gameObject.layer == 10 || collision.gameObject.layer == 12) {
            print("bat hit player");
            GameManager.instance.ChangeToddlerHealth(dmgToPlayer);
        }
        batm = BatMode.Returning;
    }

    public void KickBack(Vector3 dir, float force) {
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
    //kun kilvellä ammutaan

    private void OnDrawGizmosSelected() {
        //Gizmos.color = Color.red;
        //Debug.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
    }

    //            Vector3 d2 = ((transform.position + Vector3.forward) + (transform.position + Vector3.right)) - transform.position;
    //Vector3 d3 = 

    public void TakeDamage(float damage) {
        if (health <= 0) return;
        health -= damage;
    }

    public void Respawn() {
        //jotain?
    }
}