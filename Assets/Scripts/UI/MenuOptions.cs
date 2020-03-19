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
    public GameObject controls;
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
        panels.Add(controls);
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
            if (!isTitle)
            {
                if (isPaused) Resume();
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
        panels[current].SetActive(false);
        current = 4;
        panels[current].SetActive(true);
    }
    
    public void Pause()
    {
        if(current==4)
        isPaused = true; panels[0].SetActive(true);
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
}
