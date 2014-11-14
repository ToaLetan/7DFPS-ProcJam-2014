using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour 
{
    private const float MIN_RANGE_FROM_TARGET = 1.5f;
    private const float MAX_RANGE = 5.0f;
    private const float SHOT_COOLDOWN = 2.5f;
    private const float MOVE_SPEED = 7.5f;

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

        if (isTargetInRange == false || GetDistanceFromTarget() > MIN_RANGE_FROM_TARGET)
            MoveToTarget();
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
    }

    private void OnShotCooldownEnd()
    {
        canShoot = true;
        projectileTimer.ResetTimer();
    }

    private float GetDistanceFromTarget()
    {
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;

        return Mathf.Sqrt(Mathf.Pow(targetPosition.x - currentPosition.x, 2) + Mathf.Pow(targetPosition.y - currentPosition.y, 2) + Mathf.Pow(targetPosition.z - currentPosition.z, 2) );
    }

    private void MoveToTarget()
    {
        if (GetDistanceFromTarget() > MIN_RANGE_FROM_TARGET)
        {
            transform.LookAt(target.transform);

            gameObject.transform.position += gameObject.transform.forward * MOVE_SPEED * Time.deltaTime;
        }
    }

    private void MoveToRandPos()
    {

    }
}
