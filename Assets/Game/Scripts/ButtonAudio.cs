using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour {
		///////// Audio /////////
    public AudioClip clickAudio;
    public AudioClip hoverAudio;
    public AudioClip backClickAudio;
    public AudioClip acceptClickAudio;
    AudioSource audioSource;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Click() {
        audioSource.PlayOneShot(clickAudio);
    }
    public void BackClick() {
        audioSource.PlayOneShot(backClickAudio);
    }
    public void AcceptClick() {
        audioSource.PlayOneShot(acceptClickAudio);
    }
    public void MouseOn() {
        audioSource.PlayOneShot(hoverAudio);
    }
}
