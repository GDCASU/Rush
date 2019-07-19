//using System;
//using System.Linq;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

///*
// * Author:      Zachary Schmalz
// * Version:     1.0.0
// * Date:        September 19, 2018
// */

/// <summary>
/// The class handles the display and remapping of input controls
/// </summary>
//public class InputRemap : MonoBehaviour
//{
//     Public class variables
//    public GameObject controlPrefab;
//    public Text playerText;
//    public Text inputText;
//    public GameObject axisPanel;
//    public GameObject buttonPanel;
//    public Color neutralColor;
//    public Color selectedColor;

//    Private class variables
//    private List<GameObject> controls;
//    private int currentSelected;
//    private int currentPlayer;
//    private bool isRemapping;
//    private float leftVertical, leftHorizontal;
//    private bool up, right, down, left;
//    private InputManager.InputMethod previousMethod;

//     Private class properties
//    / <summary>
//    / Returns the current selected control in the controls list
//    / </summary>
//    private int CurrentSelected { get { return currentSelected; } set { currentSelected = Mathf.Clamp(value, 0, controls.Count - 1); } }
//    / <summary>
//    / Returns the Player instance class of the current player
//    / </summary>
//    private IInputPlayer CurrentPlayer { get { return InputManager.Players[currentPlayer].GetComponent<IInputPlayer>(); } }

//    void Start()
//    {
//        controls = new List<GameObject>() { playerText.gameObject, inputText.gameObject };
//        currentPlayer = 0;
//        UpdateText();
//        previousMethod = CurrentPlayer.InputMethod;
//        isRemapping = false;
//        CurrentSelected = 0;
//        Select();
//    }

//        void Update()
//        {
//            // Update the text in the event of an input change
//            if (previousMethod != CurrentPlayer.InputMethod)
//            {
//                UpdateText();
//                previousMethod = CurrentPlayer.InputMethod;
//            }

//            UpdateInput();

//            // Do not update anything while controls are being remapped
//            if (!isRemapping)
//            {
//                // Current player text
//                if (CurrentSelected == 0)
//                {
//                    if (leftHorizontal != 0 || leftVertical != 0 || right || down)
//                    {
//                        Deselect();
//                        if (leftHorizontal == 1 || right)
//                            CurrentSelected++;
//                        else if (leftVertical == -1 || down)
//                            CurrentSelected += 2;
//                        Select();
//                    }
//                }

//                // Input method text
//                else if (CurrentSelected == 1)
//                {
//                    if (leftHorizontal != 0 || leftVertical != 0 || left || down)
//                    {
//                        Deselect();
//                        if (leftHorizontal == -1 || left)
//                            CurrentSelected--;
//                        else if (leftVertical == -1 || down)
//                            CurrentSelected++;
//                        Select();
//                    }
//                }

//                // Control text
//                else
//                {
//                    if (leftVertical != 0 || down || up)
//                    {
//                        Deselect();
//                        if (leftVertical == -1 || down)
//                            CurrentSelected++;
//                        else if (leftVertical == 1 || up)
//                            CurrentSelected--;
//                        Select();
//                    }
//                }

//                // When the player selects a player, input method, or control to change, start the respective coroutine
//                if (InputManager.GetButtonDown("A", CurrentPlayer))
//                {
//                    if (CurrentSelected == 0)
//                        StartCoroutine(SwitchPlayer());
//                    else if (CurrentSelected == 1)
//                        StartCoroutine(SwitchInput());
//                    else
//                        StartCoroutine(RemapControl());
//                }
//            }
//        }

//     Change the text color of the currently selected control
//    private void Select()
//    {
//        foreach (Text t in controls[CurrentSelected].GetComponentsInChildren<Text>())
//            t.color = selectedColor;
//    }

//     Change the text color of the deselected control
//    private void Deselect()
//    {
//        foreach (Text t in controls[CurrentSelected].GetComponentsInChildren<Text>())
//            t.color = neutralColor;
//    }

//        // Coroutine for remapping controls
//        IEnumerator RemapControl()
//        {
//            yield return new WaitUntil(() => InputManager.GetButtonUp("A", CurrentPlayer) == true);

//            Text[] text = controls[CurrentSelected].GetComponentsInChildren<Text>();

