using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Keybinds
{
    //Movement
    public KeyCode ForwardKey;
    public KeyCode BackwardKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode PauseKey;
    public KeyCode RestartKey;
    public KeyCode ExitKey;

    public int LeftMouse;
    public int RightMouse;
}

public class InputManager
{
    #region Fields
    public delegate void KeysHeldEvent(List<string> keysHeld);
    public delegate void KeysReleasedEvent(List<string> keysReleased);
    public delegate void KeysPressedEvent(List<string> keysPressed);
    public delegate void MouseMoveEvent(Vector3 mousePosition);

    public KeysHeldEvent Keys_Held;
    public KeysReleasedEvent Keys_Released;
    public KeysPressedEvent Keys_Pressed;
    public MouseMoveEvent Mouse_Movement;

    private Keybinds playerKeybinds = new Keybinds();

    private static InputManager instance = null;

    private Vector3 previousMousePos = Vector3.zero;
    private Vector3 mousePosition = Vector3.zero;
    #endregion

    #region Properties
    public Keybinds PlayerKeybinds
    {
        get { return playerKeybinds; }
    }

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = new InputManager();
            return instance;
        }
    }

    public Vector3 MousePosition
    {
        get { return mousePosition; }
    }
    #endregion

    private InputManager()
    {
        playerKeybinds.ForwardKey = KeyCode.W;
        playerKeybinds.BackwardKey = KeyCode.S;
        playerKeybinds.LeftKey = KeyCode.A;
        playerKeybinds.RightKey = KeyCode.D;
        playerKeybinds.PauseKey = KeyCode.P;
        playerKeybinds.RestartKey = KeyCode.R;
        playerKeybinds.ExitKey = KeyCode.Escape;

        playerKeybinds.LeftMouse = 0;
        playerKeybinds.RightMouse = 1;
    }

    #region Methods
    public void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        //Check for specific keys being held down.
        List<string> heldKeys = new List<string>();

        if(Input.GetKey(playerKeybinds.ForwardKey) )
            heldKeys.Add(playerKeybinds.ForwardKey.ToString() );
        if (Input.GetKey(playerKeybinds.BackwardKey))
            heldKeys.Add(playerKeybinds.BackwardKey.ToString());
        if (Input.GetKey(playerKeybinds.LeftKey))
            heldKeys.Add(playerKeybinds.LeftKey.ToString());
        if (Input.GetKey(playerKeybinds.RightKey))
            heldKeys.Add(playerKeybinds.RightKey.ToString());

        if (Input.GetMouseButton(playerKeybinds.LeftMouse))
            heldKeys.Add(playerKeybinds.LeftMouse.ToString());
        if (Input.GetMouseButton(playerKeybinds.RightMouse))
            heldKeys.Add(playerKeybinds.RightMouse.ToString());

        if (heldKeys.Count > 0)
        {
            if (Keys_Held != null)
                Keys_Held(heldKeys);

            heldKeys.Clear();
        }

        //Check for specific keys being released.
        List<string> releasedKeys = new List<string>();

        if (!Input.GetKey(playerKeybinds.ForwardKey))
            releasedKeys.Add(playerKeybinds.ForwardKey.ToString());
        if (!Input.GetKey(playerKeybinds.BackwardKey))
            releasedKeys.Add(playerKeybinds.BackwardKey.ToString());
        if (!Input.GetKey(playerKeybinds.LeftKey))
            releasedKeys.Add(playerKeybinds.LeftKey.ToString());
        if (!Input.GetKey(playerKeybinds.RightKey))
            releasedKeys.Add(playerKeybinds.RightKey.ToString());

        if (!Input.GetMouseButton(playerKeybinds.LeftMouse))
            releasedKeys.Add(playerKeybinds.LeftMouse.ToString());
        if (!Input.GetMouseButton(playerKeybinds.RightMouse))
            releasedKeys.Add(playerKeybinds.RightMouse.ToString());

        if (releasedKeys.Count > 0)
        {
            if (Keys_Released != null)
                Keys_Released(releasedKeys);

            releasedKeys.Clear();
        }

        //Check for specific keys being pressed once.
        List<string> pressedKeys = new List<string>();

        if (Input.GetKeyDown(playerKeybinds.ForwardKey))
            pressedKeys.Add(playerKeybinds.ForwardKey.ToString());
        if (Input.GetKeyDown(playerKeybinds.BackwardKey))
            pressedKeys.Add(playerKeybinds.BackwardKey.ToString());
        if (Input.GetKeyDown(playerKeybinds.LeftKey))
            pressedKeys.Add(playerKeybinds.LeftKey.ToString());
        if (Input.GetKeyDown(playerKeybinds.RightKey))
            pressedKeys.Add(playerKeybinds.RightKey.ToString());
        if (Input.GetKeyDown(playerKeybinds.PauseKey))
            pressedKeys.Add(playerKeybinds.PauseKey.ToString());
        if (Input.GetKeyDown(playerKeybinds.RestartKey))
            pressedKeys.Add(playerKeybinds.RestartKey.ToString());
        if (Input.GetKeyDown(playerKeybinds.ExitKey))
            pressedKeys.Add(playerKeybinds.ExitKey.ToString());

        if (Input.GetMouseButtonDown(playerKeybinds.LeftMouse))
            pressedKeys.Add(playerKeybinds.LeftMouse.ToString());
        if (Input.GetMouseButtonDown(playerKeybinds.RightMouse))
            pressedKeys.Add(playerKeybinds.RightMouse.ToString());

        if (pressedKeys.Count > 0)
        {
            if (Keys_Pressed != null)
                Keys_Pressed(pressedKeys);

            pressedKeys.Clear();
        }

        //Check mouse movement
        mousePosition = Input.mousePosition;

        if (Input.mousePosition != previousMousePos)
        {
            if (Mouse_Movement != null)
                Mouse_Movement(Input.mousePosition);

            previousMousePos = Input.mousePosition;
        }
    }
    #endregion
}
