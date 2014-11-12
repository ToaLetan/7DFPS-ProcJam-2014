using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceGenerator //The class that spawns all space decorations (stars, planets, maybe warpgates too)
{
    private const float SPACE_MAX_X = 50.0f;
    private const float SPACE_MAX_Y = 50.0f;
    private const float SPACE_MAX_Z = 50.0f;

    private const int NUM_SPACE_OBJS = 300;

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
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Star_Small"));
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Star_Med"));
        possibleObjectsList.Add(Resources.Load<GameObject>("Prefabs/SpaceScenery/Star_Shiny"));
    }

    public void GenerateSpace()
    {
        for (int i = 0; i < NUM_SPACE_OBJS; i++)
        {
            GameObject newSpaceObject = GameObject.Instantiate(possibleObjectsList[GenerateSpaceObjType() ]) as GameObject;
            GenerateRandomPosition(newSpaceObject);

            newSpaceObject.transform.parent = spaceContainer.transform;
        }

            Debug.Log("Your space, sir.");
    }

    private int GenerateSpaceObjType()
    {
        //TODO: Make this some fancy percentage type deal.
        int returnIndex = 0;

        returnIndex = Mathf.FloorToInt(Random.value * 3);

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
}
