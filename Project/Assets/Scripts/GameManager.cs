using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject map;
    public MapManager mapManager;
    public int tileX, tileY;

	// Use this for initialization
	void Awake () {
        // Check if GameManager instance already exists
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        // Don't destroy when reloading scene
        DontDestroyOnLoad(gameObject);

        InitGame();
	}

    // 
    void Update() {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (tileX > 1) {
                mapManager.Undraw(tileX, tileY);
                mapManager.Draw(--tileX, tileY);
            }
        }
        // Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (tileX < 10) {
                mapManager.Undraw(tileX, tileY);
                mapManager.Draw(++tileX, tileY);
            }
        }
        // Move up
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (tileY < 10) {
                mapManager.Undraw(tileX, tileY);
                mapManager.Draw(tileX, ++tileY);
            }
        }
        // Move down
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (tileY > 1) {
                mapManager.Undraw(tileX, tileY);
                mapManager.Draw(tileX, --tileY);
            }
        }
    }

    // Initializes the game
    void InitGame() {
        // Create the map
        Instantiate(map);
        mapManager = map.GetComponent<MapManager>();
        mapManager.SetupScene();

        // Where are we on the map?
        tileX = Random.Range(0, 10);
        tileY = Random.Range(0, 10);

        // Draw our area on the map
        mapManager.Draw(tileX, tileY);
    }
}