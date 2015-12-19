using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

public enum timeOfDay
{
    morning, evening, night
}

public class NPC : MovingObject {
    // Sprite representing this NPC
    private npcSprite sprite;

    // States this npc currently has
    public Boolean hasQuest = false;
    public GameObject quest;
    private string name;
    private string personality;
    private List<string> states = new List<string>();

    // Places this npc goes
    private Building home, work;
    private Vector3 homeTile, workTile;

    // Movement stuff
    int ID; // npc's id on the map
    private float timeloc;
    private float time;
    private float movementSpeed;
    int timeOfDayLength = 1; // in minutes
    timeOfDay currentTime;



    // Use this for initialization
    public void Start()
    {
        time = Time.time;
        timeloc = time;
        currentTime = timeOfDay.morning;
        base.Start();
    }

    // Update is called once per frame
    public void Update() {
        // NPC goes to work
        if (currentTime == timeOfDay.morning && Time.time - timeloc > movementSpeed) {
            goTowards(workTile);
            timeloc = Time.time;
        }
        // NPC goes home
        if (currentTime == timeOfDay.evening && Time.time - timeloc > movementSpeed) {
            goTowards(homeTile);
            timeloc = Time.time;
        }
        // Michael what is this doing?
        if (Time.time - time > timeOfDayLength * 60) {
            time = Time.time;
            if (currentTime == timeOfDay.morning) {
                currentTime = timeOfDay.evening;
            }
            else if (currentTime == timeOfDay.evening) {
                currentTime = timeOfDay.night;
            }
            else {
                currentTime = timeOfDay.morning;
            }
        }
    }

    // Create the initial NPC
    public void init(Vector3 homeTile, Vector3 workTile, int ID)
    {
        // Decide where the npc works and lives
        this.homeTile = homeTile;
        this.workTile = workTile;
        this.ID = ID;

        // Create a random sprite and initialize it
        sprite = GetComponent<npcSprite>();
        sprite.init();

        // Create a random state and personality for the npc
        initState();
        initPersonality();

        // We're done with the sprite for now
        sprite.placeAt(new Vector3(tileX, tileY, 0));
        sprite.undraw();

        // How fast the NPC moves initially
        if (personality == "lazy")
            movementSpeed = Random.Range(1.5f, 2.0f);
        else
            movementSpeed = Random.Range(0.5f, 1.0f);
    }

    // Places the npc at the given location
    public void PlaceAt(int mX, int mY, int tX, int tY, int tZ)
    {
        Vector3 pos = new Vector3(tX, tY, tZ);
        mapX = mX;
        mapY = mY;
        tileX = tX;
        tileY = tY;
        transform.position = pos;

        if (hasQuest)
        {
            quest.GetComponent<Transform>().position = new Vector3(tX, tY + .8f, tZ);
        }
        if (sprite != null)
            sprite.placeAt(pos);
    }

    // Instantiate a quest
    public void initQuest()
    {
        hasQuest = true;
        quest = Instantiate(map.quest) as GameObject;

        if(sprite != null)
        {
            PlaceAt(mapX, mapY, tileX, tileY, 0);
            quest.SetActive(false);
        }
    }

    // Draws the NPC to the screen
    public void draw()
    {
        sprite.draw();

        if(hasQuest)
        {
            quest.SetActive(true);
        }
    }

    // Removes the npc from the screen
    public void undraw()
    {
        sprite.undraw();
        if(hasQuest)
        {
            quest.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {

    }

    protected override bool MoveToTile(int xDir, int yDir)
    {
        bool moved = base.MoveToTile(xDir, yDir);
        PlaceAt(mapX, mapY, tileX, tileY, 0);
        return moved;
    }



    // Decides what the NPC's initial state will be
    private void initState()
    {
        int mood = 0;
        string[] default_states = { "normal", "happy", "sad", "angry" };

        // 25% chance to be something other than neutral state on default
        if (Random.Range(0, 4) == 0)
            mood = Random.Range(0, 4);

        // update sprite to match state
        states.Add(default_states[mood]);
        sprite.setState(states[0]);
    }

    // Decides what the NPC's initial personality will be
    private void initPersonality()
    {
        // Initiate the personality
        int persona = Random.Range(0, 10);
        string[] personalities = { "helpful", "aggressive", "outgoing", "alcoholic", "greedy", "shy", "brave", "amoral", "lazy", "psychotic" };

        // update sprite to match personality
        personality = personalities[persona];
        sprite.setState(personality);
    }

    // NPC starts walking towards the given location
    private void goTowards(Vector3 tile) {
        int tMapX = (int)(tile.x / 10);
        int tMapY = (int)(tile.y / 10);
        int tTileX = (int)(tile.x % 10);
        int tTileY = (int)(tile.y % 10);

        int mx = tMapX - mapX;
        int my = tMapY - mapY;
        int tx = tTileX - tileX;
        int ty = tTileY - tileY;

        // Move up or down to the next map tile
        if (my != 0) {
            // Walk along the road
            if (tileX != 4 && tileX != 5) {
                int dx = Math.Min(4 - tileX, 5 - tileX);
                MoveToTile(dx / (Math.Abs(dx)), 0);
            }
            else {
                MoveToTile(0, my / (Math.Abs(my)));
            }
        }
        // Move left or right to the next map tile
        else if (mx != 0) {
            // Walk along the road
            if (tileY != 4 && tileY != 5) {
                int dy = Math.Min(4 - tileY, 5 - tileY);
                MoveToTile(0, dy / (Math.Abs(dy)));
            }
            else {
                MoveToTile(mx / (Math.Abs(mx)), 0);
            }
        }
        // Move up or down to the next tile
        else if (ty != 0) {
            MoveToTile(0, ty / (Math.Abs(ty)));
        }
        // Move left or right to the next tile
        else if (tx != 0) {
            MoveToTile(tx / (Math.Abs(tx)), 0);
        }
        // Reached the target destination
        else {
            
        }
    }
}