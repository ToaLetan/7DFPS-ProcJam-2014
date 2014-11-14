using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceGenerator //The class that spawns all space decorations (stars, planets, maybe warpgates too)
{
    private const float SPACE_MAX_X = 75.0f;
    private const float SPACE_MAX_Y = 75.0f;
    private const float SPACE_MAX_Z = 75.0f;

    private const float PLANET_SPAWN_CHANCE = 0.1f; //10% chance to spawn a planet
    private const float SHATTERED_PLANET_SPAWN_CHANCE = 0.4f; //If a planet spawns, it has a 40% chance of being the shattered design
    private const float PLANET_MIN_COLOR_SUM = 0.4f;
    private const float PLANET_MAX_COLOR_SUM = 2.5f;

    private const int NUM_SPACE_OBJS = 450;

    private List<Object> possibleObjectsList = new List<Object>();

    private GameObject spaceContainer = null;

    private static SpaceGenerator instance = null;

    public static SpaceGenerator Instance
    {
        get
        {
            if (instance == null)
                instance = new SpaceGenerator();

            return instance;
        }
    }

    public GameObject SpaceContainer
    {
        get { return spaceContainer; }
        set { spaceContainer = value; }
    }

	// Use this for initialization
	private SpaceGenerator()
    {
        PopulateObjectPool();
    }
	
	// Update is called once per frame
	public void Update ()
    {
	
	}

    private void PopulateObjectPool()
    {
        //Stars
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Star_Small"));
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Star_Med"));
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Star_Shiny"));

        //Planets
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Planet_Whole"));
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Planet_Shattered"));
    }

    public void GenerateSpace()
    {
        for (int i = 0; i < NUM_SPACE_OBJS; i++)
        {
            GameObject newSpaceObject = GameObject.Instantiate(possibleObjectsList[GenerateSpaceObjType() ]) as GameObject;
            GenerateRandomPosition(newSpaceObject);

            if (newSpaceObject.name.Contains("Planet")) //If the object is a planet, colour it.
                GenerateRandomPlanetColour(newSpaceObject);

            newSpaceObject.transform.parent = spaceContainer.transform;
        }

            Debug.Log("Your space, sir.");
    }

    private int GenerateSpaceObjType()
    {
        //TODO: Make this some fancy percentage type deal.
        int returnIndex = 0;

        if (Random.value <= PLANET_SPAWN_CHANCE) //Spawn a planet
        {
            if (Random.value <= SHATTERED_PLANET_SPAWN_CHANCE)
                returnIndex = 4;
            else
                returnIndex = 3;
        }
        else //Spawn a random star
        {
            returnIndex = Mathf.FloorToInt(Random.value * 3);
        }

        return returnIndex;
    }

    private void GenerateRandomPosition(GameObject targetObj)
    {
        Vector3 newPos = Vector3.zero;

        newPos.x = Random.Range(-SPACE_MAX_X, SPACE_MAX_X);
        newPos.y = Random.Range(-SPACE_MAX_Y, SPACE_MAX_Y);
        newPos.z = Random.Range(-SPACE_MAX_Z, SPACE_MAX_Z);

        targetObj.transform.position = newPos;
    }

    private void GenerateRandomPlanetColour(GameObject planetObj)
    {
        SpriteRenderer planetSprite = planetObj.GetComponent<SpriteRenderer>();

        if (planetSprite != null)
        {
            //Generate a colour, preventing 0,0,0 black or 1,1,1 pure white planets.

            float randR = Random.value;
            float randG = Random.value;
            float randB = Random.value;
            float colourSum = randR + randG + randB;

            //If the colourSum does not meet the criteria, repeat the process.
            while (colourSum > PLANET_MAX_COLOR_SUM || colourSum < PLANET_MIN_COLOR_SUM)
            {
                randR = Random.value;
                randG = Random.value;
                randB = Random.value;
                colourSum = randR + randG + randB;
            }

            planetSprite.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}
