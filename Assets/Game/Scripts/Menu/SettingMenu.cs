using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour {
	///////// UI ////////
	public GameObject PreviousMenu;
	public Slider MusicSlider;
	public Slider SFXSlider;

	///////// Audio ////////
	public AudioMixer audioMixer;
	float originalMusicVolume;
	float musicVolume;
	float originalSFXVolume;
	float sfxVolume;

	void Start () {
		float audioValue = 0f;
		bool isGettingValue = false;

		//Get Music Volume;
		isGettingValue = audioMixer.GetFloat("MusicVolume", out audioValue);
		if (isGettingValue) 
		{
				originalMusicVolume = audioValue;
		}
		else 
		{
				originalMusicVolume = 0f;
		}
		MusicSlider.value = originalMusicVolume;
		

		//Get SFX Volume;
		isGettingValue = audioMixer.GetFloat("SFXVolume", out audioValue);
		if (isGettingValue) 
		{
				originalSFXVolume = audioValue;
		}
		else 
		{
				originalSFXVolume = 0f;
		}
		SFXSlider.value = originalSFXVolume;
	}

	public void ChangeMusicVolume(float volume) 
	{
		audioMixer.SetFloat("MusicVolume", volume);
		musicVolume = volume;
	}

	public void ChangeSFXVolume(float volume) 
	{
		audioMixer.SetFloat("SFXVolume", volume);
		sfxVolume = volume;
	}

	public void BackToPreviousMenu() {
		///Reset volume
		audioMixer.SetFloat("MusicVolume", originalMusicVolume);
		audioMixer.SetFloat("SFXVolume", originalSFXVolume);

		///Reset value
		musicVolume = originalMusicVolume;
		MusicSlider.value = originalMusicVolume;

		sfxVolume = originalSFXVolume;
		SFXSlider.value = originalSFXVolume;

		///Back to previous menu
		PreviousMenu.SetActive(true);
	}

	public void ApplySetting() {
		//// Save Volume
		originalMusicVolume = musicVolume;
		originalSFXVolume = sfxVolume;
	}

}
