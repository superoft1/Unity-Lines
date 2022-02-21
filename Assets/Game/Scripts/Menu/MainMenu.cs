using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	///////// UI /////////
  public GameObject settingMenu;
	public GameObject leaderBoardMenu;


	public void PlayGame() {
		SceneManager.LoadScene("Game");
	}

	public void OpenLeaderBoard () {
		leaderBoardMenu.SetActive(true);
	}

	public void SettingsGame () {
		settingMenu.SetActive(true);
	}

	public void ExitGame () {
		Debug.Log("Exit Game");
		Application.Quit();
	}
	
}
