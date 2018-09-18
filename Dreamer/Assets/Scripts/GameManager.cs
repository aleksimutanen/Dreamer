using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class GameManager : MonoBehaviour {
    public ScriptableText test;
       
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
    public Quaternion checkRotation;
    public GameObject player;
    public GameObject vertRot;
    public GameObject horRot;
    public TextMeshProUGUI statusText;
    public float statusTextTimer = 0;
    public bool statusTextEmpty = true;
    public int tutorialIndex = 0;
    public bool tutorialComplete = false;
    
    public List<string> tutorialTexts = new List<string>();

    public float maxToddlerHealth;
    public float maxBuddyPower;
    public float maxDreamPower;

    public float toddlerChargeSpeed = 1;
    public float toddlerHealth = 0;
    public float buddyChargeSpeed = 1;
    public float buddyPower = 0;
    public float dreamPower = 0;
    public float dreamPowMem;
    
    public bool lookEnabled = false;
    public bool walkEnabled = false;
    public bool jumpEnabled = false;
    public bool switchEnabled = false;
    public bool bashEnabled = false;
    public bool glideEnabled = false;
    public bool shieldEnabled = false;
    public bool firingEnabled = false;
    public bool powerBallEnabled = false;
    
    public float lives = 3;
    Vector3 prevPlayerPos;

    public Bat sleepingBat;

    public List<Transform> doors = new List<Transform>(6);
    int nextDoor = 0;
    float doorOpen = 10;
    bool openDoor;

    //public int crystalAmount = 0;
    //public TextMeshProUGUI statusText;

    List<ScriptableText> textDialogQueue;
    
    public void PlayTextDialog(ScriptableText st) {
        textDialogQueue.Add(st);   
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
        
        // Buddypower charging while idling
        if (prevPlayerPos == player.transform.position){
            ChangeBuddyPower(buddyChargeSpeed * Time.deltaTime);
        }
        
        // ToddlerHealth charging while idling
        if (prevPlayerPos == player.transform.position){
            ChangeToddlerHealth(buddyChargeSpeed * Time.deltaTime);
        }
        
        prevPlayerPos = player.transform.position;
        
        statusTextTimer -= Time.deltaTime;
        // look enabled
        if (tutorialIndex == 1 && statusTextTimer < 0 && statusTextEmpty){
            ChangeStatusText(tutorialTexts[tutorialIndex], 5);
            tutorialIndex++;
            statusTextEmpty = false;
            lookEnabled = true;
        }
        // walk enabled
        if (tutorialIndex == 2 && statusTextTimer < 0 && statusTextEmpty){
            ChangeStatusText(tutorialTexts[tutorialIndex],5);
            tutorialIndex++;
            statusTextEmpty = false;
            walkEnabled = true;
        }
        // feeling
        if (tutorialIndex == tutorialTexts.Count && statusTextTimer < 0 && statusTextEmpty && !tutorialComplete){
            ChangeStatusText(tutorialTexts[tutorialIndex],5);
            tutorialComplete = true;
            statusTextEmpty = false;
        }
        
        if (statusTextTimer < 0){
            ChangeStatusText("",1);
            statusTextEmpty = true;
        }

        if (dreamPower == doorOpen) {
            openDoor = true;
        }

        if (openDoor) { 
            print("door open");
            MoveDoor(doors[nextDoor], doors[nextDoor+1]);

        }

    }

    private void Awake() {
        sleepingBat.sleeping = true;
        // Make a singleton instance out of (this) GameManager
        if(instance)
            Debug.LogError("2+ GameManagers found!");
            instance = this;
            checkpoint = gameStartPoint.position;
            tutorialTexts.Add("Welcome to your dream, I'm Mother and I will guide you through your journey!");
            tutorialTexts.Add("You can look around by moving your mouse or controller tatti. Now look around you");
            tutorialTexts.Add("Well done! You can also move here =) Use your wasd or the other tatti to move");
            tutorialTexts.Add("Press space or joystick button x to jump over obstacles");
            tutorialTexts.Add("When you see crystals like this you should pick them up!");
            tutorialTexts.Add("Sometimes when you feel you are in a bad spot, try switching to nightmare by pressing the 'e'" + " button.");
            tutorialTexts.Add("Your bunny can help you find some crystals!");
            tutorialTexts.Add("You did really good. Now move along little one!");
            ChangeStatusText(tutorialTexts[tutorialIndex], 1);
            tutorialIndex++;
        }

    public void SetCheckpoint(){
        
        checkpoint = player.transform.position;
        checkRotation = player.transform.rotation;
        dreamPowMem = dreamPower;
    }

    public void ALiveLost(){
        if(lives > 0){
            lives --;
            print("Life lost");
            player.gameObject.SetActive(false);
            player.gameObject.transform.position = checkpoint;
            player.gameObject.transform.rotation = checkRotation;
            player.gameObject.SetActive(true);
            vertRot.GetComponent<VerticalRotator>().ResetRotation();
            horRot.GetComponent<HorizontalRotator>().ResetRotation();
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
    
    // Status text
    public void ChangeStatusText(string text, float timer){
        statusText.text = text;
        statusTextTimer = timer;
        statusTextEmpty = false;
    }

    void MoveDoor (Transform door, Transform doorEndPos) {
        print(door.GetComponent<Renderer>().bounds.center.z);
        print(doorEndPos.position.z + " endpos");
        if (door.GetComponent<Renderer>().bounds.center.z <= doorEndPos.position.z) {
            openDoor = false;
            print("door stopped");
            nextDoor += 2;
            doorOpen += 10;
            return;
        }
        door.Translate(-door.forward * Time.deltaTime *2);
    }
}
