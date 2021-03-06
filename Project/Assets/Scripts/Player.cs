﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

public class Player : MovingObject
{
    // Player stats
    public int money = 0;

    // Tells player if it is in contact with something
    public bool touchingBuilding;
    public bool touchingObject;
    public bool touchingNPC;

    // Tells player if it is inside a building or a market
    public bool insideBuilding;
    public bool insideMarket;

    // The thing the player is in contact with
    private GameObject touching;



    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // Player is currently inside a market
        if (insideMarket == true)
        {
            Building building = touching.GetComponent<Building>();

            // Exit the market place
            if (Input.GetKeyDown(KeyCode.X))
                building.Exit();
            // Player is trying to buy something
            else if (Event.current.type == EventType.KeyDown)
            {

            }
            return;
        }

        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            touchingBuilding = touchingObject = touchingNPC = false;
            AttemptMove<Player>(-1, 0);
        }
        // Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            touchingBuilding = touchingObject = touchingNPC = false;
            AttemptMove<Player>(1, 0);
        }
        // Move up
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            touchingBuilding = touchingObject = touchingNPC = false;
            AttemptMove<Player>(0, 1);
        }
        // Move down
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            touchingBuilding = touchingObject = touchingNPC = false;
            AttemptMove<Player>(0, -1);
        }
        // Interaction
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (tryToInteract())
            {
                // Enter a building
                if (touchingBuilding)
                {
                    Building building = touching.GetComponent<Building>();
                    building.Enter();
                }
            }
        }
        // Hide textbox
        else if (Input.GetKeyDown(KeyCode.H))
        {
            textbox.Hide();
        }
    }

    //Sees if there is building or NPC in the surrounding tiles 
    protected virtual bool tryToInteract()
    {
        //Start is where the player is
        Vector2 start = transform.position;
        //Make vectors for the surrounding area.
        Vector2 left = start + new Vector2(-1, 0);
        Vector2 right = start + new Vector2(1, 0);
        Vector2 down = start + new Vector2(0, -1);
        Vector2 up = start + new Vector2(0, 1);

        //We don't want to have the player be considered something to interact with so
        //we turn this off temporarily
        boxCollider.enabled = false;

        RaycastHit2D leftHit = Physics2D.Linecast(start, left, blockingLayer);
        RaycastHit2D rightHit = Physics2D.Linecast(start, right, blockingLayer);
        RaycastHit2D downHit = Physics2D.Linecast(start, down, blockingLayer);
        RaycastHit2D upHit = Physics2D.Linecast(start, up, blockingLayer);
        Object target = null;

        //turn this back on since we have done all of our hit creation.
        boxCollider.enabled = true;

        //Return true if any of the surrounding area has an object you interact with it

        if (leftHit.transform != null)
        {
            print("Left hit");
            target = leftHit.collider.gameObject.GetComponent<Object>();
        }
        else if (rightHit.transform != null)
        {
            print("Right hit");
            target = rightHit.collider.gameObject.GetComponent<Object>();
        }
        else if (upHit.transform != null)
        {
            print("Up hit");
            target = upHit.collider.gameObject.GetComponent<Object>();
        }
        else if (downHit.transform != null)
        {
            print("down hit");
            target = downHit.collider.gameObject.GetComponent<Object>();
        }

        if (target != null)
        {
            target.Interact();
            return true;
        }

        return false;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // Reference to the previous map we were on
        int oldX = mapX;
        int oldY = mapY;

        // Try to move in the direction of the input
        base.AttemptMove<T>(xDir, yDir);

        // Draw the new tile we're on
        map.Undraw(oldX, oldY);
        map.Draw(mapX, mapY);

        // Redraw the textbox so it's on top
        textbox.RedrawNPC();
    }

    protected override void OnCantMove<T>(T component)
    {

    }

    // Another object entered a trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // We collided with a building
        if (other.tag == "Building")
        {
            print("Hit a building");
            touchingBuilding = true;
            touching = other.gameObject;
            Building building = touching.GetComponent<Building>();
            textbox.Write("Press z to enter " + building.name, null);
        }
        // We collided with an npc
        else if (other.tag == "NPC")
        {
            print("Walked into an NPC");
            touchingNPC = true;
            touching = other.gameObject;
        }
    }
}