using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

public class NPC : MovingObject
{
    // Sprite representing this NPC
    private npcSprite sprite;

    // States this npc currently has
    private string name;
    private string personality;
    private List<string> states = new List<string>();

    // Places this npc goes
    private Building home;
    private Building work;

    private Vector3 homeTile, workTile;
    private float timeloc;
    private float time;
    int timeOfDayLength = 1; //in minutes
    String ID;
    enum timeOfDay
    {
        morning, evening, night
    }
    timeOfDay currentTime;



    // Use this for initialization
    void Start()
    {
        time = Time.time;
        timeloc = time;
        currentTime = timeOfDay.morning;
        mapX = (int)homeTile.x;
        mapY = (int)homeTile.y;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //print("time " + (time - Time.time));
        if (currentTime == timeOfDay.morning && Time.time - timeloc > 10)
        {
            goTowardsWork();
            timeloc = Time.time;
        }
        if (currentTime == timeOfDay.evening && Time.time - timeloc > 10)
        {
            goTowardsHome();
            timeloc = Time.time;
        }
        if (Time.time - time > timeOfDayLength * 60)
        {
            time = Time.time;
            if (currentTime == timeOfDay.morning)
            {
                currentTime = timeOfDay.evening;
            }
            else if (currentTime == timeOfDay.evening)
            {
                currentTime = timeOfDay.night;
            }
            else
            {
                currentTime = timeOfDay.morning;
            }
        }
    }

    // Create the initial NPC
    public void init()
    {
        // Create a random sprite and initialize it
        sprite = GetComponent<npcSprite>();
        sprite.init();

        // Create a random state for the npc
        initState();
        initPersonality();

        // We're done with the sprite for now
        sprite.undraw();
    }

    public void setUp(Vector3 homeTile, Vector3 workTile, string ID)
    {
        this.homeTile = homeTile;
        this.workTile = workTile;
        this.ID = ID;
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
        sprite.placeAt(pos);
    }

    // Draws the NPC to the screen
    public void draw()
    {
        sprite.draw();
    }

    // Removes the npc from the screen
    public void undraw()
    {
        sprite.undraw();
    }

    protected override void OnCantMove<T>(T component)
    {

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

    private void goTowardsHome()
    {
        int dx = (int)homeTile.x - mapX;
        int dy = (int)homeTile.y - mapY;
        mapX += dx;
        mapY += dy;
        if (dx != 0 || dy != 0)
            GameManager.logger(ID + "Entered Tile " + mapX + " " + mapY + " While Going Home at " + Time.time + "\n");
        // AttemptMove<NPC>(dx, dy);
    }

    private void goTowardsWork()
    {
        int dx = (int)workTile.x - mapX;
        int dy = (int)workTile.y - mapY;
        if (dx != 0)
            dx = dx / (Math.Abs(dx));
        if (dy != 0)
            dy = dy / (Math.Abs(dy));
        mapX += dx;
        mapY += dy;
        if (dx != 0 || dy != 0)
            GameManager.logger(ID + "Entered Tile " + mapX + " " + mapY + " While Going To Work at " + Time.time + "\n");
        //AttemptMove<NPC>(dx, dy);
    }
}
