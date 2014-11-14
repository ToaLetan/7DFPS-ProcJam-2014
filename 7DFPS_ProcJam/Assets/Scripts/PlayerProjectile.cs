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
                //Spawn the enemy death animation and destroy the enemy.
                GameObject.Instantiate(Resources.Load("Prefabs/Enemy_Death_Anim") as GameObject, collider.gameObject.transform.position, Quaternion.identity);

                GameObject.Destroy(collider.gameObject);

                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerNumKills++;

                GameObject.Destroy(gameObject);
                break;
        }
    }
}
