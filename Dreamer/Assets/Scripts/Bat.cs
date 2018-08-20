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
    RaycastHit hit2;
    public float sphereRadius = 2;
    public float maxDist = 2;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (batm == BatMode.Flying) {
            Fly();
        }
	}

    void Fly() {
        RaycastHit hit;

        Physics.SphereCast(transform.position, sphereRadius, transform.forward, out hit);
        hit2 = hit;

        //print(hit.distance);
        if (hit.collider != null && hit.distance < maxDist) {
            print("väistää");
            rb.velocity = new Vector3(0, 0, 0);
            var targetPoint = transform.position + transform.right;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        }
        else {
            print("kääntyy");
            rb.velocity = transform.forward * speed;
            var targetPoint = transform.position + transform.right * 0.2f;
            var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
        }
        //entiiä
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + transform.forward * hit2.distance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * hit2.distance, sphereRadius);
    }
}
