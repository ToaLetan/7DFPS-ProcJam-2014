using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour 
{
    private const float MOVESPEED = 5.0f;
    private const float TURNSPEED = 5.0f;
    private const float MAX_ROTATION_SPEED = 2.5f;
    private const float ROTATION_DEADZONE = 20.0f;

    private float currentRotationSpeedX = 0.0f;
    private float currentRotationSpeedY = 0.0f;
    private float previousMousePosX = 0.0f;
    private float previousMousePosY = 0.0f;
    private float rotationAccelerationX = 0.0f;
    private float rotationAccelerationY = 0.0f;

    private InputManager inputManager;

    private GameObject cockpit = null;
    private GameObject crosshair = null;

    private bool hasClickedWindow = false;

	// Use this for initialization
	void Start () 
    {
        inputManager = InputManager.Instance;

        inputManager.Keys_Held += ProcessMovement;
        inputManager.Keys_Pressed += ProcessMouseClicks;
        //inputManager.Mouse_Movement += ProcessRotation;

        cockpit = gameObject.transform.FindChild("Cockpit").gameObject;
        crosshair = gameObject.transform.FindChild("Crosshair").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
    {
        inputManager.Update();

        if (hasClickedWindow == true)
            ProcessRotation(inputManager.MousePosition);
        //PositionCrosshair(inputManager.MousePosition);
	}

    private void ProcessMovement(List<string> keysHeld)
    {
        Vector3 newPosition = gameObject.transform.position;

        //Move forward or backward based on rotation.
        if (keysHeld.Contains(inputManager.PlayerKeybinds.ForwardKey.ToString()) )
        {
            newPosition += gameObject.transform.forward * MOVESPEED * Time.deltaTime;
        }
        if (keysHeld.Contains(inputManager.PlayerKeybinds.BackwardKey.ToString()))
        {
            newPosition += -gameObject.transform.forward * MOVESPEED * Time.deltaTime;
        }

        //Strafe left or right based on rotation.
        if (keysHeld.Contains(inputManager.PlayerKeybinds.LeftKey.ToString()))
        {
            newPosition += -gameObject.transform.right * MOVESPEED * Time.deltaTime;
        }
        if (keysHeld.Contains(inputManager.PlayerKeybinds.RightKey.ToString()))
        {
            newPosition += gameObject.transform.right * MOVESPEED * Time.deltaTime;
        }

        gameObject.transform.position = newPosition;
    }

    private void ProcessRotation(Vector3 mousePosition)
    {
        //TODO: Figure out a way to handle being upside-down (ALWAYS turn right when mouse is on the right side)

        float angleX = mousePosition.x - Screen.width/2;
        float angleY = mousePosition.y - Screen.height/2;

        int directionX = 0;
        int directionY = 0;

        //Apply acceleration to the horizontal rotation and cap it at MAX_ROTATION_SPEED
        if (rotationAccelerationX < MAX_ROTATION_SPEED && rotationAccelerationX > -MAX_ROTATION_SPEED)
            rotationAccelerationX = (angleX * 0.01f);

        if (rotationAccelerationX > MAX_ROTATION_SPEED)
            rotationAccelerationX = MAX_ROTATION_SPEED;
        if (rotationAccelerationX < -MAX_ROTATION_SPEED)
            rotationAccelerationX = -MAX_ROTATION_SPEED;

        //Prevent any rotation if the mouse is near the center of the screen.
        if (angleX > ROTATION_DEADZONE)
        {
            directionX = 1;
        }
        else if (angleX < -ROTATION_DEADZONE)
        {
            directionX = -1;
        }
        else
            rotationAccelerationX = 0;

        //Do the same to the vertical rotation.
        if (rotationAccelerationY < MAX_ROTATION_SPEED && rotationAccelerationY > -MAX_ROTATION_SPEED)
            rotationAccelerationY = (angleY * 0.01f);

        if (rotationAccelerationY > MAX_ROTATION_SPEED)
            rotationAccelerationY = MAX_ROTATION_SPEED;
        if (rotationAccelerationY < -MAX_ROTATION_SPEED)
            rotationAccelerationY = -MAX_ROTATION_SPEED;

        //Prevent any rotation if the mouse is near the center of the screen.
        if (angleY > ROTATION_DEADZONE)
        {
            directionY = 1;
        }
        else if (angleY < -ROTATION_DEADZONE)
        {
            directionY = -1;
        }
        else
            rotationAccelerationY = 0;

        //Apply the acceleration to the rotation speed.
        currentRotationSpeedX += rotationAccelerationX + TURNSPEED * Time.deltaTime * directionX;
        currentRotationSpeedY += rotationAccelerationY + TURNSPEED * Time.deltaTime * directionY;

        Quaternion newRotationX = Quaternion.AngleAxis(currentRotationSpeedX, Vector3.up);
        Quaternion newRotationY = Quaternion.AngleAxis(currentRotationSpeedY, -Vector3.right);

        gameObject.transform.rotation = newRotationX * newRotationY;
    }

    private void PositionCrosshair(Vector3 mousePosition)
    {
        float cursorX = 0;
        float cursorY = 0;

        cursorX = mousePosition.x - Screen.width / 2;
        cursorY = mousePosition.y - Screen.height / 2;

        crosshair.transform.localPosition = new Vector3(cursorX, cursorY, crosshair.transform.localPosition.z);
    }

    private void ProcessMouseClicks(List<string> keysPressed)
    {
        if (keysPressed.Contains(inputManager.PlayerKeybinds.LeftMouse.ToString()) )
        {
            if (hasClickedWindow == true)
                Shoot();
            else
                hasClickedWindow = true;
        }

        if (keysPressed.Contains(inputManager.PlayerKeybinds.PauseKey.ToString()) )
            hasClickedWindow = false;
    }

    private void Shoot()
    {
        GameObject projectile = GameObject.Instantiate(Resources.Load("Prefabs/Projectile")) as GameObject;

        Vector3 projectilePosition = crosshair.transform.position;
        projectilePosition.z += 0.05f;

        projectile.transform.position = projectilePosition;
        projectile.transform.rotation = cockpit.transform.rotation;
    }
}
