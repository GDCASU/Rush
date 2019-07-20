using UnityEngine;

/*
 * Author:      Zachary Schmalz
 * Version:     1.0.0
 * Date:        September 19, 2018
 */

/// <summary>
/// This KeyboardController class is a wrapper class utilizing Unity's Input methods for keyboard input
/// </summary>
public class KeyboardController
{
    /// <summary>
    /// Returns true while any keyboard or mouse button is held down
    /// </summary>
    public bool AnyButton { get { return Input.anyKey; } }
    /// <summary>
    /// Returns true during the first frame any keyboard or mouse button is pressed
    /// </summary>
    public bool AnyButtonDown { get { return Input.anyKeyDown; } }

    public KeyboardController() {}

    public void Start() {}

    public void Update() {}

    /// <summary>
    /// Returns the float value of keyboard axis
    /// </summary>
    /// <param name="axis">The keyboard button to check the value of</param>
    public float GetAxis(KeyCode axis)
    {
        return axis == KeyCode.None ? 0.0f : Input.GetKey(axis) ? 1.0f : 0.0f;
    }

    /// <summary>
    /// Returns the float value of the axis during the first frame the key was pressed
    /// </summary>
    /// <param name="axis"></param>
    public float GetAxisDown(KeyCode axis)
    {
        return axis == KeyCode.None ? 0.0f : Input.GetKeyDown(axis) ? 1.0f : 0.0f;
    }

    /// <summary>
    /// Return true while the keyboard button is being held down
    /// </summary>
    /// <param name="button">The keyboard button to check</param>
    public bool GetButton(KeyCode button)
    {
        return button == KeyCode.None ? false : Input.GetKey(button);
    }

    /// <summary>
    /// Return true during the first frame the keyboard button is held down
    /// </summary>
    /// <param name="button">The keyboard button to check</param>
    public bool GetButtonDown(KeyCode button)
    {
        return button == KeyCode.None ? false : Input.GetKeyDown(button);
    }

    /// <summary>
    /// Returns true during the first frame the keyboard button is released
    /// </summary>
    /// <param name="button">The keyboard button to check</param>
    public bool GetButtonUp(KeyCode button)
    {
        return button == KeyCode.None ? false : Input.GetKeyUp(button);
    }
}