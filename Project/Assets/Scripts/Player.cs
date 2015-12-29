using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Player : MovingObject {
    // Tells player if it is in contact with something
    bool touchingBuilding;
    bool touchingObject;
    bool touchingNPC;

	// Use this for initialization
	void Start () {
        base.Start();
    }
	
	// Update is called once per frame
    void Update () {
        // Move left
        if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			touchingBuilding = touchingObject = touchingNPC = false;
			AttemptMove<Player> (-1, 0);
		}
        // Move right
        else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			touchingBuilding = touchingObject = touchingNPC = false;
			AttemptMove<Player> (1, 0);
		}
        // Move up
        else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			touchingBuilding = touchingObject = touchingNPC = false;
			AttemptMove<Player> (0, 1);
		}
        // Move down
        else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			touchingBuilding = touchingObject = touchingNPC = false;
			AttemptMove<Player> (0, -1);
		}
		//Interaction
		else if (Input.GetKeyDown (KeyCode.Z)) {
			if (tryToInteract()){
				print("Successful Interaction");
			}else{
				print("Unsuccessful Interaction");
			}
		}
	}

	//Sees if there is building or NPC in the surrounding tiles 
	protected virtual bool tryToInteract (){
		//Start is where the player is
		Vector2 start = transform.position; 
		//Make vectors for the surrounding area.
		Vector2 left = start + new Vector2 (-1, 0);
		Vector2 right = start + new Vector2 (1, 0);
		Vector2 down = start + new Vector2 (0, -1);
		Vector2 up = start + new Vector2 (0, 1);

		//We don't want to have the player be considered something to interact with so
		//we turn this off temporarily
		boxCollider.enabled = false;

		RaycastHit2D leftHit = Physics2D.Linecast (start, left, blockingLayer);
		RaycastHit2D rightHit = Physics2D.Linecast (start, right, blockingLayer);
		RaycastHit2D downHit = Physics2D.Linecast (start, down, blockingLayer);
		RaycastHit2D upHit = Physics2D.Linecast (start, up, blockingLayer); 

		//turn this back on since we have done all of our hit creation.
		boxCollider.enabled = true;

		//Return true if any of the surrounding area has an object you interact with it
		if (leftHit != null || rightHit != null || downHit != null || upHit != null) {
			return true;
		}

		return false;
	}

    protected override void AttemptMove<T>(int xDir, int yDir) {
        // Reference to the previous map we were on
        int oldX = mapX;
        int oldY = mapY;

        // Try to move in the direction of the input
        base.AttemptMove<T>(xDir, yDir);


        // Draw the new tile we're on
        map.Undraw(oldX, oldY);
        map.Draw(mapX, mapY);
    }

    protected override void OnCantMove<T>(T component) {
        //throw new System.NotImplementedException();
    }

    // Another object entered a trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other) {
        // We collided with a building
        if (other.tag == "Building") {
            print("Hit a building");
            touchingBuilding = true;
        }
        // We collided with an npc
        else if (other.tag == "NPC") {
            print("Walked into an NPC");
            touchingNPC = true;
        }
    }
}
