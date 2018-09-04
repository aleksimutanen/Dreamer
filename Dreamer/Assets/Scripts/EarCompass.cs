using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarCompass : MonoBehaviour {
    public GameObject[] crystals;
    public GameObject nearestCrystal = null;
    public float distToCrystal;
    public Transform buddy;
    float rotSpeed = 100;

    private void Awake() {
        crystals = GameObject.FindGameObjectsWithTag("Crystal");
        print(crystals);
    }

    void Update () {

        float shortestDist = float.MaxValue;
        GameObject crystalCandidate = null;
        foreach (GameObject crystal in crystals) {
            
                
            var dist = Vector3.Distance(buddy.transform.position, crystal.transform.position);

            if (dist < shortestDist && crystal.gameObject.activeSelf == true) {
                crystalCandidate = crystal;
                shortestDist = dist;
                distToCrystal = Mathf.Max(1f, shortestDist);
            }
            
        }
        nearestCrystal = crystalCandidate;
        transform.position = buddy.position;
        if (nearestCrystal != null)
        {
            Vector3 fwd = nearestCrystal.transform.position - transform.position;
            Debug.DrawLine(transform.position, transform.position + fwd * 3);
            var right = Vector3.Cross(Vector3.up, fwd);
            Debug.DrawLine(transform.position, transform.position + right * 3);
            var up = Vector3.Cross(fwd, right);
            Debug.DrawLine(transform.position, transform.position + up * 3);



            Quaternion rotationGoal = Quaternion.LookRotation(fwd, up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationGoal, rotSpeed * Time.deltaTime);
        }
    }
}
