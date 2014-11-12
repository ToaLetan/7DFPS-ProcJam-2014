using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private SpaceGenerator spaceGen = null;

	// Use this for initialization
	void Start () 
    {
        spaceGen = SpaceGenerator.Instance;
        spaceGen.SpaceContainer = GameObject.FindGameObjectWithTag("Space");

        spaceGen.GenerateSpace();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
