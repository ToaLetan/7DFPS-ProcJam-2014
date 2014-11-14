using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour 
{
    private const int DAMAGE = 1;

    private GameManager gameManager = null;

	// Use this for initialization
	void Start () 
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
                if (gameManager.PlayerHull > 0)
                {
                    gameManager.ApplyPlayerDamage(DAMAGE);

                    if (gameManager.PlayerHull <= 0)
                    {
                        gameManager.IsGameOver = true;
                        gameManager.IsGamePaused = true;
                    }

                    Destroy(gameObject);
                }
                break;
        }
    }
}
