﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour {
    // Constants
    private int columns = 10;       // # of columns in a tile
    private int rows = 10;          // # of rows in a tile
    private int townSize = 7;       // size of the town
    
    // Map information
    private int numCave;            // # of caves on this map
    private int numMarket;          // # of markets in the town
    private int numFarm;            // # of farms on this map
    public TileManager tileManager;
    public TileManager[][] map = new TileManager[10][];

    // Prefab objects
    public GameObject caveInRoad, caveOutRoad, forestRoad, townRoad, marketRoad, farmRoad;
    public GameObject[] caveInOuterWall, caveOutOuterWall, forestOuterWall, townOuterWall, marketOuterWall, farmOuterWall;
    public GameObject[] caveInWalls, caveOutWalls, forestWalls, townWalls, marketWalls, farmWalls;
    public GameObject[] caveInFloors, caveOutFloors, forestFloors, townFloors, marketFloors, farmFloors;
    public GameObject[] caveInBuildings, caveOutBuildings, forestBuildings, townBuildings, marketBuildings, farmBuildings;

    // Grid information
    private Grid grid;
    public List<GameObject> buildings = new List<GameObject>();     // List of all buildings on all tiles
    public List<Vector3> buildingLocations = new List<Vector3>();   // List of all buildings locations (xy where x = map, y = tile)
    public List<GameObject> npcs = new List<GameObject>();          // List of all NPCs on all tiles
    public List<Vector3> npcLocations = new List<Vector3>();        // List of all NPC locations


    // Instantiates all of the tiles on the map
    private void initMap() {
        for (int x = 0; x < columns; x++) {
            map[x] = new TileManager[11];
            for (int y = 0; y < rows; y++) {
                map[x][y] = (TileManager)Instantiate(tileManager, new Vector3(5, 5, 0), Quaternion.identity);
            }
        }
    }

    // Determines where the town will be located
    List<Vector3> getTown() {
        List<Vector3> town = new List<Vector3>();
        int townX = Random.Range(3, 7);
        int townY = Random.Range(3, 7);

        // Locations that will be the town
        for (int x = townX - 3; x <= townX + 3; x++) {
            for (int y = townY - 3; y <= townY + 3; y++) {
                town.Add(new Vector3(x, y, 0f));
            }
        }
        return town;
    }

    // Assigns random map locations for certain map tiles
    List<Vector3> assignMapLocations(List<Vector3> mapLocs, int numTile) {
        // The list of where the map tiles are
        List<Vector3> tiles = new List<Vector3>();

        for (int i = 0; i < numTile; i++) {
            // Find a random location on the map
            int index = Random.Range(0, mapLocs.Count);
            Vector3 newLoc = mapLocs[index];
            mapLocs.RemoveAt(index);
            tiles.Add(newLoc);
        }
        return tiles;
    }

    // Sets up the map
    void MapSetup() {
        // Figure out where the town will be located
        List<Vector3> town = getTown();
        // Figure out where the markets will be located in the town
        List<Vector3> markets = assignMapLocations(town, numMarket);
        // Figure out where the caves will be located
        List<Vector3> caves = assignMapLocations(grid.gridPositions, numCave);
        // Figure out where the farms will be located
        List<Vector3> farms = assignMapLocations(grid.gridPositions, numFarm);

        // Layout the map and setup each map tile
        for (int x = 0; x < columns; x++) {
            for (int y = 0; y < rows; y++) {
                // Market
                if (grid.findTile(markets, x, y) != -1) {
                    map[x][y].SetupSprites(marketRoad, marketOuterWall, marketFloors, marketBuildings, marketWalls);
                    map[x][y].SetupScene(y, x, "Market");
                }
                // Town
                else if (grid.findTile(town, x, y) != -1) {
                    map[x][y].SetupSprites(townRoad, townOuterWall, townFloors, townBuildings, townWalls);
                    map[x][y].SetupScene(y, x, "Town");
                }
                // Cave Exterior
                else if (grid.findTile(caves, x, y) != -1) {
                    map[x][y].SetupSprites(caveOutRoad, caveOutOuterWall, caveOutFloors, caveOutBuildings, caveOutWalls);
                    map[x][y].SetupScene(y, x, "Cave");
                }
                // Farm
                else if (grid.findTile(farms, x, y) != -1) {
                    map[x][y].SetupSprites(farmRoad, farmOuterWall, farmFloors, farmBuildings, farmWalls);
                    map[x][y].SetupScene(y, x, "Farm");
                }
                // Forest
                else {
                    map[x][y].SetupSprites(forestRoad, forestOuterWall, forestFloors, forestBuildings, forestWalls);
                    map[x][y].SetupScene(y, x, "Forest");
                }
            }
        }
    }

    // Finds and stores all references to buildings and npcs on all tiles
    void GetReferences() {
        for (int x = 0; x < columns; x++) {
            for (int y = 0; y < rows; y++) {
                // Find all the buildings
                for (int i = 0; i < map[x][y].buildings.Count; i++) {
                    Vector3 loc = map[x][y].buildingLocations[i];
                    buildingLocations.Add(new Vector3(loc.x + 10 * x, loc.y + 10 * y, loc.z));
                    buildings.Add(map[x][y].buildings[i]);
                }
                // Find all the NPCs
                for (int i = 0; i < map[x][y].npcs.Count; i++) {
                    Vector3 loc = map[x][y].npcLocations[i];
                    npcLocations.Add(new Vector3(loc.x + 10 * x, loc.y + 10 * y, loc.z));
                    npcs.Add(map[x][y].npcs[i]);
                }
            }
        }
    }

    // Sets up the map
    public void SetupScene() {
        numCave = Random.Range(1, 3);       // 1 to 2 caves
        numMarket = Random.Range(3, 6);     // 3 to 5 markets
        numFarm = Random.Range(5, 8);       // 5 to 7 farms
        tileManager = GetComponent<TileManager>();

        grid = new Grid(columns, rows);
        initMap();
        MapSetup();
        GetReferences();
    }

    // Draws the tile at the given location to the screen
    public void Draw(int x, int y) {
        map[x][y].Draw();
    }

    // Removes the tile at the location from the screen
    public void Undraw(int x, int y) {
        map[x][y].Undraw();
    }
}