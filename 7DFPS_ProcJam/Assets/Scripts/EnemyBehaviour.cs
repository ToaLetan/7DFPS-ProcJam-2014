using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour 
{
    private const float MAX_RANGE = 5.0f;
    private const float SHOT_COOLDOWN = 2.5f;

    private GameObject target = null;

    private Timer projectileTimer = null;

    private bool isTargetInRange = false;
    private bool canShoot = false;

	// Use this for initialization
	void Start () 
    {
        target = GameObject.FindGameObjectWithTag("Player");
        projectileTimer = new Timer(SHOT_COOLDOWN, true);

        projectileTimer.OnTimerComplete += OnShotCooldownEnd;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //AI logic steps
        //If target isn't in range, move towards it
        //If target is in range, shoot

        CheckTargetRange();

        if (canShoot == false)
            projectileTimer.Update();

        if (isTargetInRange == true && canShoot == true)
            Shoot();
	}

    private void CheckTargetRange()
    {
        if (target != null)
        {
            if ((target.transform.position.x >= gameObject.transform.position.x - MAX_RANGE && target.transform.position.x <= gameObject.transform.position.x + MAX_RANGE)
                && (target.transform.position.y >= gameObject.transform.position.y - MAX_RANGE && target.transform.position.y <= gameObject.transform.position.y + MAX_RANGE)
                && (target.transform.position.z >= gameObject.transform.position.z - MAX_RANGE && target.transform.position.z <= gameObject.transform.position.z + MAX_RANGE))
            {
                isTargetInRange = true;
            }
            else
            {
                isTargetInRange = false;
            }
                
        }
    }

    private void Shoot()
    {
        GameObject projectile = GameObject.Instantiate(Resources.Load("Prefabs/Enemy_Projectile")) as GameObject;

        Vector3 projectilePosition = gameObject.transform.position;
        projectilePosition.z += 0.05f;

        projectile.transform.position = projectilePosition;
        projectile.transform.rotation = gameObject.transform.rotation;

        canShoot = false;
        projectileTimer.StartTimer();

        Debug.Log("SHOOSTED");
    }

    private void OnShotCooldownEnd()
    {
        canShoot = true;
        projectileTimer.ResetTimer();
    }
}
