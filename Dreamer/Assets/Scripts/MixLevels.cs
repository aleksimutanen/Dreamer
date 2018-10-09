using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MixLevels : MonoBehaviour {
    public AudioMixer masterMixer;

    public void SetSoundSFXLevel(float sfxLevel){
        masterMixer.SetFloat("sfxVolume", sfxLevel);
    }

    public void SetMusicVolume(float musicLevel){
        masterMixer.SetFloat("musicVolume", musicLevel);
    }


}
