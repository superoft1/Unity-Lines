﻿using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Color[] possibleColors;
    private Tile[] tiles;
    private Arena arena;
    private Game game;

	void Awake () {
        tiles = GetComponentsInChildren<Tile>();
        arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<Arena>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) randNewTiles();
        if (Input.GetKeyDown(KeyCode.S)) spawn();
    }

    public void randNewTiles() {
        foreach(var til in tiles) {
            int rand = Random.Range(0, possibleColors.Length);
            til.color = possibleColors[rand];
        }
    }
	
	public void spawn() {
        List<ArenaTile> possibleTiles = arena.getEmptyTiles();

        if(possibleTiles.Count < 6) {
            game.gameLost();
        }
        else {
            foreach (var ti in tiles) {
                ArenaTile pos = possibleTiles[Random.Range(0, possibleTiles.Count)];
                possibleTiles.Remove(pos);
                Tile obj = Instantiate(tilePrefab, tileParent).GetComponent<Tile>();
                obj.initialize(pos, ti.color);
                Debug.Log("Spawned new tile.", pos);
            }

            randNewTiles();
            arena.checkPoints(true);
        }
    }
}
