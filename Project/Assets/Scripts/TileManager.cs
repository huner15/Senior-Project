using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

// Class that holds a min and max for a count
public class Count {
    public int minimum;
    public int maximum;

    public Count(int min, int max) {
        minimum = min;
        maximum = max;
    }
}

public class TileManager : MonoBehaviour {
    // Constants
    private int columns = 10;       // # of columns in a tile
    private int rows = 10;          // # of rows in a tile
    
    // Tile information
    public String tileType;         // the type of tile this is
    public int tileRow, tileCol;    // location of this tile on the map
    private int bSizeX, bSizeY;     // size of buildings on this tile
    private Count buildingCount;    // # of buildings that can spawn on this tile
    private Count wallCount;        // # of walls that can spawn on this tile
    private Count npcCount;         // # of people that can spawn on this tile

    // Prefab objects
    private GameObject exitTile;         // prefab for the exit tile
    private GameObject roadTile;         // prefab for the road tile
    private GameObject[] outerWallTiles; // prefab for the outer-wall tile
    private GameObject[] floorTiles;     // Array of floor prefabs
    private GameObject[] buildingTiles;  // Array of building prefabs
    private GameObject[] wallTiles;      // Array of wall prefabs
    private GameObject[] npcTiles;       // Array of npc prefabs

    // Grid information
    private Transform boardHolder;                              // Reference to the transform of the tile
    private List<Vector3> gridPositions = new List<Vector3>();  // Possible locations to place tiles

    // All objects on this tile
    public List<GameObject> floors = new List<GameObject>();        // prefabs for the floor
    public List<Vector3> floorLocations = new List<Vector3>();      // location of floors
    public List<GameObject> buildings = new List<GameObject>();     // prefabs for the buildings
    public List<Vector3> buildingLocations = new List<Vector3>();   // location of buildings
    public List<GameObject> walls = new List<GameObject>();         // prefabs for the walls
    public List<Vector3> wallLocations = new List<Vector3>();       // location of walls
    public List<GameObject> npcs = new List<GameObject>();          // prefabs for the npcs
    public List<Vector3> npcLocations = new List<Vector3>();        // location of npcs


    // Finds the index of the tile with given coordinates in the grid
    int findTile(float x, float y) {
        for (int i = 0; i < gridPositions.Count; i++) {
            if (gridPositions[i].x == x && gridPositions[i].y == y)
                return i;
        }
        return -1;
    }

    // Removes the tile from the list of possible grid locations
    void removeTile(float x, float y) {
        int find = findTile(x, y);
        if (find != -1)
            gridPositions.RemoveAt(find);
    }

    // Remove all tiles that overlap with the object
    void removeAdjacent(float locX, float locY, int sizeX, int sizeY) {
        for (int x = 1; x < sizeX; x++) {
            for (int y = 1; y < sizeY; y++) {
                removeTile(locX + x, locY);
                removeTile(locX - x, locY);
                removeTile(locX, locY + y);
                removeTile(locX, locY - y);
                removeTile(locX + x, locY + y);
                removeTile(locX + x, locY - y);
                removeTile(locX - x, locY + y);
                removeTile(locX - x, locY - y);
            }
        }
    }

    // Clears the list of gridPositions and prepares it to generate a new tile
    void InitialiseList() {
        gridPositions.Clear();

        for (int x = 1; x <= columns; x++) {
            for (int y = 1; y <= rows; y++)
                gridPositions.Add(new Vector3(x, y, 0f));
        }
    }

