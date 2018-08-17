using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    // Singleton related
    public static GameManager instance;

    public int crystalAmount = 0;
    public TextMeshProUGUI crystalText;

    private void Awake() {
        // Make a singleton instance out of (this) GameManager
        if(instance)
            Debug.LogError("2+ GameManagers found!");
        instance = this;
    }

    void Start () {
		
	}
	
	void Update () {
		
	}

    // When crystal is collected
    public void AddCrystal() {
        crystalAmount += 1;
        crystalText.text = "crystals: " + crystalAmount + "kpl"; 
    }
}
