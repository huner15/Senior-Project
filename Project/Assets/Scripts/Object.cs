using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour {

    public int mapX, mapY;          // object's location on the map
    public int tileX, tileY;        // object's location on the tile
	public bool hasQuest = false;		//if an object has a quest

    // Places the object at the given map location
    public void PlaceAt(int mX, int mY, int tX, int tY, int tZ) {
        mapX = mX;
        mapY = mY;
        tileX = tX;
        tileY = tY;
        transform.position = new Vector3(tX, tY, tZ);
		if(hasQuest)
		{
			GameObject quest = Instantiate(MapManager.quest) as GameObject;
			quest.GetComponent<Transform>().position = new Vector3(tX, tY + .8f, tZ);
		}
    }
}