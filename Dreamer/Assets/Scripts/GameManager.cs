﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


    public float maxToddlerHealth;
    public float maxBuddyPower;
    public float maxDreamPower;

    float toddlerHealth;
    float buddyPower = 0;
    float dreamPower = 0;

    public int crystalAmount = 0;
    public TextMeshProUGUI crystalText;

    private void Awake() {
        // Make a singleton instance out of (this) GameManager
        if(instance)
            Debug.LogError("2+ GameManagers found!");
        instance = this;
    }





    // When crystal is collected
    public void ChangeDreamPower(int amount) {

        dreamPower += amount;
        dreamPower = Mathf.Clamp(dreamPower, 0, maxDreamPower);
        dreamPowerFill.value = dreamPower / maxDreamPower;
    }
    public void ChangebuddyPower(int amount) {

        buddyPower += amount;
        buddyPower = Mathf.Clamp(buddyPower, 0, maxBuddyPower);
        buddyPowerFill.value = buddyPower / maxBuddyPower;
    }
    public void ChangeToddlerHealth(int amount) {

        toddlerHealth += amount;
        toddlerHealth = Mathf.Clamp(toddlerHealth, 0, maxToddlerHealth);
        toddlerHealthFill.value = toddlerHealth / maxToddlerHealth;
    }
}
