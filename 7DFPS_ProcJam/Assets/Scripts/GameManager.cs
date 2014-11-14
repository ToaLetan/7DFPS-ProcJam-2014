using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    private SpaceGenerator spaceGen = null;
    private SpawnManager spawnManager = null;

    private int playerNumKills = 0;
    private int playerHull = 100;
    private int numEnemiesInScene = 0;
    private int prevNumEnemies = 0;

    public int PlayerNumKills
    {
        get { return playerNumKills; }
        set { playerNumKills = value; }
    }

    public int PlayerHull
    {
        get { return playerHull; }
        set { playerHull = value; }
    }

    public int NumEnemiesInScene
    {
        get { return numEnemiesInScene; }
        set { numEnemiesInScene = value; }
    }

	// Use this for initialization
	void Start () 
    {
        spaceGen = SpaceGenerator.Instance;
        spaceGen.SpaceContainer = GameObject.FindGameObjectWithTag("Space");

        spaceGen.GenerateSpace();

        spawnManager = SpawnManager.Instance;
        spawnManager.GameManager = this;
        spawnManager.EstablishSpawnPoints();
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateEnemyCount();

        if (spawnManager != null)
        {
            spawnManager.Update();
        }
	}

    private void UpdateEnemyCount()
    {
        numEnemiesInScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if(prevNumEnemies != numEnemiesInScene)
            prevNumEnemies = numEnemiesInScene;
    }
}
