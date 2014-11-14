using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour 
{
    private const float ACCELERATION = 0.25f;
    private const float DECELERATION = 2.0f;
    private const float MAX_VELOCITY = 0.5f;
    private const float TURNSPEED = 5.0f;
    private const float MAX_ROTATION_SPEED = 2.5f;
    private const float ROTATION_DEADZONE = 10.0f;
    private const float ROTATION_DECELERATION = 1.0f;

    //Rotation variables
    private float currentRotationSpeedX = 0.0f;
    private float currentRotationSpeedY = 0.0f;
    private float previousMousePosX = 0.0f;
    private float previousMousePosY = 0.0f;
    private float rotationAccelerationX = 0.0f;
    private float rotationAccelerationY = 0.0f;

    private float movementAcceleration = 0.0f;
    public float currentVelocity = 0.0f;

    private Vector3 previousDirectionMoved = Vector3.zero;

    private InputManager inputManager;

    private GameObject cockpit = null;
    private GameObject crosshair = null;
    private GameObject cursor = null;

    private bool hasClickedWindow = false;

	// Use this for initialization
	void Start () 
    {
        //Hide the mouse cursor.
        Screen.showCursor = false;

        inputManager = InputManager.Instance;

        inputManager.Keys_Held += ProcessMovement;
        inputManager.Keys_Released += ApplyDeceleration;
        inputManager.Keys_Pressed += ProcessMouseClicks;
        //inputManager.Mouse_Movement += ProcessRotation;

        cockpit = gameObject.transform.FindChild("Cockpit").gameObject;
        crosshair = gameObject.transform.FindChild("Crosshair").gameObject;
        cursor = gameObject.transform.FindChild("Cursor").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
    {
        inputManager.Update();


        if (hasClickedWindow == true)
        {
            PositionCrosshair(inputManager.MousePosition);
            ProcessRotation(inputManager.MousePosition);
        }
	}

    private void ProcessMovement(List<string> keysHeld)
    {
        Vector3 newPosition = gameObject.transform.position;

        //Move forward or backward based on rotation.
        if (keysHeld.Contains(inputManager.PlayerKeybinds.ForwardKey.ToString()) )
        {
            if (currentVelocity < MAX_VELOCITY)
            {
                currentVelocity += ACCELERATION * Time.deltaTime;
            }
            previousDirectionMoved = gameObject.transform.forward;
            newPosition += gameObject.transform.forward * currentVelocity;
        }

        if (keysHeld.Contains(inputManager.PlayerKeybinds.BackwardKey.ToString()))
        {
            if (currentVelocity < MAX_VELOCITY)
            {
                currentVelocity += ACCELERATION * Time.deltaTime;
            }
            previousDirectionMoved = -gameObject.transform.forward;
            newPosition += -gameObject.transform.forward * currentVelocity;
        }

        //Strafe left or right based on rotation.
        if (keysHeld.Contains(inputManager.PlayerKeybinds.LeftKey.ToString()))
        {
            if (currentVelocity < MAX_VELOCITY)
            {
                currentVelocity += ACCELERATION * Time.deltaTime;
            }
            previousDirectionMoved = -gameObject.transform.right;
            newPosition += -gameObject.transform.right * currentVelocity;
        }

        if (keysHeld.Contains(inputManager.PlayerKeybinds.RightKey.ToString()))
        {
            if (currentVelocity < MAX_VELOCITY)
            {
                currentVelocity += ACCELERATION * Time.deltaTime;
            }
            previousDirectionMoved = gameObject.transform.right;
            newPosition += gameObject.transform.right * currentVelocity;
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

        //Quaternion newRotationX = Quaternion.AngleAxis(angleX, Vector3.up);
        //Quaternion newRotationY = Quaternion.AngleAxis(angleY, -Vector3.right);

        Quaternion newRotationX = Quaternion.AngleAxis(currentRotationSpeedX, Vector3.up);
        Quaternion newRotationY = Quaternion.AngleAxis(currentRotationSpeedY, -Vector3.right);

        gameObject.transform.rotation = newRotationX * newRotationY;

        previousMousePosX = angleX;
        previousMousePosY = angleY;
    }

    private void ApplyDeceleration(List<string> keysReleased)
    {
        Vector3 newPosition = gameObject.transform.position;

        if (keysReleased.Contains(inputManager.PlayerKeybinds.ForwardKey.ToString()) && keysReleased.Contains(inputManager.PlayerKeybinds.BackwardKey.ToString())
            && keysReleased.Contains(inputManager.PlayerKeybinds.LeftKey.ToString()) && keysReleased.Contains(inputManager.PlayerKeybinds.RightKey.ToString()))
        {
            if (currentVelocity > 0)
            {
                currentVelocity -= DECELERATION * Time.deltaTime;

                gameObject.transform.position += currentVelocity * previousDirectionMoved;
            }
        }
    }

    private void PositionCrosshair(Vector3 mousePosition)
    {
        float cursorX = 0;
        float cursorY = 0;

        float cockpitWidth = cockpit.transform.GetComponent<BoxCollider>().bounds.extents.x * 2;
        float cockpitHeight = cockpit.transform.GetComponent<BoxCollider>().bounds.extents.x * 2;

        //Determine an X and Y position based on the mouse position in relation to the HUD.
        float percentX = (mousePosition.x - Screen.width / 2) / Screen.width;
        float percentY = (mousePosition.y - Screen.height / 2) / Screen.height;

        cursorX = percentX * cockpitWidth;
        cursorY = percentY * cockpitHeight;

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
        //projectilePosition.z += 0.05f;

        projectile.transform.position = projectilePosition;
        projectile.transform.rotation = cockpit.transform.rotation;
    }
}
