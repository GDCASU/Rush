using System;
using System.Linq;
using System.Collections.Generic;
using XInputDotNetPure;

/*
 * Author:      Zachary Schmalz
 * Version:     1.0.0
 * Date:        September 19, 2018
 */

/// <summary>
/// This XboxController class outlines the buttons and axes for the Xbox 360/One controller functionality
/// </summary>
public class XboxController
{
    /// <summary>
    /// This enum lists all of the controller axes of type float
    /// </summary>
    public enum XboxAxis
    {
        LeftStickHorizontal,
        LeftStickVertical,
        RightStickHorizontal,
        RightStickVertical,
        LeftTrigger,
        RightTrigger,
        None
    };
    /// <summary>
    /// This enum lists all of the xontroller buttons of type ButtonPresses / bool
    /// </summary>
    public enum XboxButton
    {
        A,
        B,
        X,
        Y,
        LeftBumper,
        RightBumper,
        LeftStick,
        RightStick,
        Back,
        Start,
        Guide,
        DPadUp,
        DPadRight,
        DPadDown,
        DPadLeft,
        None
    };

    // Private class variables
    private PlayerIndex playerIndex;
    private GamePadState currentState;
    private GamePadState previousState;
    private Dictionary<XboxAxis, float> previousAxisDict;
    private Dictionary<XboxAxis, float> currentAxisDict;
    private Dictionary<XboxButton, ButtonState> previousButtonDict;
    private Dictionary<XboxButton, ButtonState> currentButtonDict;

    // Public class properties
    /// <summary>
    /// Returns the state of the controller being connected or not
    /// </summary>
    public bool IsConnected { get; private set; }
    /// <summary>
    /// Returns true if any button is being held down
    /// </summary>
    public bool AnyButton
    {
        get
        {
            if (IsConnected == false)
                return false;

            foreach (KeyValuePair<XboxButton, ButtonState> xb in previousButtonDict)
                if (xb.Value == ButtonState.Pressed && currentButtonDict[xb.Key] == ButtonState.Pressed)
                    return true;
            return false;
        }
    }
    /// <summary>
    /// Returns true during the frame any button was pressed
    /// </summary>
    public bool AnyButtonDown
    {
        get
        {
            if (IsConnected == false)
                return false;

            foreach (KeyValuePair<XboxButton, ButtonState> xb in previousButtonDict)
                if (xb.Value == ButtonState.Released && currentButtonDict[xb.Key] == ButtonState.Pressed)
                    return true;
            return false;
        }
    }
    /// <summary>
    /// Returns the first Xbox button being pressed
    /// </summary>
    public XboxButton NextXboxButton
    {
        get
        {
            if (IsConnected == false)
                return XboxButton.None;

            foreach (KeyValuePair<XboxButton, ButtonState> xb in currentButtonDict)
                if (currentButtonDict[xb.Key] == ButtonState.Pressed)
                    return xb.Key;
            return XboxButton.None;
        }
    }

    /// <summary>
    /// Constructor that sets the InputType, the PlayerIndex, and initializes the input dictionaries
    /// </summary>
    /// <param name="index">Which index is this controller mapped to</param>
    public XboxController(PlayerIndex index)
    {
        playerIndex = index;
        IsConnected = false;

        InitializeDictionaries();
    }

	public void Start() {}
	
    /// <summary>
    /// If the controller is connected, updates the controller state and dictionary values
    /// If disconnected, will continue to listen for connecting controllers
    /// </summary>
	public void Update()
    {
        // Check for controller connection
        if (IsConnected == false)
        {
            // If controller is connected, add a new player to the input manager
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                IsConnected = true;
                InputManager.AddPlayer(InputManager.InputMethod.XboxController);
                Debug.InputLog("XboxController " + (int)(playerIndex + 1) + " connected");
            }
        }

