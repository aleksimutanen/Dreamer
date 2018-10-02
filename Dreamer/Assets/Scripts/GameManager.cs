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
    public bool tutorialComplete = false;

    public List<string> tutorialTexts = new List<string>();

    public bool toddlerMoving = false;
    public bool lookEnabled = false;
    public bool walkEnabled = false;
    public bool jumpEnabled = false;
    public bool switchEnabled = false;
    public bool bashEnabled = false;
    public bool glideEnabled = false;
    public bool shieldEnabled = false;
    public bool firingEnabled = false;
    public bool powerBallEnabled = false;
    public bool reflectionEnabled = false;
    
    public float lives = 300;
    public Vector3 prevPlayerPos;

    public Bat sleepingBat;

    public List<Transform> doors = new List<Transform>(6);

    float maxToddlerHealth = 100;
    float maxBuddyPower = 100;
    float maxDreamPower = 100;

    //float toddlerChargeSpeed = 1;
    float toddlerHealth = 100;
    float buddyChargeSpeed = 1;
    public float buddyPower = 0;
    public float crystalAmount = 0;
    float dreamPowMem;

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
        /*if (prevPlayerPos == player.transform.position){
            ChangeBuddyPower(buddyChargeSpeed * Time.deltaTime);
        }*/
        
        // ToddlerHealth charging while idling
        if (prevPlayerPos == player.transform.position){
            ChangeToddlerHealth(buddyChargeSpeed * Time.deltaTime);
        }
        
        prevPlayerPos = player.transform.position;
        
        statusTextTimer -= Time.deltaTime;
        
        if (statusTextTimer < 0){
            ChangeStatusText("",1);
        }

        if (crystalAmount == doorOpen) {
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
            tutorialTexts.Add("You are now ready for your adventure, go on little one!");
            tutorialTexts.Add("Bashing Skillz, try it out by pressing 'b' for a while!");
            tutorialTexts.Add("Gliiidddeeerrr Skillllz!");
            tutorialTexts.Add("Time to reflect some things!");
            tutorialTexts.Add("Go find some more crystals, Your bunny can help you");
            tutorialTexts.Add("You Ded!");

            ChangeStatusText(tutorialTexts[0], 3);
        }

    public void SetCheckpoint(){    
        checkpoint = player.transform.position;
        //dreamPowMem = dreamPower;
        checkRotation = player.transform.rotation;
    }

    public void ALiveLost(){
        if(lives > 0){
            lives --;
            print("Life lost");
            buddyPower = 0;
            toddlerHealth = 100;
            TeleportToCheckPoint(false);
            ChangeStatusText(tutorialTexts[11], 3);
            //ChangeDreamPower(-(dreamPower-dreamPowMem));       
        } else {
            print("Game Over");
        }
    }

    public void TeleportToCheckPoint(bool flipDir) {
        player.SetActive(false);
        player.transform.position = checkpoint;
        if(flipDir) {
            player.transform.rotation = Quaternion.Euler(checkRotation.eulerAngles.x, checkRotation.eulerAngles.y + 180, checkRotation.eulerAngles.z);
        } else {
            player.transform.rotation = checkRotation;
        }
        player.SetActive(true);
        vertRot.GetComponent<VerticalRotator>().ResetRotation();
        horRot.GetComponent<HorizontalRotator>().ResetRotation();
    }

    // When crystal is collected
    public void ChangeDreamPower(float amount) {

        crystalAmount += amount;
        crystalAmount = Mathf.Clamp(crystalAmount, 0, maxDreamPower);
        dreamPowerFill.value = crystalAmount / maxDreamPower;
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
        if (toddlerHealth <= 0) {
            ALiveLost();
        }
    }
    
    // Status text
    public void ChangeStatusText(string text, float timer){
        statusText.text = text;
        statusTextTimer = timer;
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
