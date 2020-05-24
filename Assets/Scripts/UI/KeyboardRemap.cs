using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardRemap : MonoBehaviour
{
    PlayerInput.PlayerButton action;
    public GameObject canvas;
    public GameObject text;
    IInputPlayer player;
    public string keyName;
    bool remaping;

    private void Start()
    {
        switch (keyName)
        {
            case "LeftShift":
                action = PlayerInput.PlayerButton.Dash;
                break;
            case "Mouse0":
                action = PlayerInput.PlayerButton.Melee;
                break;
            case "H":
                action = PlayerInput.PlayerButton.Potion;
                break;
            case "Mouse1":
                action = PlayerInput.PlayerButton.Shoot;
                break;
            default:
                break;
        }
        GetComponentInChildren<Text>().text = keyName;
    } 
    private void Update()
    {
        if (remaping)
        {
            if (Input.anyKey)
            {
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(vKey))
                    {
                        SetButton(vKey);
                        remaping = false;
                    }
                }
            }
        }
    }

    public void Remaping() => remaping = true;
    public void SetButton(KeyCode passed)
    {
        List<string> keyboardCodes = canvas.GetComponent<MenuOptions>().keyboardCodes;
        player = GameObject.Find("Player").GetComponentInChildren<IInputPlayer>();
        foreach (string key in keyboardCodes)
        {
            if (passed.ToString() == key)
            {
                return;
            }
        }
        InputManager.RemapKeyboardButton(action, passed, player);
        keyboardCodes.Remove(keyName);
        keyName = passed.ToString();
        keyboardCodes.Add(keyName);
        text.GetComponentInChildren<Text>().text = keyName;
    }

}
