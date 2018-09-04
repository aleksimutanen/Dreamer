using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

    // Singleton related
    public static GameManager instance;
    public Transform toddlerHealthBar;
    public Slider toddlerHealthFill;
    public Transform buddyPowerBar;
    public Slider buddyPowerFill;
    public Transform dreamPowerBar;
    public Slider dreamPowerFill;
    public Transform gameStartPoint;
    public Vector3 checkpoint;
    public GameObject player;
    

    public float maxToddlerHealth;
    public float maxBuddyPower;
    public float maxDreamPower;

    public float toddlerHealth;
    public float buddyChargeSpeed = 1;
    public float buddyPower = 0;
    public float dreamPower = 0;
    public float dreamPowMem;
    
    float lives = 3;
    Vector3 prevPlayerPos;
    
    //public int crystalAmount = 0;
    //public TextMeshProUGUI statusText;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
        
        if (prevPlayerPos == player.transform.position){
            ChangeBuddyPower(buddyChargeSpeed * Time.deltaTime);
        }

        prevPlayerPos = player.transform.position;
        
    }

    private void Awake() {
        // Make a singleton instance out of (this) GameManager
        if(instance)
            Debug.LogError("2+ GameManagers found!");
        instance = this;
        checkpoint = gameStartPoint.position;
    }

    public void SetCheckpoint(){
        
        checkpoint = player.transform.position;
        dreamPowMem = dreamPower;
    }

    public void ALiveLost(){
        if(lives > 0){
            lives -= 1;
            player.gameObject.SetActive(false);
            player.gameObject.transform.position = checkpoint;
            player.gameObject.SetActive(true);      
            ChangeDreamPower(-(dreamPower-dreamPowMem));       
        } else {
            print("Game Over");
        }
    }

    // When crystal is collected
    public void ChangeDreamPower(float amount) {

        dreamPower += amount;
        dreamPower = Mathf.Clamp(dreamPower, 0, maxDreamPower);
        dreamPowerFill.value = dreamPower / maxDreamPower;
    }


    // When BuddyShield absorbs energy
    public void ChangeBuddyPower(float amount) {

        buddyPower += amount;
        buddyPower = Mathf.Clamp(buddyPower, 0, maxBuddyPower);
        buddyPowerFill.value = buddyPower / maxBuddyPower;
    }


    // When toddler takes damage
    public void ChangeToddlerHealth(float amount) {

        toddlerHealth += amount;
        toddlerHealth = Mathf.Clamp(toddlerHealth, 0, maxToddlerHealth);
        toddlerHealthFill.value = toddlerHealth / maxToddlerHealth;
    }
}
