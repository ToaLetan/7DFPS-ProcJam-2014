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
    #endregion

    private InputManager()
    {
        playerKeybinds.ForwardKey = KeyCode.W;
        playerKeybinds.BackwardKey = KeyCode.S;
        playerKeybinds.LeftKey = KeyCode.A;
        playerKeybinds.RightKey = KeyCode.D;
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

        if (releasedKeys.Count > 0)
        {
            if (Keys_Released != null)
                Keys_Released(releasedKeys);

            releasedKeys.Clear();
        }

        //Check for specific keys being pressed once.
        List<string> pressedKeys = new List<string>();

        if (!Input.GetKeyDown(playerKeybinds.ForwardKey))
            pressedKeys.Add(playerKeybinds.ForwardKey.ToString());
        if (!Input.GetKeyDown(playerKeybinds.BackwardKey))
            pressedKeys.Add(playerKeybinds.BackwardKey.ToString());
        if (!Input.GetKeyDown(playerKeybinds.LeftKey))
            pressedKeys.Add(playerKeybinds.LeftKey.ToString());
        if (!Input.GetKeyDown(playerKeybinds.RightKey))
            pressedKeys.Add(playerKeybinds.RightKey.ToString());

        if (pressedKeys.Count > 0)
        {
            if (Keys_Pressed != null)
                Keys_Pressed(pressedKeys);

            pressedKeys.Clear();
        }

        //Check mouse movement
        if (Input.mousePosition != previousMousePos)
        {
            if (Mouse_Movement != null)
                Mouse_Movement(Input.mousePosition);

            previousMousePos = Input.mousePosition;
        }
    }
    #endregion
}
