using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    IInputPlayer player;
    public PlayerMovement pm;
    public bool isPaused;
    public bool won;
    public bool dead;
    private bool isTitle;
    public List<string> keyboardCodes;
    public List<string> xboxCodes;
    public GameObject winUI;
    public GameObject deathUI;
    public GameObject hud;
    public GameObject mainMenu;
    public GameObject bossSelect;
    public GameObject pause;
    public GameObject backDrop;
    public GameObject settings;
    public GameObject graphics;
    public GameObject sound;
    public GameObject controls;
    public GameObject controller;
    public GameObject keyboard;
    public GameObject chimeraButton;
    public GameObject dwarfButton;
    public GameObject horsemanButton;
    public GameObject ratbossButton;
    public GameObject wispButton;

    public List<GameObject> panels;
    private int current;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject.GetComponentInChildren<IInputPlayer>();
        keyboardCodes.Add("W");
        keyboardCodes.Add("S");
        keyboardCodes.Add("D");
        keyboardCodes.Add("A");
        foreach (InputManager.Button b in GameObject.Find("Managers").GetComponent<InputManager>().buttons)
        {
            keyboardCodes.Add(b.keyboardButton.ToString());
            xboxCodes.Add(b.xboxButton.ToString());
        }
        isTitle = (SceneManager.GetActiveScene().name == "Title") ? true : false;
        if (isTitle)
        {
            current = 1;
            panels[current].SetActive(true);
        }
        else
        {
            current = 0;
            panels[current].SetActive(true);
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (dead || won)
        {
            backToPause();
            Resume();
            if (won) winUI.SetActive(true);
            else deathUI.SetActive(true);
            pm.StopMovement();           
        }
        else if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Pause, player))
        {
            if (current >= 4)
            {
                backFromSettings();
            }
            if (!isTitle)
            {
                if (isPaused)
                    if (current == 4) backToPause();
                    else Resume();
                else Pause();
            }
            else backToMain();
        } 
    }
    
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void switchToBossLevel()
    {
        SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void switchToBoss()
    {
        panels[current].SetActive(false);
        current = 2;
        panels[current].SetActive(true);

    }
    public void backToMain()
    {
        panels[current].SetActive(false);
        current = 1;
        panels[current].SetActive(true);
    }
    public void Settings()
    {
        if (!isTitle)panels[3].SetActive(false);
        panels[4].SetActive(true);                          //4 is the seetings tabs
        backDrop.SetActive(true);
        toGraphics();
    }
    public void backToPause()
    {
        panels[current].SetActive(false);
        current = 0;
        if (!dead)
        {
            panels[3].SetActive(true);
            panels[current].SetActive(true);
        } 
    }
    public void Pause()
    {
        pm.StopMovement();
        isPaused = true;
        panels[3].SetActive(true);
        Time.timeScale = 0;
        current = 0;
    }
    public void Resume()
    {
        pm.RestoreMovement();
        isPaused = false;
        panels[3].SetActive(false);
        Time.timeScale = 1;
        current = 0;
    }
    public void Controls()
    {
        panels[current].SetActive(false);
        current = 5;
        panels[current].SetActive(true);
    }
    public void fullsScreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow) Screen.fullScreenMode = FullScreenMode.Windowed;
        else Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
    public void toGraphics()
    {
        if (current >=6)
        {
            panels[7].SetActive(false);
            panels[(panels[8].activeSelf == true) ? 8 : 9].SetActive(false);
        }
        panels[current].SetActive(false);
        current = 5;
        panels[current].SetActive(true);
    }
    public void toSound()
    {
        if (current >= 7)
        {
            //panels[7].SetActive(false);
            panels[(panels[8].activeSelf == true) ? 8 : 9].SetActive(false);
        }
        panels[current].SetActive(false);
        current = 6;
        panels[current].SetActive(true);
    }
    public void toControls()
    {
        panels[current].SetActive(false);
        current = 7;
        panels[current].SetActive(true);
        panels[8].SetActive(true);
    }
    public void betweenControllers()
    {
        //print((panels[8].activeSelf));
        if (panels[8].activeSelf == true)
        {
            panels[8].SetActive(false);
            panels[9].SetActive(true);
        }
        else
        {
            panels[9].SetActive(false);
            panels[8].SetActive(true);
        }
        
    }
    public void backFromSettings()
    {
        if (current>=7) panels[(panels[8].activeSelf == true) ?8: 9].SetActive(false);
        backDrop.SetActive(false);
        panels[current].SetActive(false);
        current = 4;
        panels[current].SetActive(false);
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
