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
        Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.PlaySound);
        Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PlaySound);
        Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PauseSound);


    }

    void Update() {
        TimedText xd = new TimedText("ykä on paras", 5f);

        if(Input.GetButtonDown("Switch") /*&& !transitionIn*/) {
            transitionOut = true;
            if(state == AwakeState.Dream) {
                Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.PauseSound);
                Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.UnpauseSound);
            } else {
                Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.UnpauseSound);
                Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PauseSound);
            }
        }
        if (state == AwakeState.Dream) {

            //SwitchWorld(dreamCam, nmCam, lightOn, cm.EnterNightmare, nightmareSolid, AwakeState.NightMare);
            if (transitionOut) {
                Switch(fadeSpeed, 1f, cm.EnterNightmare, nightmareSolid, AwakeState.NightMare);
            }
            //Fabric.EventManager.Instance.PostEvent("Jump");


        }
        if (state == AwakeState.NightMare) {
            //SwitchWorld(nmCam, dreamCam, lightOff, cm.EnterDream, dreamSolid, AwakeState.Dream);
            if (transitionOut) {
                Switch(fadeSpeed, 0f, cm.EnterDream, dreamSolid, AwakeState.Dream);
            }

        }
    }

    public void Switch(float fadeSpeed, float target, UnityAction afterTransition, LayerMask newSolid, AwakeState newState) {
        var c = faderImage.color;
        var a = c.a;
        if (target < a)
            fadeSpeed *= -1;
        if (transitionOut) {
            a += fadeSpeed * Time.deltaTime;
            c.a = Mathf.Clamp01(a);
            faderImage.color = c;
        }
        if (a < 0 || a > 1) {
            print("ihamitövaa");
            transitionOut = false;
            state = newState;
            map = newSolid;
            afterTransition();
        }
        //if (fadeSpeed > 0) {
        //    if (c.a > target) {
        //        transitionOut = false;
        //        state = newState;
        //        map = newSolid;
        //        afterTransition();
        //    }
        //} else if (fadeSpeed < 0) {
        //    if (c.a < target) {
        //        transitionOut = false;
        //        state = newState;
        //        map = newSolid;
        //        afterTransition();

        //    }
        //}
    }

    //void SwitchWorld(Camera currentCamera, Camera newCamera, bool light, UnityAction afterTransition, LayerMask newSolid, AwakeState newState) {
    //    //if (transitionIn == false && transitionOut == false) return;
    //    if (Input.GetButtonDown("Switch") && !transitionIn) {
    //        transitionOut = true;
    //    }
    //    if (transitionOut) {
    //        currentCamera.fieldOfView += Time.deltaTime * transitionSpeed; // current
    //        foreach (Light l in lights) {
    //            l.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 1f * Time.deltaTime);
    //        }
    //        if (currentCamera.fieldOfView > 178) { // current
    //            transitionIn = true;
    //            newCamera.fieldOfView = 179f; // new
    //            currentCamera.gameObject.SetActive(false); // current
    //            newCamera.gameObject.SetActive(true); //new
    //            nightmareLight.gameObject.SetActive(light);
    //            transitionOut = false;
    //        }
    //    }
    //    if (transitionIn) {
    //        newCamera.fieldOfView -= Time.deltaTime * transitionSpeed; // new
    //        if (newCamera.fieldOfView < 60) { // new
    //            newCamera.fieldOfView = 60f;
    //            transitionIn = false;
    //            afterTransition();
    //            map = newSolid;
    //            state = newState;
    //        }
    //    }
    //}
}

