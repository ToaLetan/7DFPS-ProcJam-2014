using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour 
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
            case "Enemy":
                GameObject.Destroy(collider.gameObject);
                GameObject.Destroy(gameObject);
                break;
        }
    }
}
