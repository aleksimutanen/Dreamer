using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerTrigger : MonoBehaviour {

    public GameObject wall;
    public GameObject tunnel;
    private void OnTriggerEnter(Collider other)
    {
        wall.gameObject.SetActive(false);
        tunnel.gameObject.SetActive(true);
    }
}
