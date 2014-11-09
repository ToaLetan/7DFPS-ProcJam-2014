using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour 
{
    private const float MOVESPEED = 0.01f;

    private InputManager inputManager;


	// Use this for initialization
	void Start () 
    {
        inputManager = InputManager.Instance;

        inputManager.Keys_Held += ProcessMovement;
        inputManager.Mouse_Movement += ProcessRotation;
	}
	
	// Update is called once per frame
	void Update () 
    {
        inputManager.Update();
	}

    private void ProcessMovement(List<string> keysHeld)
    {
        Vector3 newPosition = gameObject.transform.position;

        //Move forward or backward based on rotation.
        if (keysHeld.Contains(inputManager.PlayerKeybinds.ForwardKey.ToString()) )
        {
            newPosition += gameObject.transform.forward * MOVESPEED;
        }
        if (keysHeld.Contains(inputManager.PlayerKeybinds.BackwardKey.ToString()))
        {
            newPosition -= gameObject.transform.forward * MOVESPEED;
        }

        //Strafe left or right based on rotation.
        if (keysHeld.Contains(inputManager.PlayerKeybinds.LeftKey.ToString()))
        {
            newPosition -= gameObject.transform.right * MOVESPEED;
        }
        if (keysHeld.Contains(inputManager.PlayerKeybinds.RightKey.ToString()))
        {
            newPosition += gameObject.transform.right * MOVESPEED;
        }

        gameObject.transform.position = newPosition;
    }

    private void ProcessRotation(Vector3 mousePosition)
    {
        float angle = mousePosition.x - 180;

        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.up);

        gameObject.transform.rotation = newRotation;

        Debug.Log(mousePosition);
    }
}
