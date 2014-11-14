using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour 
{
    private const int DAMAGE = 1;

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
                if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerHull > 0)
                {
                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ApplyPlayerDamage(DAMAGE);
                }
                break;
        }
    }
}
