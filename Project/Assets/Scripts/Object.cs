using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour {

    public GameObject prefab;       // prefab representing this object
    public MapManager map;
    public int mapX, mapY;          // object's location on the map
    public int tileX, tileY;        // object's location on the tile

    // Places the object at the given map location
    public void PlaceAt(int mX, int mY, int tX, int tY) {
        mapX = mX;
        mapY = mY;
        tileX = tX;
        tileY = tY;
    }
}