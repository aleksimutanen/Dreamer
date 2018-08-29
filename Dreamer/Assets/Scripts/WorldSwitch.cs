using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool switching;
    public bool switching2;

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

        if (state == AwakeState.Dream) {
            DreamToNightmare();
        }
        if (state == AwakeState.NightMare) {
            NightmareToDream();
        }
        //if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1)) {
        //if (Input.GetButtonDown("Switch")) {
        //    SwitchWorld();
        //    switching = true;
        //}
    }

    void DreamToNightmare() {
        if (Input.GetButtonDown("Switch")) {
            switching = true;
        }
        if (switching) {
            dreamCam.fieldOfView += Time.deltaTime * transitionSpeed;
            foreach (Light l in lights) {
                l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 1f * Time.deltaTime);
            }
            if (dreamCam.fieldOfView > 178) {
                switching2 = true;
                nmCam.fieldOfView = 179f;
                dreamCam.gameObject.SetActive(false);
                nmCam.gameObject.SetActive(true);
                nightmareLight.gameObject.SetActive(true);
                switching = false;
            }
        }
        if (switching2) {
            nmCam.fieldOfView -= Time.deltaTime * transitionSpeed;
            if (nmCam.fieldOfView < 60) {
                switching2 = false;
                cm.EnterNightmare();
                map = nightmareSolid;
                state = AwakeState.NightMare;
            }
        }
    }

    void NightmareToDream() {
        if (Input.GetButtonDown("Switch")) {
            switching = true;
        }
        if (switching) {
            nmCam.fieldOfView += Time.deltaTime * transitionSpeed; 
            foreach (Light l in lights) {
                l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 1f * Time.deltaTime);
            }
            if (nmCam.fieldOfView > 178) { 
                switching2 = true;
                dreamCam.fieldOfView = 179f;
                nmCam.gameObject.SetActive(false);  
                dreamCam.gameObject.SetActive(true);
                nightmareLight.gameObject.SetActive(false);
                switching = false;
            }
        }
        if (switching2) {
            dreamCam.fieldOfView -= Time.deltaTime * transitionSpeed;
            if (dreamCam.fieldOfView < 60) {
                switching2 = false;
                cm.EnterDream();
                map = nightmareSolid;
                state = AwakeState.Dream;
            }
        }
    }

    //void ChangeWorld(AwakeState state, Camera currentCamera, Camera newCamera, bool light) {
    //    //if (state == AwakeState.Dream) {
    //    if (currentCamera.fieldOfView < 179f) {
    //        currentCamera.fieldOfView += Time.deltaTime * 150;
    //        foreach (Light l in lights) {
    //            l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 0.5f * Time.deltaTime);
    //        }
    //    }
    //    if (currentCamera.fieldOfView > 178) {
    //        newCamera.fieldOfView = 179f;
    //        currentCamera.gameObject.SetActive(false);
    //        newCamera.gameObject.SetActive(true);
    //        nightmareLight.gameObject.SetActive(light);
    //    }
    //    if (newCamera.fieldOfView <= 179f) {
    //        newCamera
    //    }
    //}

    public void SwitchWorld() {
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

