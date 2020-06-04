using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ControllerRemap : MonoBehaviour
{
    PlayerInput.PlayerButton action;
    public GameObject canvas;
    public GameObject text;
    IInputPlayer player;
    public string keyName;
    bool remaping;

    public void Update()
    {
        if (remaping)
        {
            if (InputManager.xboxControllers.FirstOrDefault().AnyButtonDown)
            {
                SetButton(InputManager.GetNextXboxButton(player));
                remaping = false;
            }

        }
    }
    public void Remaping() => StartCoroutine(timerRemaping());
    public void SetButton(XboxController.XboxButton passed)
    {
        List<string> xboxCodes = canvas.GetComponent<MenuOptions>().xboxCodes;
        player = GameObject.Find("Player").GetComponentInChildren<IInputPlayer>();
        foreach (string xKey in xboxCodes)
        {
            if (passed.ToString() == xKey)
            {
                return;
            }
        }
        InputManager.RemapXboxButton(action, passed, player);
        xboxCodes.Remove(keyName);
        keyName = passed.ToString();
        xboxCodes.Add(keyName);
        GetComponentInChildren<Text>().text = keyName;
    }
    public IEnumerator timerRemaping()
    {
        for (int x = 0; x < 10; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        remaping = true;
    }
}
