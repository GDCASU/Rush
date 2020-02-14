using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraA5 : BossAction
{
    public int lightingFrames;
    public GameObject lightingSpot;
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;
    public int lps;

    private System.Random rng = new System.Random();
    private void OnEnable()
    {
        StartCoroutine("lighting");
    }

    IEnumerator lighting()
    {
        actionRunning = true;
        Vector2 location=Vector3.zero;
        for (int i = lightingFrames-1; i >=0; i--)
        {
            if (i % 12 == 0)
            {
                //Not so fun way
                //float x = (float)rng.Next(minX, maxX);
                //float y = (float)rng.Next(minY, maxY);
                //GameObject temp = GameObject.Instantiate(lightingSpot, new Vector3(x, y, -.1f), Quaternion.identity);

                location = pickSpot(i);
                GameObject temp = GameObject.Instantiate(lightingSpot, new Vector3(location.x, location.y, -.1f), Quaternion.identity);

            }

            yield return new WaitForEndOfFrame();
        }
        print("ended");
        actionRunning = false;

    }
    Vector2 pickSpot(int i)
    {
        int x;
        int y;
        int y1 = rng.Next((maxY / lps) * (int)Mathf.Floor(i / 60), (maxY / lps) * ((int)Mathf.Floor(i / 60)+1));
        int y2 = rng.Next((minY / lps) * ((int)Mathf.Floor(i / 60) + 1), (minY / lps) * (int)Mathf.Floor(i / 60));
        if (Random.value < 0.5f) y = y1;
        else y = y2;
        x = rng.Next(minX, maxX);
        return new Vector2(x,y);
    }
}
