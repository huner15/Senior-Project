using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour {
    // Constants
    private int columns = 10;       // # of columns in a tile
    private int rows = 10;          // # of rows in a tile
    private int townSize = 7;       // size of the town
    
    // Map information
    public int numCave;             // # of caves on this map
    public int numMarket;           // # of markets in the town
    public TileManager tileManager;
    private TileManager[][] map = new TileManager[11][];

    // Prefab objects
    public GameObject caveInExit, caveOutExit, forestExit, townExit, marketExit;
    public GameObject caveInRoad, caveOutRoad, forestRoad, townRoad, marketRoad;
    public GameObject[] caveInOuterWall, caveOutOuterWall, forestOuterWall, townOuterWall, marketOuterWall;
    public GameObject[] caveInWalls, caveOutWalls, forestWalls, townWalls, marketWalls;
    public GameObject[] caveInFloors, caveOutFloors, forestFloors, townFloors, marketFloors;
    public GameObject[] caveInBuildings, caveOutBuildings, forestBuildings, townBuildings, marketBuildings;
    public GameObject[] npcs;

    // Grid information
    private Transform mapHolder;                                // Reference to the transform of the map
    private List<Vector3> gridPositions = new List<Vector3>();  // Possible locations to place tiles


    // Finds the index of the tile with given coordinates in the grid
    int findTile(List<Vector3> grid, float x, float y) {
        for (int i = 0; i < grid.Count; i++) {
            if (grid[i].x == x && grid[i].y == y)
                return i;
        }
        return -1;
    }

    // Removes the tile from the list of possible grid locations
    void removeTile(float x, float y) {
        int find = findTile(gridPositions, x, y);
        if (find != -1)
            gridPositions.RemoveAt(find);
    }

    // Clears the list of gridPositions and prepares it to generate a new tile
    void InitialiseList() {
        gridPositions.Clear();

        for (int x = 1; x <= columns; x++) {
            map[x] = new TileManager[11];
            for (int y = 1; y <= rows; y++) {
                map[x][y] = (TileManager)Instantiate(tileManager, new Vector3(5, 5, 0), Quaternion.identity);
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Determines where the town will be located
    List<Vector3> getTown() {
        List<Vector3> town = new List<Vector3>();
        int townX = Random.Range(4, 8);
        int townY = Random.Range(4, 8);

        // Locations that will be the town
        for (int x = townX - 3; x <= townX + 3; x++) {
            for (int y = townY - 3; y <= townY + 3; y++)
                town.Add(new Vector3(x, y, 0f));
        }
        return town;
    }

    // Sets up the map
    void MapSetup() {
        // Figure out where the town will be located
        List<Vector3> town = getTown();

        // Figure out where the markets will be located in the town
        List<Vector3> markets = new List<Vector3>();
        for (int i = 0; i < numMarket; i++) {
            Vector3 market = town[Random.Range(0, town.Count)];
            // Tile is already a market
            while (findTile(markets, market.x, market.y) != -1) {
                market = town[Random.Range(0, town.Count)];
            }
            markets.Add(market);
        }

        // Figure out where the caves will be located
        List<Vector3> caves = new List<Vector3>();
        for (int i = 0; i < numCave; i++) {
            Vector3 cave = gridPositions[Random.Range(0, gridPositions.Count)];
            // Tile is already a town or a cave
            while (findTile(town, cave.x, cave.y) != -1 || findTile(caves, cave.x, cave.y) != -1) {
                cave = gridPositions[Random.Range(0, gridPositions.Count)];
            }
            caves.Add(cave);
        }

        // Layout the map and setup each map tile
        for (int x = 1; x <= columns; x++) {
            for (int y = 1; y <= rows; y++) {
                // Market
                if (findTile(markets, x, y) != -1) {
                    map[x][y].SetupSprites(marketExit, marketRoad, marketOuterWall, marketFloors, marketBuildings, marketWalls, npcs);
                    map[x][y].SetupScene(y, x, "Market");
                }
                // Town
                else if (findTile(town, x, y) != -1) {
                    map[x][y].SetupSprites(townExit, townRoad, townOuterWall, townFloors, townBuildings, townWalls, npcs);
                    map[x][y].SetupScene(y, x, "Town");
                }
                // Cave Exterior
                else if (findTile(caves, x, y) != -1) {
                    map[x][y].SetupSprites(caveOutExit, caveOutRoad, caveOutOuterWall, caveOutFloors, caveOutBuildings, caveOutWalls, npcs);
                    map[x][y].SetupScene(y, x, "Cave");
                }
                // Forest
                else {
                    map[x][y].SetupSprites(forestExit, forestRoad, forestOuterWall, forestFloors, forestBuildings, forestWalls, npcs);
                    map[x][y].SetupScene(y, x, "Forest");
                }
            }
        }
    }

    // Sets up the map
    public void SetupScene() {
        numCave = Random.Range(1, 3);
        numMarket = Random.Range(3, 8);
        tileManager = GetComponent<TileManager>();

        InitialiseList();
        MapSetup();
    }

    // Draws the tile at the given location to the screen
    public void Draw(int x, int y) {
        map[x][y].Draw();
        print("On tile (" + x + ", " + y + ")");
    }

    // Removes the tile at the location from the screen
    public void Undraw(int x, int y) {
        map[x][y].Undraw();
    }
}