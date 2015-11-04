using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Player : MovingObject {

	// Use this for initialization
	void Start () {
        base.Start();
    }
	
	// Update is called once per frame
    void Update () {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            AttemptMove<Player>(-1, 0);
        // Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            AttemptMove<Player>(1, 0);
        // Move up
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            AttemptMove<Player>(0, 1);
        // Move down
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            AttemptMove<Player>(0, -1);
	}

    protected override void AttemptMove<T>(int xDir, int yDir) {
        // Reference to the previous map we were on
        int oldX = mapX;
        int oldY = mapY;

        // Try to move in the direction of the input
        base.AttemptMove<T>(xDir, yDir);

        print("map " + mapX + ", " + mapY);
        print("tile " + tileX + ", " + tileY);

        // Draw the new tile we're on
        map.Undraw(oldX, oldY);
        map.Draw(mapX, mapY);
    }

    protected override void OnCantMove<T>(T component) {
        //throw new System.NotImplementedException();
    }

    // Another object entered a trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other) {
        // We collided with an npc
        if (other.tag == "NPC") {
            print("Walked into an NPC");
        }
        // We collided with a building
        else if (other.tag == "Building") {
            print("Hit a building");
        }
    }
}
