using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BatMode {Hanging, Flying, Attacking};
//tarviiko tän olla enum?

public class Bat : MonoBehaviour {

    //miten ja millon lepakot pitää luoda? aktivoida?
    //pitääkö niiden olla jossain samassa folderissa? miks? miksei?

    public BatMode batm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (batm == BatMode.Flying) {
            Fly();
        }
	}

    void Fly() {
        //entiiä
    }
}
