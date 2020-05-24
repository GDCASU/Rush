using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager singleton;

    public Image healthBar;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    
    public void setLiveCount(float nextLives) 
    {
        healthBar.fillAmount = nextLives / 10f;
    }
}
