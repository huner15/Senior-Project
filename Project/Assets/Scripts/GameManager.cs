using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject player;
    public GameObject map;

    private MapManager mapManager;
    private Player playerManager;

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

    // Initializes the game
    void InitGame() {
        // Create the map
        map = Instantiate(map, new Vector3(5.5f, 5.5f, 0), Quaternion.identity) as GameObject;
        mapManager = map.GetComponent<MapManager>();
        mapManager.SetupScene();

        // Where are we on the map?
        int mapX = Random.Range(0, 10);
        int mapY = Random.Range(0, 10);
        Vector3 tile = mapManager.map[mapX][mapY].EmptyLocation();
        while (tile.x == -1)
            tile = mapManager.map[mapX][mapY].EmptyLocation();

        print("map " + mapX + ", " + mapY);
        print("tile " + tile.x + ", " + tile.y);

        // Create the player
        player = Instantiate(player, new Vector3(tile.x, tile.y, 10f), Quaternion.identity) as GameObject;
        playerManager = player.GetComponent<Player>();
        playerManager.PlaceAt(mapX, mapY, (int)tile.x, (int)tile.y);
        playerManager.map = mapManager;

        // Draw our area on the map
        mapManager.Draw(playerManager.mapX, playerManager.mapY);
    }
}