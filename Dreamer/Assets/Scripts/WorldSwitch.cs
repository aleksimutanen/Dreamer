using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Rendering;

public enum AwakeState { Dream, Nightmare }

public class WorldSwitch : MonoBehaviour {

    public float fadeSpeed;
    public float transitionSpeed;

    public static WorldSwitch instance;

    public AwakeState state;

    public Light dreamLight;
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

    public bool switchNow = false;

    public Material dreamSbMaterial;
    public Material nightmareSbMaterial;
    public Color dreamAmbientColor;
    public Color nightmareAmbientColor;

    Color c;
    Color b;
    float d;
    float a;

    float blendTime;

    private void Awake() {
        if (instance)
            Debug.LogError("2+ WorldSwitchers found!");
        instance = this;
    }

    void Start() {
        RenderSettings.skybox.Lerp(dreamSbMaterial, nightmareSbMaterial, 0);
        RenderSettings.ambientLight = dreamAmbientColor;
        map = dreamSolid;
        state = AwakeState.Dream;
        cm = FindObjectOfType<CharacterMover>();
        nmCam.gameObject.SetActive(false);

        Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.PlaySound);
        Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PlaySound);
        Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PauseSound);
    }

    void Update() {
        //TimedText xd = new TimedText("ykä on paras", 5f);
        if (Input.GetButtonDown("Switch") && !transitionIn && !transitionOut&&GameManager.instance.switchEnabled == true||switchNow) {
            switchNow = false;
            transitionOut = true;
            Fabric.EventManager.Instance.PostEvent("Switch");
            if(state == AwakeState.Dream) {
                Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.PauseSound);
                Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.UnpauseSound);
            } else {
                Fabric.EventManager.Instance.PostEvent("DreamMusic", Fabric.EventAction.UnpauseSound);
                Fabric.EventManager.Instance.PostEvent("NightmareMusic", Fabric.EventAction.PauseSound);
            }
        }
        if (state == AwakeState.Dream) {
            if (transitionOut || transitionIn) {
                Switch(fadeSpeed, -transitionSpeed, drFaderImage, nmFaderImage, drCam, nmCam, dreamLight, nightmareLight, 1f, 2f,
                cm.EnterNightmare, nightmareSolid, AwakeState.Nightmare, dreamSbMaterial, nightmareSbMaterial, dreamAmbientColor, nightmareAmbientColor);
            }
            //Fabric.EventManager.Instance.PostEvent("Jump");
        }
        if (state == AwakeState.Nightmare) {
            if (transitionOut || transitionIn) {
                Switch(fadeSpeed, transitionSpeed, nmFaderImage, drFaderImage, nmCam, drCam, nightmareLight, dreamLight, 2f, 1f,
                cm.EnterDream, dreamSolid, AwakeState.Dream, nightmareSbMaterial, dreamSbMaterial, nightmareAmbientColor, dreamAmbientColor);
            }
        }
    }

    public void Switch(float fadeSpeed, float fadingSpeed, RawImage currentImage, RawImage newImage, Camera currentCam, Camera newCam, Light currentLight, Light newLight, float currentLightFade,
    float newLightFade, UnityAction afterTransition, LayerMask newSolid, AwakeState newState, Material currentSbMaterial, Material newSbMaterial, Color currentAmbientColor, Color newAmbientColor) {

        if (b != newImage.color || c != currentImage.color) {
            newLight.gameObject.SetActive(true);
            newCam.gameObject.SetActive(true);
            b = newImage.color;
            d = b.a;
            c = currentImage.color;
            a = c.a;
        }
        if (transitionOut) {
            blendTime += fadeSpeed * Time.deltaTime;
            RenderSettings.skybox.Lerp(currentSbMaterial, newSbMaterial, blendTime);
            RenderSettings.ambientLight = Color.Lerp(currentAmbientColor, newAmbientColor, blendTime);
            a -= fadeSpeed * Time.deltaTime;
            d += fadeSpeed * Time.deltaTime;
            c.a = Mathf.Clamp01(a);
            b.a = Mathf.Clamp01(d);
            currentImage.color = c;
            newImage.color = b;
            currentLight.intensity -= currentLightFade * Time.deltaTime;
            newLight.intensity += newLightFade * Time.deltaTime;
            foreach (Camera cam in cameras) {
                cam.fieldOfView += fadingSpeed * Time.deltaTime;
            }
        }
        if (a < 0 || a > 1) {
            DynamicGI.UpdateEnvironment();
            blendTime = 0f;
            currentLight.gameObject.SetActive(false);
            transitionOut = false;
            transitionIn = true;
            map = newSolid;
            afterTransition();
        }
        if (transitionIn) {
            //foreach (Camera cam in cameras) {
            //    cam.fieldOfView -= Time.deltaTime * transitionSpeed * 2;
            //}
            if ((state == AwakeState.Nightmare/* && newCam.fieldOfView < 60*/) || (state == AwakeState.Dream/* && newCam.fieldOfView < 45*/)) {
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