//            isRemapping = true;
//            Color c = controls[CurrentSelected].GetComponentInChildren<Image>().color;
//            XboxController.XboxButton button = XboxController.XboxButton.None;
//            KeyCode key = KeyCode.None;
//            text[1].text = "";

//            IInputPlayer player = CurrentPlayer;
//            if (player.InputMethod == InputManager.InputMethod.Keyboard)
//            {
//                while ((key = InputManager.GetNextKeyboardButton()) == KeyCode.None)
//                {
//                    c.a = Mathf.Abs(Mathf.Sin(Time.time * 3));
//                    controls[CurrentSelected].GetComponentInChildren<Image>().color = c;
//                    yield return new WaitForEndOfFrame();
//                }
//                if (key != KeyCode.None)
//                {
//                    InputManager.RemapKeyboardButton(text[0].text, key, player);
//                    text[1].text = key.ToString();
//                }
//            }

//            else if (player.InputMethod == InputManager.InputMethod.XboxController)
//            {
//                while ((button = InputManager.GetNextXboxButton(player)) == XboxController.XboxButton.None)
//                {
//                    c.a = Mathf.Abs(Mathf.Sin(Time.time * 3));
//                    controls[CurrentSelected].GetComponentInChildren<Image>().color = c;
//                    yield return new WaitForEndOfFrame();
//                }
//                if (button != XboxController.XboxButton.None)
//                {
//                    InputManager.RemapXboxButton(text[0].text, button, player);
//                    text[1].text = button.ToString();
//                }
//            }

//            yield return new WaitUntil(() => InputManager.GetButtonUp(text[0].text, player));

//            c.a = 0;
//            controls[CurrentSelected].GetComponentInChildren<Image>().color = c;
//            isRemapping = false;
//        }

//        // Coroutine for selecting the current players input mapping
//        IEnumerator SwitchPlayer()
//        {
//            yield return new WaitUntil(() => InputManager.GetButtonUp("A", CurrentPlayer) == true);

//            isRemapping = true;
//            Color color = playerText.color;
//            color.a = 0;

//            IInputPlayer player = CurrentPlayer;
//            int previousPlayer = currentPlayer;

//            while (InputManager.GetButton("A", player) == false)
//            {
//                color.a = Mathf.Abs(Mathf.Sin(Time.time * 3));
//                playerText.color = color;

//                if (leftHorizontal == -1 || left)
//                {
//                    if (currentPlayer == 0)
//                        currentPlayer = InputManager.Players.Count - 1;
//                    else
//                        currentPlayer--;
//                    playerText.text = "Player " + (currentPlayer + 1);
//                }

//                else if (leftHorizontal == 1 || right)
//                {
//                    if (currentPlayer == InputManager.Players.Count - 1)
//                        currentPlayer = 0;
//                    else
//                        currentPlayer++;
//                    playerText.text = "Player " + (currentPlayer + 1);
//                }

//                yield return new WaitForEndOfFrame();
//            }

//            yield return new WaitUntil(() => InputManager.GetButtonUp("A", CurrentPlayer) == true);

//            color.a = 1;
//            playerText.color = color;
//            isRemapping = false;

//            if (previousPlayer != currentPlayer)
//                UpdateText();
//        }

//        // Coroutine for switching the input method
//        IEnumerator SwitchInput()
//        {
//            yield return new WaitUntil(() => InputManager.GetButtonUp("A", CurrentPlayer) == true);

//            isRemapping = true;
//            Color color = inputText.color;
//            color.a = 0;

//            IInputPlayer player = CurrentPlayer;
//            int previousMethod = (int)player.InputMethod;
//            int newMethod = previousMethod;

//            while (InputManager.GetButton("A", player) == false)
//            {
//                color.a = Mathf.Abs(Mathf.Sin(Time.time * 3));
//                inputText.color = color;

//                if (previousMethod == (int)InputManager.InputMethod.Keyboard && InputManager.ControllersConnected == 0) { }
//                else
//                {
//                    if (leftHorizontal == -1 || left)
//                        newMethod--;
//                    else if (leftHorizontal == 1 || right)
//                        newMethod++;
//                }

//                newMethod = Mathf.Clamp(newMethod, 0, (int)Enum.GetValues(typeof(InputManager.InputMethod)).Cast<InputManager.InputMethod>().Max());
//                inputText.text = "Input Type: " + ((InputManager.InputMethod)newMethod).ToString();
//                yield return new WaitForEndOfFrame();
//            }

