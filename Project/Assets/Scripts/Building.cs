using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public string name;
    public TileManager inside;

    public void Enter() {
        inside.Draw();
    }
}