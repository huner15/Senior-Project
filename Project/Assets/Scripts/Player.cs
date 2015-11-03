using UnityEngine;
using System.Collections;

public class Player : MovingObject {

    public int mapX, mapY;      // player's location on the map
    public int tileX, tileY;    // player's location on the tile

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	private void Update () {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        }
        // Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
        }
        // Move up
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
        }
        // Move down
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) (Input.GetAxisRaw("Horizontal"));
        vertical = (int) (Input.GetAxisRaw("Vertical"));

        // Check if moving horizontally, if so set vertical to zero
        if (horizontal != 0)
            vertical = 0;

        // Check if we have a non-zero value
        if (horizontal != 0 || vertical != 0) {
            // Did we hit a building?
            AttemptMove<Player>(horizontal, vertical);
        }
	}

    protected override void AttemptMove<T>(int xDir, int yDir) {
        base.AttemptMove<T>(xDir, yDir);

        // Allows us to reference the result of the Linecast done in Move
        RaycastHit2D hit;

        // If Move returns true, the player was able to move into an empty space
        if (Move(xDir, yDir, out hit))
        {
            
        }
    }

    protected override void OnCantMove<T>(T component) {
        //throw new System.NotImplementedException();
    }

    // Another object entered a trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other) {
        // We collided with an npc
        if (other.tag == "NPC") {

        }
        // We collided with a building
        else if (other.tag == "Building")
        {

        }
        // We collided with an exit
        else if (other.tag == "Exit")
        {
            Invoke("Exit", 1f);
        }
    }

    // Load the last scene loaded
    private void Exit() {
        Application.LoadLevel(Application.loadedLevel);
    }
}
