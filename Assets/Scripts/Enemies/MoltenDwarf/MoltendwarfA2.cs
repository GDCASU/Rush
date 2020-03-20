using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltendwarfA2 : BossAction
{
    public BulletSpawner spawner;
    public int yeetFrames;
    public int returnFrames;

    public void OnEnable() =>StartCoroutine(TriShot()); 
    public void OnDisable() =>StartCoroutine(TriShotEnd());
    IEnumerator TriShot()
    {
        actionRunning = true;
        for (int x = 0; x < yeetFrames; x++)
        {
            if (x < 40) transform.Rotate(new Vector3(0, 0, -1.125f), Space.Self);           //This makes the sprite rotate 45 degreees counter clokwise in 40 frames
            else transform.Rotate(new Vector3(0, 0, 4.5f), Space.Self);                     //This makes the sprite rotate 90 degreees clokwise in 20 frames                        
            if (x >= 40 && x % 6 == 0)                                                      
            {                                                                               //This lets the bulletSpawner spawn a bullet every 1/3 of those 20 frames or avery 6.66 frames
                spawner.enabled = true;
                spawner.enabled = false;
            }
            yield return new WaitForEndOfFrame();

            //Alternate way for the bullets to spawn
            /*
             if (x < 50) transform.Rotate(new Vector3(0, 0, -0.9f), Space.Self);            
            else transform.Rotate(new Vector3(0, 0, 9), Space.Self);
            if(x==50) spawner.enabled = true;
            yield return new WaitForEndOfFrame();
            */
        }
        actionRunning = false;                                                                  //Lets the BossBehaviorController its done with the action
    }
    IEnumerator TriShotEnd()                                
    {
        for (int x = 0; x < returnFrames; x++)  
        {
            transform.Rotate(new Vector3(0, 0, -4.5f), Space.Self);                             //This makes the sprite rotate 45 degreees counter clokwise in 10 frames
            yield return new WaitForEndOfFrame();
        }
    }
}
