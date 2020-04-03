using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    IInputPlayer player;
    public bool isPaused;
    private bool isTitle;
    public GameObject hud;
    public GameObject mainMenu;
    public GameObject bossSelect;
    public GameObject pause;
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
    public GameObject wipsButton;

    public List<GameObject> panels;
    private int current;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject.GetComponentInChildren<IInputPlayer>();
        panels.Add(hud);
        panels.Add(mainMenu);
        panels.Add(bossSelect);
        panels.Add(pause);
        panels.Add(settings);
        panels.Add(graphics);
        panels.Add(sound);
        panels.Add(controls);
        panels.Add(controller);
        panels.Add(keyboard);
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
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Pause, player))
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
        if(!isTitle)panels[3].SetActive(false);
        panels[4].SetActive(true);                          //4 is the seetings tabs
        toGraphics();
    }
    public void backToPause()
    {
        panels[current].SetActive(false);
        current = 0;
        panels[3].SetActive(true);
        panels[current].SetActive(true);
    }
    public void Pause()
    {
        isPaused = true;
        panels[3].SetActive(true);
        Time.timeScale = 0;
        current = 0;
    }
    public void Resume()
    {
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
        if (current >=7)
        {
            //panels[7].SetActive(false);
            panels[(panels[8].activeSelf == true) ? 9 : 8].SetActive(false);
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
        print((panels[8].activeSelf == true) ? 9 : 8);
        if (current>=7) panels[(panels[8].activeSelf == true) ?8: 9].SetActive(false);
        panels[current].SetActive(false);
        panels[4].SetActive(false);
    }
}
