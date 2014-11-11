using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour 
{
    private const float PROJECTILE_SPEED = 5.0f;
    private const float PROJECTILE_LIFESPAN = 10.0f;

    private Timer lifeTimer = null;

	// Use this for initialization
	void Start () 
    {
        lifeTimer = new Timer(PROJECTILE_LIFESPAN, true);
        lifeTimer.OnTimerComplete += DestroyProjectile;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (lifeTimer != null)
            lifeTimer.Update();

        ProcessMovement();
	}

    private void ProcessMovement()
    {
        Vector3 newPosition = gameObject.transform.position;

        newPosition += gameObject.transform.forward * PROJECTILE_SPEED * Time.deltaTime;

        gameObject.transform.position = newPosition;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
