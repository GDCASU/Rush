/*
 * Author:      Zachary Schmalz
 * Version:     1.0.0
 * Date:        September 28, 2018
 */

 /// <summary>
 /// This interface is used for communication with the InputManager class. 
 /// </summary>
public interface IInputPlayer
{
    InputManager.InputMethod InputMethod { get; set; }
    int PlayerIndex { get; set; }
}