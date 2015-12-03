using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class NPC : MovingObject {
    // Sprite representing this NPC
    private npcSprite sprite;

    // States this npc currently has
    private string name;
    private string personality;
    private List<string> states = new List<string>();

    // Places this npc goes
    private Building home;
    private Building work;


    // Decides what the NPC's initial state will be
    private void initState() {
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
    private void initPersonality() {
        // Initiate the personality
        int persona = Random.Range(0, 10);
        string[] personalities = { "helpful", "aggressive", "outgoing", "alcoholic", "greedy", "shy", "brave", "amoral", "lazy", "psychotic" };

        // update sprite to match personality
        personality = personalities[persona];
        sprite.setState(personality);
    }


    // Create the initial NPC
    public void init() {
        // Create a random sprite and initialize it
        sprite = GetComponent<npcSprite>();
        sprite.init();

        // Create a random state for the npc
        initState();
        initPersonality();

        // We're done with the sprite for now
        sprite.undraw();
    }

    // Draws the NPC to the screen
    public void draw() {
        sprite.draw();
    }

    // Removes the npc from the screen
    public void undraw() {
        sprite.undraw();
    }


    // Use this for initialization
    void Start() {
        base.Start();
    }

    // Places the npc at the given location
    public void PlaceAt(int mX, int mY, int tX, int tY, int tZ) {
        Vector3 pos = new Vector3(tX, tY, tZ);
        mapX = mX;
        mapY = mY;
        tileX = tX;
        tileY = tY;
        sprite.placeAt(pos);
    }

    protected override void OnCantMove<T>(T component) {
    }
}