        else
        {
            // Update the previous and current controller state
            previousState = currentState;
            currentState = GamePad.GetState(playerIndex, GamePadDeadZone.IndependentAxes);

            // Check for controller disconnect and remove player if disconnected
            if (previousState.IsConnected && currentState.IsConnected == false)
            {
                IsConnected = false;
                InputManager.RemovePlayer((int)playerIndex);
                Debug.InputLog("Controller " + (int)(playerIndex + 1) + " disconnected", Debug.LogType.Warning);
                return;
            }

            UpdateDictionaries();
        }
	}

    /// <summary>
    /// Returns the current axis value of the controller
    /// </summary>
    /// <param name="axis">The axis on the controller to check</param>
    public float GetAxis(XboxAxis axis)
    {
        if (!IsConnected)
            return 0.0f;
        return axis == XboxAxis.None ? 0.0f : currentAxisDict[axis];
    }

    /// <summary>
    /// Returns the axis value on the first frame where the axis reaches its maximum absolute value
    /// </summary>
    /// <param name="axis"></param>
    public float GetAxisDown(XboxAxis axis)
    {
        if (!IsConnected)
            return 0.0f;
        return Math.Abs(currentAxisDict[axis]) == 1 && Math.Abs(previousAxisDict[axis]) != 1 ? currentAxisDict[axis] : 0; 
    }

    /// <summary>
    /// Returns true while the button is held down
    /// </summary>
    /// <param name="button">The button on the controller to check</param>
    public bool GetButton(XboxButton button)
    {
        if (!IsConnected)
            return false;
        return button == XboxButton.None ? false : currentButtonDict[button] == ButtonState.Pressed ? true : false;
    }

    /// <summary>
    /// Returns true during the first frame the button was held down
    /// </summary>
    /// <param name="button">The button on the controller to check</param>
    public bool GetButtonDown(XboxButton button)
    {
        if (!IsConnected)
            return false;
        return button == XboxButton.None ? false : previousButtonDict[button] == ButtonState.Released && currentButtonDict[button] == ButtonState.Pressed ? true : false;
    }

    /// <summary>
    /// Returns true during the first frame the button was released
    /// </summary>
    /// <param name="button">The button on the controller to check</param>
    public bool GetButtonUp(XboxButton button)
    {
        if (!IsConnected)
            return false;
        return button == XboxButton.None ? false : currentButtonDict[button] == ButtonState.Released && previousButtonDict[button] == ButtonState.Pressed ? true : false;
    }

    /// <summary>
    /// Resets the ButtonState of the button in the current button dictionary
    /// </summary>
    /// <param name="button">The button to reset</param>
    public void ResetButton(XboxButton button)
    {
        currentButtonDict[button] = ButtonState.Released;
    }

    /// <summary>
    /// Create the dictionaries required for holding the controllers' axis values, and current/previous button values
    /// </summary>
    private void InitializeDictionaries()
    {
        // Initialize the previous XboxAxis dictionary with 0 values
        previousAxisDict = new Dictionary<XboxAxis, float>();
        foreach (XboxAxis axis in Enum.GetValues(typeof(XboxAxis)))
            if (axis != XboxAxis.None)
                previousAxisDict.Add(axis, 0);

        // Initiailze the previous XboxButton dictionary with Released ButtonState
        previousButtonDict = new Dictionary<XboxButton, ButtonState>();
        foreach(XboxButton button in Enum.GetValues(typeof(XboxButton)))
            if (button != XboxButton.None)
                previousButtonDict.Add(button, ButtonState.Released);

        // Initialize the current XboxAxis dictionary with 0 values
        currentAxisDict = previousAxisDict.ToDictionary(entry => entry.Key, entry => entry.Value);

        // Initiailze the current XboxButton dictionary with Released ButtonState
        currentButtonDict = previousButtonDict.ToDictionary(entry => entry.Key, entry => entry.Value);
    }

    /// <summary>
    /// Update the dictionaries axis values and previous/current controller button values
    /// </summary>
    private void UpdateDictionaries()
    {
        // Transfer the current XboxAxis dictionary values to the previous dictionary values
        foreach (KeyValuePair<XboxAxis, float> kp in previousAxisDict.ToList())
            previousAxisDict[kp.Key] = currentAxisDict[kp.Key];

        // Transfer the current XboxButton dictionary values to the previous dictionary values
        foreach (KeyValuePair<XboxButton, ButtonState> kp in previousButtonDict.ToList())
            previousButtonDict[kp.Key] = currentButtonDict[kp.Key];

        // Update the current XboxAxis dictionary values
        currentAxisDict[XboxAxis.LeftStickHorizontal] = currentState.ThumbSticks.Left.X;
        currentAxisDict[XboxAxis.LeftStickVertical] = currentState.ThumbSticks.Left.Y;
        currentAxisDict[XboxAxis.RightStickHorizontal] = currentState.ThumbSticks.Right.X;
        currentAxisDict[XboxAxis.RightStickVertical] = currentState.ThumbSticks.Right.Y;
        currentAxisDict[XboxAxis.LeftTrigger] = currentState.Triggers.Left;
        currentAxisDict[XboxAxis.RightTrigger] = currentState.Triggers.Right;
        
        // Update the current XboxButton dictionary values
        currentButtonDict[XboxButton.A] = currentState.Buttons.A;
        currentButtonDict[XboxButton.B] = currentState.Buttons.B;
        currentButtonDict[XboxButton.X] = currentState.Buttons.X;
        currentButtonDict[XboxButton.Y] = currentState.Buttons.Y;
        currentButtonDict[XboxButton.LeftBumper] = currentState.Buttons.LeftShoulder;
        currentButtonDict[XboxButton.RightBumper] = currentState.Buttons.RightShoulder;
        currentButtonDict[XboxButton.LeftStick] = currentState.Buttons.LeftStick;
        currentButtonDict[XboxButton.RightStick] = currentState.Buttons.RightStick;
        currentButtonDict[XboxButton.Back] = currentState.Buttons.Back;
        currentButtonDict[XboxButton.Start] = currentState.Buttons.Start;
        currentButtonDict[XboxButton.Guide] = currentState.Buttons.Guide;
        currentButtonDict[XboxButton.DPadUp] = currentState.DPad.Up;
        currentButtonDict[XboxButton.DPadRight] = currentState.DPad.Right;
        currentButtonDict[XboxButton.DPadDown] = currentState.DPad.Down;
        currentButtonDict[XboxButton.DPadLeft] = currentState.DPad.Left;
    }
}