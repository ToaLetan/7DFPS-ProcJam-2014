using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour 
{


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision collider)
    {
        switch(collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("HULL DAMAGE");
                break;
        }
    }
}