    // Sets up the walls, exits and floor of the tile
    void BoardSetup() {
        boardHolder = new GameObject("Board").transform;

        for (int x = 1; x <= columns; x++) {
            for (int y = 1; y <= rows; y++) {
                // Create a random floor tile
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                // Create a floor tile that can go to the next map
                //if (x == 1 || x == columns || y == 1 || y == rows)
                //    toInstantiate = exitTile;

                // Edge of the map; create an impassable wall
                if ((x == 1 && tileCol == 1) || (x == columns && tileCol == columns) || (y == 1 && tileRow == 1) || (y == rows && tileRow == rows)) {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    removeTile(x, y);
                }

                // Add the tile to the game
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                instance.SetActive(false);
                floors.Add(instance);
                floorLocations.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Returns a random position from the grid where the object can be placed
    Vector3 RandomPosition(int sizeX, int sizeY) {
        int index = Random.Range(0, gridPositions.Count);
        Vector3 pos = gridPositions[index];

        // Object goes off the edge of the tile grid
        while (pos.x - sizeX + 1 < 1 || pos.x + sizeX - 1 > columns || pos.y - sizeY + 1 < 1 || pos.y + sizeY - 1 > rows ||
               findTile(pos.x - sizeX + 1, pos.y) == -1 || findTile(pos.x + sizeX - 1, pos.y) == -1 ||
               findTile(pos.x, pos.y + sizeY - 1) == -1 || findTile(pos.x, pos.y - sizeY + 1) == -1) {
            index = Random.Range(0, gridPositions.Count);
            pos = gridPositions[index];
        }
        removeTile(pos.x, pos.y);
        removeAdjacent(pos.x, pos.y, sizeX, sizeY);

        return pos;
    }

    // Takes in an array of game objects and randomly places them on the map
    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max, int sizeX, int sizeY, List<GameObject> objs, List<Vector3> pos) {
        int objectCount = Random.Range(min, max + 1);

        // Nothing can be placed on the current tile
        if (gridPositions.Count == 0)
            return;

        for (int i = 0; i < objectCount; i++) {
            // Where and what to place
            Vector3 randomPosition = RandomPosition(sizeX, sizeY);
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            randomPosition.z = randomPosition.y;

            // Add the object
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
            instance.SetActive(false);
            objs.Add(instance);
            pos.Add(randomPosition);
        }
    }

    // Tells the tile what to use as sprites
    public void SetupSprites(GameObject exit, GameObject road, GameObject[] outerwall, GameObject[] floor, GameObject[] building, GameObject[] wall, GameObject[] npc) {
        exitTile = exit;
        roadTile = road;
        outerWallTiles = outerwall;
        floorTiles = floor;
        buildingTiles = building;
        wallTiles = wall;
        npcTiles = npc;
    }

    // Initializes and lays out the tile
    public void SetupScene(int tRow, int tCol, String type) {
        // Instantiate the tile
        tileType = type;            // what type of tile is this?
        tileRow = tRow;             // where is the tile on the map?
        tileCol = tCol;

        // Tile is a market; has lots of stalls and people
        if (tileType.Equals("Market")) {
            buildingCount = new Count(3, 7);
            wallCount = new Count(0, 0);
            npcCount = new Count(10, 30);
            bSizeX = 2;
            bSizeY = 1;
        }
        // Tile is a town; has houses and people
        else if (tileType.Equals("Town")) {
            buildingCount = new Count(1, 3);
            wallCount = new Count(0, 10);
            npcCount = new Count(5, 10);
            bSizeX = 2;
            bSizeY = 2;
        }
        // Tile is a forest; has trees and bushes
        else if (tileType.Equals("Forest")) {
            buildingCount = new Count(0, 0);
            wallCount = new Count(10, 30);
            npcCount = new Count(0, 1);
            bSizeX = 1;
            bSizeY = 1;
        }
        // Tile is a cave entrance; has rocks and caves
        else if (tileType.Equals("Cave")) {
            buildingCount = new Count(1, 1);
            wallCount = new Count(5, 10);
            npcCount = new Count(0, 1);
            bSizeX = 2;
            bSizeY = 1;
        }
        
        // Resets the list of grid positions
        InitialiseList();
        // Creates the outer walls, floor, and exit
        BoardSetup();

        // Instantiates and places a random number of buildings
        LayoutObjectAtRandom(buildingTiles, buildingCount.minimum, buildingCount.maximum, bSizeX, bSizeY, buildings, buildingLocations);
        // Instantiates and places a random number of walls
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum, 1, 1, walls, wallLocations);
        // Instantiates and places a random number of people
        LayoutObjectAtRandom(npcTiles, npcCount.minimum, npcCount.maximum, 1, 1, npcs, npcLocations);
    }

    // Draw this tile to the screen
    public void Draw() {
        // Draw the floor
        for (int i = 0; i < floors.Count; i++)
            floors[i].SetActive(true);
        // Draw the walls
        for (int i = 0; i < walls.Count; i++)
            walls[i].SetActive(true);
        // Draw the buildings
        for (int i = 0; i < buildings.Count; i++)
            buildings[i].SetActive(true);
        // Draw the people
        for (int i = 0; i < npcs.Count; i++)
            npcs[i].SetActive(true);
    }

    // Removes the tile from the screen
    public void Undraw() {
        // Undraw the floor
        for (int i = 0; i < floors.Count; i++)
            floors[i].SetActive(false);
        // Undraw the walls
        for (int i = 0; i < walls.Count; i++)
            walls[i].SetActive(false);
        // Undraw the buildings
        for (int i = 0; i < buildings.Count; i++)
            buildings[i].SetActive(false);
        // Undraw the people
        for (int i = 0; i < npcs.Count; i++)
            npcs[i].SetActive(false);
    }
}