//            yield return new WaitUntil(() => InputManager.GetButtonUp("A", player) == true);

//            color.a = 1;
//            inputText.color = color;
//            isRemapping = false;

//            if (previousMethod != newMethod)
//            {
//                player.InputMethod = (InputManager.InputMethod)newMethod;
//                UpdateText();
//            }
//        }

//     Updates canvas text
//    private void UpdateText()
//    {
//        foreach (GameObject o in controls)
//            if (o != playerText.gameObject && o != inputText.gameObject)
//                Destroy(o);

//        controls.Clear();
//        controls.Add(playerText.gameObject);
//        controls.Add(inputText.gameObject);

//        playerText.text = "Player " + (currentPlayer + 1);
//        inputText.text = "Input Type: " + CurrentPlayer.InputMethod.ToString();

//        IInputPlayer player = CurrentPlayer;

//        if (player.InputMethod == InputManager.InputMethod.Keyboard)
//            UpdateKeyboardControlText();
//        else if (player.InputMethod == InputManager.InputMethod.XboxController)
//            UpdateXboxControlText();
//    }

//     Updates the text on the canvas with the axis and button keys/values of the current player keyboard input mapping
//    private void UpdateKeyboardControlText()
//    {
//        List<Dictionary<string, KeyCode>> keyboardDictionary = InputManager.GetKeyboardDictionary(currentPlayer + 1);
//        int j = 0;
//        for (int i = 0; i < keyboardDictionary.Count; i++)
//        {
//            foreach (KeyValuePair<string, KeyCode> kp in keyboardDictionary[i])
//            {
//                controls.Add(Instantiate(controlPrefab, i == 0 ? axisPanel.transform : buttonPanel.transform));
//                Text[] text = controls[j + 2].GetComponentsInChildren<Text>();
//                for (int k = 0; k < text.Length; k++)
//                {
//                    if (k == 0)
//                        text[k].text = kp.Key.ToString();

//                    else if (k == 1)
//                        text[k].text = kp.Value.ToString();

//                    text[k].color = neutralColor;
//                }
//                j++;
//            }
//        }
//    }

//     Updates the text on the canvas with the axis and button keys/values of the current player xbox controller input mapping 
//    private void UpdateXboxControlText()
//    {
//        Dictionary<string, XboxController.XboxAxis> xboxAxisDictionary = InputManager.GetXboxAxisDictionary(currentPlayer + 1);
//        Dictionary<string, XboxController.XboxButton> xboxButtonDictionary = InputManager.GetXboxButtonDictionary(currentPlayer + 1);

//        int i = 0;
//        foreach (KeyValuePair<string, XboxController.XboxAxis> kp in xboxAxisDictionary)
//        {
//            controls.Add(Instantiate(controlPrefab, axisPanel.transform));
//            Text[] text = controls[i + 2].GetComponentsInChildren<Text>();
//            for (int j = 0; j < text.Length; j++)
//            {
//                if (j == 0)
//                    text[j].text = kp.Key.ToString();
//                else if (j == 1)
//                    text[j].text = kp.Value.ToString();

//                text[j].color = neutralColor;
//            }
//            i++;
//        }

//        foreach (KeyValuePair<string, XboxController.XboxButton> kp in xboxButtonDictionary)
//        {
//            controls.Add(Instantiate(controlPrefab, buttonPanel.transform));
//            Text[] text = controls[i + 2].GetComponentsInChildren<Text>();
//            for (int j = 0; j < text.Length; j++)
//            {
//                if (j == 0)
//                    text[j].text = kp.Key.ToString();
//                else if (j == 1)
//                    text[j].text = kp.Value.ToString();

//                text[j].color = neutralColor;
//            }
//            i++;
//        }
//    }

//        // Update input values from the current player
//        private void UpdateInput()
//        {
//            IInputPlayer player = CurrentPlayer;

//            leftHorizontal = InputManager.GetAxisDown("LeftHorizontal", player);
//            leftVertical = InputManager.GetAxisDown("LeftVertical", player);
//            up = InputManager.GetButtonDown("Up", player);
//            right = InputManager.GetButtonDown("Right", player);
//            down = InputManager.GetButtonDown("Down", player);
//            left = InputManager.GetButtonDown("Left", player);
//        }
//}