using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AwakeState { Dream, NightMare }

public class WorldSwitch : MonoBehaviour {

    public float transitionSpeed;
    public static WorldSwitch instance;
    public AwakeState state;
    public Light[] lights;
    public Light nightmareLight;

    public Camera dreamCam;
    public Camera nmCam;

    [HideInInspector] public LayerMask map;
    [SerializeField] LayerMask dreamSolid;
    [SerializeField] LayerMask nightmareSolid;

    CharacterMover cm;

    bool lightOn = true;
    bool lightOff = false;
    public bool transitionOut;
    public bool transitionIn;

    private void Awake() {
        if (instance)
            Debug.LogError("2+ WorldSwitchers found!");
        instance = this;
    }

    void Start() {
        map = dreamSolid;
        state = AwakeState.Dream;
        nmCam.gameObject.SetActive(false);
        cm = FindObjectOfType<CharacterMover>();
    }

    void Update() {

        if (Input.GetButtonDown("Switch")) {
            SwitchState();
        }
        //if (state == AwakeState.Dream) {
        //    SwitchWorld(dreamCam, nmCam, lightOn, cm.EnterNightmare, nightmareSolid, AwakeState.NightMare);
        //}
        //if (state == AwakeState.NightMare) {
        //    SwitchWorld(nmCam, dreamCam, lightOff, cm.EnterDream, dreamSolid, AwakeState.Dream);
        //}
    }

    void SwitchWorld(Camera currentCamera, Camera newCamera, bool light, UnityAction afterTransition, LayerMask newSolid, AwakeState newState) {
        //if (transitionIn == false && transitionOut == false) return;
        if (Input.GetButtonDown("Switch") && !transitionIn) {
            transitionOut = true;
        }
        if (transitionOut) {
            currentCamera.fieldOfView += Time.deltaTime * transitionSpeed; // current
            foreach (Light l in lights) {
                l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 1f * Time.deltaTime);
            }
            if (currentCamera.fieldOfView > 178) { // current
                transitionIn = true;
                newCamera.fieldOfView = 179f; // new
                currentCamera.gameObject.SetActive(false); // current
                newCamera.gameObject.SetActive(true); //new
                nightmareLight.gameObject.SetActive(light);
                transitionOut = false;
            }
        }
        if (transitionIn) {
            newCamera.fieldOfView -= Time.deltaTime * transitionSpeed; // new
            if (newCamera.fieldOfView < 60) { // new
                newCamera.fieldOfView = 60f;
                transitionIn = false;
                afterTransition();
                map = newSolid;
                state = newState;
            }
        }
    }

    //void DreamToNightmare(UnityAction afterTransition) {
    //    if (Input.GetButtonDown("Switch")) {
    //        transitionOut = true;
    //    }
    //    if (transitionOut) {
    //        dreamCam.fieldOfView += Time.deltaTime * transitionSpeed;
    //        foreach (Light l in lights) {
    //            l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 1f * Time.deltaTime);
    //        }
    //        if (dreamCam.fieldOfView > 178) {
    //            transitionIn = true;
    //            nmCam.fieldOfView = 179f;
    //            dreamCam.gameObject.SetActive(false);
    //            nmCam.gameObject.SetActive(true);
    //            nightmareLight.gameObject.SetActive(true);
    //            transitionOut = false;
    //        }
    //    }
    //    if (transitionIn) {
    //        nmCam.fieldOfView -= Time.deltaTime * transitionSpeed;
    //        if (nmCam.fieldOfView < 60) {
    //            transitionIn = false;
    //            afterTransition();
    //            map = nightmareSolid;
    //            state = AwakeState.NightMare;
    //        }
    //    }
    //}

    //void NightmareToDream(UnityAction afterTransition) {
    //    if (Input.GetButtonDown("Switch")) {
    //        transitionOut = true;
    //    }
    //    if (transitionOut) {
    //        nmCam.fieldOfView += Time.deltaTime * transitionSpeed; 
    //        foreach (Light l in lights) {
    //            l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 1f * Time.deltaTime);
    //        }
    //        if (nmCam.fieldOfView > 178) { 
    //            transitionIn = true;
    //            dreamCam.fieldOfView = 179f;
    //            nmCam.gameObject.SetActive(false);  
    //            dreamCam.gameObject.SetActive(true);
    //            nightmareLight.gameObject.SetActive(false);
    //            transitionOut = false;
    //        }
    //    }
    //    if (transitionIn) {
    //        dreamCam.fieldOfView -= Time.deltaTime * transitionSpeed;
    //        if (dreamCam.fieldOfView < 60) {
    //            transitionIn = false;
    //            cm.EnterDream();
    //            map = dreamSolid;
    //            state = AwakeState.Dream;
    //        }
    //    }
    //}

    public void SwitchState() {
        if (state == AwakeState.Dream) {
            state = AwakeState.NightMare;
            map = nightmareSolid;
            cm.EnterNightmare();
            foreach (Light l in lights) {
                l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 500);
            }
            nightmareLight.gameObject.SetActive(true);
            dreamCam.gameObject.SetActive(false);
            nmCam.gameObject.SetActive(true);
        } else if (state == AwakeState.NightMare && GameManager.instance.dreamPower >= 1) {
            map = dreamSolid;
            cm.EnterDream();
            GameManager.instance.ChangeDreamPower(-1f);
            state = AwakeState.Dream;
            foreach (Light l in lights) {
                l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 500);
            }
            nightmareLight.gameObject.SetActive(false);
            nmCam.gameObject.SetActive(false);
            dreamCam.gameObject.SetActive(true);
        }
    }
}

