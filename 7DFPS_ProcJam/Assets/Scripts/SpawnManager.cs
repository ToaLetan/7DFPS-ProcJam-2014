using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager
{
    private const float BASE_SPAWN_TIME = 1.5f;
    private const float MIN_SPAWN_TIME = 0.25f;

    private int MAX_NUM_ENEMIES = 117;

    private Timer spawnTimer = null;

    private List<GameObject> spawnPoints = new List<GameObject>();
    private List<GameObject> numEnemiesInScene = new List<GameObject>();

    private GameManager gameManager = null;

    private float spawnTime = 0.0f; //Time between individual spawns.

    private int numEnemiesToSpawn = 0;
    private int currentWave = 0;

    private bool isSpawningWave = false;

    private static SpawnManager instance = null;

    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SpawnManager();
            return instance;
        }
    }

    public bool IsSpawningWave
    {
        get { return isSpawningWave; }
    }

    public GameManager GameManager
    {
        set { gameManager = value; }
    }

	// Use this for initialization
	private SpawnManager()
    {
        spawnTimer = new Timer(spawnTime);

        spawnTimer.OnTimerComplete += SpawnNewEnemy;
    }
	
	// Update is called once per frame
	public void Update () 
    {
        if (gameManager.IsGamePaused == false)
        {
            if (spawnTimer != null)
                spawnTimer.Update();

            if (gameManager.NumEnemiesInScene == 0 && isSpawningWave == false)
                SpawnEnemyWave();
        }
	}

    public void EstablishSpawnPoints()
    {
        if (spawnPoints.Count > 0)
            spawnPoints.Clear();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("SpawnPoint").Length; i++)
        {
            if (GameObject.FindGameObjectsWithTag("SpawnPoint")[i] != null)
            {
                spawnPoints.Add(GameObject.FindGameObjectsWithTag("SpawnPoint")[i]);
            }
        }
    }

    private void CalculateSpawnValues() //Determines the time until next wave, how many enemies to spawn based on player stats.
    {
        numEnemiesToSpawn = currentWave * Random.Range(currentWave, currentWave * 5);

        if (numEnemiesToSpawn > MAX_NUM_ENEMIES)
            numEnemiesToSpawn = MAX_NUM_ENEMIES;

        spawnTime = BASE_SPAWN_TIME - (0.1f * currentWave);

        if (spawnTime < MIN_SPAWN_TIME)
            spawnTime = MIN_SPAWN_TIME;

        spawnTimer.TargetTime = spawnTime;
    }

    public void SpawnEnemyWave()
    {
        currentWave++;

        CalculateSpawnValues();

        spawnTimer.StartTimer();

        isSpawningWave = true;

        Debug.Log("NEW WAVE SPAWN: " + currentWave);
    }

    private void SpawnNewEnemy()
    {
        bool resetTimerAfterSpawn = false;

        numEnemiesToSpawn--;

        GameObject newEnemy = GameObject.Instantiate(Resources.Load("Prefabs/Enemy_Standard")) as GameObject;

        //Randomly select a warp gate to spawn from.
        GameObject spawnGate = spawnPoints[Random.Range(0, spawnPoints.Count)];

        newEnemy.transform.position = spawnGate.transform.position;

        //Spawn an animation to go along with the enemy.
        GameObject enemySpawnAnim = GameObject.Instantiate(Resources.Load("Prefabs/Enemy_Spawn_Anim")) as GameObject;
        enemySpawnAnim.transform.position = newEnemy.transform.position;

        if (numEnemiesToSpawn > 0)
            resetTimerAfterSpawn = true;
        else
        {
            resetTimerAfterSpawn = false;
            isSpawningWave = false;
        }

        spawnTimer.ResetTimer(resetTimerAfterSpawn);
    }

}
