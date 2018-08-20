using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AwakeState { Dream, NightMare }

public class WorldSwitch : MonoBehaviour {

    public AwakeState state;
    public Light awakeLight;
    public Light nightmareLight;

    public Camera wakeCam;
    public Camera nmCam;

    void Start() {
        state = AwakeState.Dream;
        nmCam.gameObject.SetActive(false);
    }

    void Update() {

        //if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1)) {
            if (Input.GetButtonDown("Switch")) {

                SwitchWorld();
        }
    }

    public void SwitchWorld() {
        if (state == AwakeState.Dream) {
            state = AwakeState.NightMare;
            //awakeLight.gameObject.SetActive(false);
            awakeLight.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 500);
            nightmareLight.gameObject.SetActive(true);
            wakeCam.gameObject.SetActive(false);
            nmCam.gameObject.SetActive(true);
            //StartCoroutine(MoveLight(230f, 1.5f));
        } else if (state == AwakeState.NightMare) {
            state = AwakeState.Dream;
            //StartCoroutine(MoveLight(50f, 1.5f));
            //awakeLight.gameObject.SetActive(true);
            awakeLight.transform.rotation *= Quaternion.RotateTowards(transform.rotation, new Quaternion(-180, 0, 0, 0), 500);
            nightmareLight.gameObject.SetActive(false);
            nmCam.gameObject.SetActive(false);
            wakeCam.gameObject.SetActive(true);
        }
        
    }

    //IEnumerable MoveLight() {
    //    if (state == AwakeState.Wake) {
    //        state = AwakeState.NightMare;
    //        awakeLight.transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(230, 0, 0, 0), 50 * Time.deltaTime);
    //        nightmareLight.gameObject.SetActive(true);
    //        yield return new WaitForSeconds(1.5f);
    //    } else if (state == AwakeState.NightMare) {
    //        state = AwakeState.Wake;
    //        awakeLight.transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(50, 0, 0, 0), 50 * Time.deltaTime);
    //        nightmareLight.gameObject.SetActive(false);
    //        yield return new WaitForSeconds(1.5f);
    //    } else {
    //        yield return null;
    //    }
    //}
    public IEnumerator MoveLight(float angle, float time) {
        awakeLight.transform.rotation = Quaternion.Euler(angle, 0, 0);
        yield return new WaitForSeconds(time);
    }
}

