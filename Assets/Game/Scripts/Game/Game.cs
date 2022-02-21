using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class Game : MonoBehaviour {
    [SerializeField] private int pointsPerRow = 15;
    [SerializeField] private int extraTilesMultiplier = 2;
    [SerializeField] private TileSpawner _spawner;
    [SerializeField] private GameLost lostPanel;
    [SerializeField] private PauseGame pausePanel;
    [SerializeField] private Text pointsText;
    private int points = 0;

    public TileSpawner spawner { get { return _spawner; } }

    private bool isPause = false;

    private int[] highScores;

    // sound
    AudioSource audioSource;
    public AudioClip finishMusic;
    public AudioClip bgMusic;

    void Start() {
        Debug.Log("Loaded scene Game");
        audioSource = GetComponent<AudioSource>();
        LoadHighScore();
        newGame();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPause) {
                ResumeGame();
            }
            else {
                PauseGame();
            }
        }
    }

    public void newGame() {
        spawner.randNewTiles();
        points = 0;
        pointsText.text = points.ToString();
        GameObject.FindGameObjectWithTag("Arena").GetComponent<Arena>().removeAllTiles();
        lostPanel.hide();
        pausePanel.hide();
        spawner.spawn();
        PlayLoopSound(bgMusic);
    }

    public void addPoints(int extraTiles = 0) {
        int pointsToAdd = 0;

        if (extraTiles == 0) pointsToAdd = pointsPerRow;
        else pointsToAdd = extraTiles * extraTilesMultiplier * pointsPerRow;

        Debug.Log(string.Format("Add {0} points with {1} extra tiles.", pointsToAdd, extraTiles));
        points += pointsToAdd;

        pointsText.text = points.ToString();
    }

    public void gameLost() {
        Debug.Log("Game Lost");
        pausePanel.hide();
        bool isHighscore = CheckHightscore();
        SaveHighScore();
        PlayLoopSound(finishMusic);
        lostPanel.display(points, isHighscore);
    }

    public void ResumeGame () {
        pausePanel.hide();
        isPause = false;
    }

    void PauseGame () {
        pausePanel.display();
        isPause = true;
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

    public void SaveHighScore() {
        string path = Application.persistentDataPath + "/highscore.bin";
        FileStream file = File.Create(path);
        
        // Leader board Data
        LeaderBoardData highScoreData = new LeaderBoardData(highScores);
        
        //Create binary formatter
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, highScoreData);
        file.Close();
        Debug.Log("Highscore Saved to " + path);
    }

    private bool CheckHightscore() {
        int[] newArr = new int[highScores.Length + 1];

        for (int i = 0; i < highScores.Length; i++) {
            newArr[i] = highScores[i];
        }

        newArr[highScores.Length] = points;

        Sort(newArr);

        highScores = newArr;

        if (newArr[0] == points) {
            return true;
        }
        else {
            return false;
        }
    }

    private static void Sort(int[] arr) 
    {
        int checkPoint = arr[arr.Length - 1];
        for (int i = 0; i < arr.Length - 1; i++) {
            if (checkPoint > arr[i]) {
                int temp = arr[i];
                arr[i] = checkPoint;
                checkPoint = temp;
            }
        }
        arr[arr.Length - 1] = checkPoint;
    }

    private void PlayLoopSound(AudioClip music) {
        audioSource.Stop ();
        audioSource.loop = true;
        audioSource.clip = music;
        audioSource.Play();
    }
}
