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

    public Camera drCam;
    public Camera nmCam;
    public Camera[] cameras;

    public RawImage nmFaderImage;
    public RawImage drFaderImage;

    [HideInInspector] public LayerMask map;
    [SerializeField] LayerMask dreamSolid;
    [SerializeField] LayerMask nightmareSolid;

    CharacterMover cm;

    public bool transitionOut;
    public bool transitionIn;

    private void Awake() {
        if (instance)
            Debug.LogError("2+ WorldSwitchers found!");
        instance = this;
    }

    void Start() {
        //nmFaderImage.gameObject.SetActive(false);
        map = dreamSolid;
        state = AwakeState.Dream;
        cm = FindObjectOfType<CharacterMover>();
        nmCam.gameObject.SetActive(false);

        //Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.PlaySound);
        //Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PlaySound);
        //Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PauseSound);
    }

    void Update() {
        TimedText xd = new TimedText("ykä on paras", 5f);

        if (Input.GetButtonDown("Switch") && !transitionIn && !transitionOut) {

            transitionOut = true;
            if(state == AwakeState.Dream) {
                //Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.PauseSound);
                //Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.UnpauseSound);
            } else {
                //Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.UnpauseSound);
                //Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PauseSound);
            }
        }
        if (state == AwakeState.Dream) {
            if (transitionOut || transitionIn) {
                Switch(fadeSpeed, /*1f,*/ transitionSpeed, drFaderImage, nmFaderImage, drCam, nmCam, cm.EnterNightmare, nightmareSolid, AwakeState.NightMare);
            }
            //Fabric.EventManager.Instance.PostEvent("Jump");
        }
        if (state == AwakeState.NightMare) {
            if (transitionOut || transitionIn) {
                Switch(fadeSpeed, /*0f,*/ transitionSpeed, nmFaderImage, drFaderImage, nmCam, drCam, cm.EnterDream, dreamSolid, AwakeState.Dream);
            }
        }
    }

    public void Switch(float fadeSpeed, /*float target,*/ float fadingSpeed, RawImage currentImage, RawImage newImage, Camera currentCam, 
                       Camera newCam, UnityAction afterTransition, LayerMask newSolid, AwakeState newState) {

        newCam.gameObject.SetActive(true);
        var b = newImage.color;
        var d = b.a;
        var c = currentImage.color;
        var a = c.a;
        //if (target < a)
        //    fadeSpeed *= -1;
        if (transitionOut) {
            a -= fadeSpeed * Time.deltaTime;
            d += fadeSpeed * Time.deltaTime;
            c.a = Mathf.Clamp01(a);
            b.a = Mathf.Clamp01(d);
            currentImage.color = c;
            newImage.color = b;
            foreach (Camera cam in cameras) {
                cam.fieldOfView += fadingSpeed * Time.deltaTime;
            }
        }
        if (a < 0 || a > 1){
            transitionOut = false;
            transitionIn = true;
            map = newSolid;
            afterTransition();
        }
        if (transitionIn) {
            foreach (Camera cam in cameras) {
                cam.fieldOfView -= Time.deltaTime * transitionSpeed * 2;
            }
            if ((state == AwakeState.NightMare && newCam.fieldOfView < 60) || (state == AwakeState.Dream && newCam.fieldOfView < 45)) {
                transitionIn = false;
                currentCam.gameObject.SetActive(false);
                state = newState;
                print(currentCam.gameObject.name);
            }
        }
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

