﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalNag : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player")
            GameManager.instance.ChangeStatusText(15, 5);
    }
}
