using UnityEngine;
using System.Collections;

public class NPC : MovingObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected override void OnCantMove<T>(T component)
    {
        //throw new System.NotImplementedException();
    }
}
