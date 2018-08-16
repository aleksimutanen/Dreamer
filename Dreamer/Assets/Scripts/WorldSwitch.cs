using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AwakeState { Wake, NightMare }

public class WorldSwitch : MonoBehaviour {

    public AwakeState state;
    public Light awakeLight;
    public Light nightmareLight;
    Animator anim;
    public AnimationClip LowToHigh;
    public AnimationClip HighToLow;
    bool playing;


    void Start() {
        state = AwakeState.Wake;
        anim = GetComponent<Animator>();
    }

    void Update() {
        //var swi = Input.GetAxis("Switch");
        //      if (swi > 0) {

        //      }
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1)) {
            //SwitchWorld();
            if (state == AwakeState.Wake) {
                state = AwakeState.NightMare;
                StartCoroutine(MoveLight(230f, 1.5f));
            } else if (state == AwakeState.NightMare) {
                state = AwakeState.Wake;
                StartCoroutine(MoveLight(50f, 1.5f));
            }
        }
    }

    //public void SwitchWorld() {
    //    if (state == AwakeState.Wake) {
    //        state = AwakeState.NightMare;
    //        awakeLight.transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(230, 0, 0, 0), 500000);
    //        nightmareLight.gameObject.SetActive(true);
    //    } else if (state == AwakeState.NightMare) {
    //        state = AwakeState.Wake;
    //        awakeLight.transform.rotation = Quaternion.RotateTowards(transform.rotation, new Quaternion(50, 0, 0, 0), 500);
    //        nightmareLight.gameObject.SetActive(false);
    //    }
    //}

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

