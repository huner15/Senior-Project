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

    static System.IO.StreamWriter writer;

    public static void logger(String str)
    {
        if (writer == null)
        {
            if (!System.IO.File.Exists("log.txt"))
            {
                System.IO.File.Create("log.txt");
            }

            writer = new System.IO.StreamWriter(System.IO.File.OpenWrite("log.txt"));
        }
        print(str);
        writer.WriteLine(str);
        writer.Flush();

    }

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


    void Update()
    {
        for (int i = 0; i < mapManager.activeNPCs.Length; i++)
        {
            NPC npc = mapManager.activeNPCs[i].GetComponent<NPC>();

            // Place the NPC so that it can be drawn on the current map tile
            if (npc.mapX == playerManager.mapX && npc.mapY == playerManager.mapY)
            {
                if (npc.placed == false)
                {
                    Vector3 tileLoc = mapManager.map[npc.mapX][npc.mapY].EmptyLocation();
                    npc.PlaceAt((int)npc.mapX, (int)npc.mapY, (int)tileLoc.x, (int)tileLoc.y, 0);
                    mapManager.tempLocations.Add(tileLoc);
                    npc.placed = true;
                }
                
            }
            // Place the NPC off screen if not on current map tile
            else
            {
                npc.placed = false;
                npc.PlaceAt((int)npc.mapX, (int)npc.mapY, 11, 11, 0);

            }
        }
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

        // Create the player
        player = Instantiate(player, new Vector3(tile.x, tile.y, 10f), Quaternion.identity) as GameObject;
        playerManager = player.GetComponent<Player>();
        playerManager.PlaceAt(mapX, mapY, (int)tile.x, (int)tile.y, (int)(10 - tile.y));
        playerManager.map = mapManager;

        // Draw our area on the map
        mapManager.Draw(playerManager.mapX, playerManager.mapY);
    }
}