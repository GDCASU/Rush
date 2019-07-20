using UnityEngine;
using PlayerInput;

/*
 * Author:      Zachary Schmalz
 * Version:     1.0.0
 * Date:        September 19, 2018
 * 
 * Author:      Zachary Schmalz
 * Version:     1.1.0
 * Date:        October 12, 2018
 *              Updated class to use new Input system
 */

/// <summary>
/// This player class is used to communicate with the InputManager, and handle any player related functions
/// </summary>
public class InputPlayer : MonoBehaviour, IInputPlayer
{
    // Public class properties
    /// <summary>
    /// Property to get and set the players' input method type
    /// </summary>
    public InputManager.InputMethod InputMethod
    {
        get { return inputMethod; }
        set { inputMethod = value; }
    }

    /// <summary>
    /// Property to get and set the player index
    /// </summary>
    public int PlayerIndex
    {
        get { return playerIndex; }
        set { playerIndex = value; }
    }
    
    [SerializeField]
    private InputManager.InputMethod inputMethod;
    [SerializeField]
    private int playerIndex;

    public void Start () {
        if(InputManager.ControllersConnected > 0) inputMethod = InputManager.InputMethod.XboxController;
    }

/*
    void Update ()
    {
        // If getting specific player input (in this case, this player)

        float f = 0;
        if ((f = InputManager.GetAxis(PlayerAxis.MoveHorizontal, this)) != 0) Debug.InputLog("Player " + (PlayerIndex + 1) + " - Left Horizontal: " + f);
        if ((f = InputManager.GetAxis(PlayerAxis.MoveVertical, this)) != 0) Debug.InputLog("Player " + (PlayerIndex + 1) + " - Left Vertical: " + f);
        if ((f = InputManager.GetAxis(PlayerAxis.CameraHorizontal, this)) != 0) Debug.InputLog("Player " + (PlayerIndex + 1) + " - Camera Horizontal: " + f);
        if ((f = InputManager.GetAxis(PlayerAxis.CameraVertical, this)) != 0) Debug.InputLog("Player " + (PlayerIndex + 1) + " - Camera Vertical: " + f);
        if (InputManager.GetButton(PlayerButton.Jump, this)) Debug.InputLog("Player " + (PlayerIndex + 1) + " - Jump");
    } */
}