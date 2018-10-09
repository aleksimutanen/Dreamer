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
    public Vector3 previousCheckpoint;
    public Quaternion previousCheckRotation;
    public GameObject player;
    public GameObject vertRot;
    public GameObject horRot;
    public GameObject pauseMenu;
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
    
    float lives = 100;
    public bool gamePaused = false;
    public bool gameOver = false;
    public float waitTimer = 1;
    public float waitTime = 1;
    public Vector3 prevPlayerPos;

    public Bat sleepingBat;
    public Transform flyArea;

    public List<GameObject> doors = new List<GameObject>(3);

    float maxToddlerHealth = 100;
    float maxBuddyPower = 100;
    int maxCrystalAmount = 10;

    float toddlerChargeSpeed = 1;
    float toddlerHealth = 100;
    float buddyChargeSpeed = 1;
    public float buddyPower = 0;
    public int crystalAmount = 0;
    float dreamPowMem;

    int nextDoor = 0;
    int doorOpen = 10;
    bool openDoor;

    List<ScriptableText> textDialogQueue;
    
    public void PlayTextDialog(ScriptableText st) {
        textDialogQueue.Add(st);   
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }

        if (Input.anyKeyDown && gameOver && waitTimer < 0){
            LoadMenu();
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
            MoveDoor(doors[nextDoor]);
        }
        waitTimer -= Time.unscaledDeltaTime;
    }

    private void Awake() {
        sleepingBat.sleeping = true;
        sleepingBat.startPos = flyArea.position;
        // Make a singleton instance out of (this) GameManager
        if(instance) {
            Debug.LogError("2+ GameManagers found!");
        }
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
        tutorialTexts.Add("Kill tree with BunnyPowers, block hits to charge press (Left Shift) and releaseby pressing (b)");
        tutorialTexts.Add("That bat looks explosive! Maybe you can get rid of the roadblocking stones if you make it go boom!");
        tutorialTexts.Add("You Ded, PERMANENTLY LOL!");
        tutorialTexts.Add("You REALLY should pick up the crystal!");
        ChangeStatusText(tutorialTexts[0], 3);
    }

    public void SetCheckpoint(){
        previousCheckpoint = checkpoint;
        previousCheckRotation = checkRotation;
        checkpoint = player.transform.position;
        checkRotation = player.transform.rotation;
    }

    public void ALiveLost(){
        if(lives > 1){
            lives --;
            print("Life lost");
            buddyPower = 0;
            toddlerHealth = 100;
            TeleportToCheckPoint(false, false);
            ChangeStatusText(tutorialTexts[11], 3);
            Fabric.EventManager.Instance.PostEvent("Death");
            //ChangeDreamPower(-(dreamPower-dreamPowMem));       
        } else if(!gameOver){
            ChangeStatusText(tutorialTexts[14], 3);
            print("Game Over");
            Fabric.EventManager.Instance.PostEvent("Death");
            gameOver = true;
            Time.timeScale = 0;
            waitTimer = waitTime;

        }
    }

    public void TeleportToCheckPoint(bool flipDir, bool previous) {

        player.SetActive(false);
        if(previous){
            player.transform.position = previousCheckpoint;
            if(flipDir) {
                player.transform.rotation = Quaternion.Euler(checkRotation.eulerAngles.x, checkRotation.eulerAngles.y + 180, checkRotation.eulerAngles.z);
            } else {
                player.transform.rotation = checkRotation;
            }
        } else {
            player.transform.position = checkpoint;
            if(flipDir) {
                player.transform.rotation = Quaternion.Euler(checkRotation.eulerAngles.x, checkRotation.eulerAngles.y + 180, checkRotation.eulerAngles.z);
            } else {
                player.transform.rotation = checkRotation;
            }
        }
        player.SetActive(true);
        vertRot.GetComponent<VerticalRotator>().ResetRotation();
        horRot.GetComponent<HorizontalRotator>().ResetRotation();
    }

    // When crystal is collected
    public void ChangeCrystalAmount(int amount) {

        crystalAmount += amount;
        crystalAmount = Mathf.Clamp(crystalAmount, 0, maxCrystalAmount);
        dreamPowerFill.value = crystalAmount / (float)maxCrystalAmount;
        print("Kristalli poimittu");
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
            if(!gameOver)
            ALiveLost();
        }
    }
    
    // Status text
    public void ChangeStatusText(string text, float timer){
        statusText.text = text;
        statusTextTimer = timer;
    }

    void MoveDoor (GameObject door) {
        if (door.activeSelf) {
            door.SetActive(false);
            openDoor = false;
            nextDoor++;
            doorOpen += 10;
            return;
        }
    }
    public void Pause() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        gamePaused = !gamePaused;
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void LoadMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
