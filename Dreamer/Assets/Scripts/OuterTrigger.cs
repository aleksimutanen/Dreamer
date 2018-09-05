using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterTrigger : MonoBehaviour {
    public GameObject wall;
    public GameObject tunnel;
    private void OnTriggerEnter(Collider other)
    {
        wall.gameObject.SetActive(true);
        tunnel.gameObject.SetActive(false);
    }
}
