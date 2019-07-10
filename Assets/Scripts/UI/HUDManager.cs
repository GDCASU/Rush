using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager singleton;
    public List<Image> liveIcons=new List<Image>();

    public Sprite activeLive;
    public Sprite inactiveLive;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    
    public void setLiveCount(int nextLives) {
        for(int i = 0; i < liveIcons.Count; i++)
            liveIcons[i].sprite = (i < nextLives) ? activeLive : inactiveLive;
    }
}
