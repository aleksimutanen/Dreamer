using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum AwakeState { Dream, NightMare }

public class WorldSwitch : MonoBehaviour {

    public float fadeSpeed;
    public float transitionSpeed;
    public static WorldSwitch instance;
    public AwakeState state;
    public Light[] lights;
    public Light nightmareLight;

    public Camera dreamCam;
    public Camera nmCam;

    public RawImage faderImage;

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
        //nmCam.gameObject.SetActive(false);
        cm = FindObjectOfType<CharacterMover>();
    }

    void Update() {

        //if (Input.GetButtonDown("Switch")) {
        //    //SwitchState();
        //    transitionOut = true;
        //}
        //if (transitionOut) {
        //    var c = mr.material.color;
        //    c.a += 0.5f * Time.deltaTime;
        //    mr.material.color = c;
        //    //nightmareLight.gameObject.SetActive(true);
        //    if (c.a > 1) {
        //        transitionOut = false;
        //    }
        //}
        if (state == AwakeState.Dream) {
            //SwitchWorld(dreamCam, nmCam, lightOn, cm.EnterNightmare, nightmareSolid, AwakeState.NightMare);
            Switch(fadeSpeed, 1f, cm.EnterNightmare, nightmareSolid, AwakeState.NightMare);
        }
        if (state == AwakeState.NightMare) {
            //SwitchWorld(nmCam, dreamCam, lightOff, cm.EnterDream, dreamSolid, AwakeState.Dream);
            Switch(-fadeSpeed, 0f, cm.EnterDream, dreamSolid, AwakeState.Dream);
        }
    }

    public void Switch(float fadeDir, float max, UnityAction afterTransition, LayerMask newSolid, AwakeState newState) {
        var c = faderImage.color;
        if (Input.GetButtonDown("Switch") && !transitionIn) {
            transitionOut = true;
        }
        if (transitionOut) {
            c.a += fadeDir * Time.deltaTime;
            faderImage.color = c;
            //print(c.a);
        }
        if (fadeDir > 0) {
            if (c.a > max) {
                transitionOut = false;
                state = AwakeState.NightMare;
                state = newState;
                map = newSolid;
                afterTransition();
            }
        } else if (fadeDir < 0) {
            if (c.a < max) {
                transitionOut = false;
                state = AwakeState.Dream;
                state = newState;
                map = newSolid;
                afterTransition();

            }
        }
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

    

    //public void SwitchState() {
    //    if (state == AwakeState.Dream) {
    //        state = AwakeState.NightMare;
    //        map = nightmareSolid;
    //        cm.EnterNightmare();
    //        //foreach (Light l in lights) {
    //        //    l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 500);
    //        //mr.material.color.a = 0.5f;
    //        var c = mr.material.color;
    //        c.a = 1f;
    //        mr.material.color = c;

    //        //var p = transform.position;
    //        //p.x = 1;
    //        //transform.position = p;
    //       //}
    //        nightmareLight.gameObject.SetActive(true);
    //        //dreamCam.gameObject.SetActive(false);
    //        //nmCam.gameObject.SetActive(true);
    //    } else if (state == AwakeState.NightMare /*&& GameManager.instance.dreamPower >= 1*/) {
    //        map = dreamSolid;
    //        cm.EnterDream();
    //        //GameManager.instance.ChangeDreamPower(-1f);
    //        state = AwakeState.Dream;
    //        //foreach (Light l in lights) {
    //        //    l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 500);
    //        var c = mr.material.color;
    //        c.a = 0f;
    //        mr.material.color = c;
    //        //}
    //        nightmareLight.gameObject.SetActive(false);
    //        //nmCam.gameObject.SetActive(false);
    //        //dreamCam.gameObject.SetActive(true);
    //    }
    //}
}

