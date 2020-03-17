using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraA6 : BossAction
{
    public float legs_up;
    public float legs_down;
    public float attackingFrames;

    public GameObject wavePrefab;
    // Start is called before the first frame update
    void OnEnable() => StartCoroutine("waves");

    IEnumerator waves()
    {
        actionRunning = true;

        for (int i = 0; i < legs_up; i++)
        {
            transform.Rotate(new Vector3(0, 0, -45f / legs_up), Space.Self);           //This makes the sprite rotate 45 degreees counter clokwise in 40 frames
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < legs_down; i++)
        {
            transform.Rotate(new Vector3(0, 0, 45f / legs_down), Space.Self);           //This makes the sprite rotate 45 degreees counter clokwise in 40 frames
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < attackingFrames; i++)
        {
            if (i % 40 == 0)
            {
                GameObject temp = GameObject.Instantiate(wavePrefab, transform.position, Quaternion.identity);
                temp.transform.up = -(Vector2)(PlayerHealth.singleton.transform.position - transform.position).normalized;
                temp.transform.localScale = new Vector3(3f,1,1);
            }
            if (i < attackingFrames / 3)
            {
                if(i<(attackingFrames / 12)) transform.Rotate(new Vector3((-75f / (attackingFrames / 12)), 0, 0), Space.Self);
                else transform.Rotate(new Vector3((75f / (attackingFrames / 12)/3), 0, 0), Space.Self);
            }
            else if (i < (attackingFrames / 3)*2)
            {

                if (i < (attackingFrames / 12) + (attackingFrames / 3))transform.Rotate(new Vector3((75f / (attackingFrames / 12)), 0, 0), Space.Self);
                else transform.Rotate(new Vector3((-75f / (attackingFrames / 12) / 3), 0, 0), Space.Self);
            }
            else if (i < attackingFrames)
            {
                if (i < (attackingFrames / 12) + (attackingFrames / 3) * 2) transform.Rotate(new Vector3((-75f / (attackingFrames / 12)), 0, 0), Space.Self);
                else transform.Rotate(new Vector3((75f / (attackingFrames / 12) / 3), 0, 0), Space.Self);
            }
            yield return new WaitForEndOfFrame();
        }

        actionRunning = false;
    }
}
