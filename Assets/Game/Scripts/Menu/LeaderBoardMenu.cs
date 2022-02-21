using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderBoardMenu : MonoBehaviour {
	public GameObject PreviousMenu;
	[SerializeField] private Text leaderBoardRank;
	[SerializeField] private Text leaderBoardPoint;
	private int[] highScores;
	private string boardRank;
	private string boardPoint;

	void Start () {
		LoadHighScore();

		boardRank = "";
		boardPoint = "";

		int rank = 1;

		if (highScores.Length > 0 ) {
			for (int i = 0; i < highScores.Length; i++) {
				if (i == 8) {   //// Show Top 8 
					break;
				}
				else {
					if (highScores[i] > 0) {
						boardRank = boardRank + "Top " + rank.ToString() + "\n";
						boardPoint = boardPoint + highScores[i].ToString() + "\n";
						++rank;
					}
				}
			}
		}
		else {
			boardRank = "No Data";
		}
		leaderBoardRank.text = boardRank;
		leaderBoardPoint.text = boardPoint;
	}
	
	public void LoadHighScore() {
		string path = Application.persistentDataPath + "/highscore.bin";
		if(File.Exists(path)) {
				FileStream file = File.Open(path, FileMode.Open);
				BinaryFormatter formatter = new BinaryFormatter();
				LeaderBoardData highScoreData = (LeaderBoardData)formatter.Deserialize(file);
				file.Close();

				this.highScores = highScoreData.highScores;
				Debug.Log("Highscore Loaded from " + path);
		}
		else {
				highScores = new int[0];
		}
	}

	public void BackToPreviousMenu() {
		PreviousMenu.SetActive(true);
	}
}
