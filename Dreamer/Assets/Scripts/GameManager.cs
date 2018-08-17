using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public int crystalAmount = 0;
    public TextMeshProUGUI crystalText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddCrystal() {
        crystalAmount += 1;
        crystalText.text = "crystals: " + crystalAmount + "kpl"; 
    }
}
