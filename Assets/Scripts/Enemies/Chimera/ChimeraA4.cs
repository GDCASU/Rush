using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChimeraA4 : BossAction
{
    public int windUpFrames;
    public int lookingFrames;
    public int attackingFrames;

    public BoxCollider2D claw1;
    public BoxCollider2D claw2;
    public BoxCollider2D claw3;
    //public BoxCollider2D claw3;

    void OnEnable()
    {
        StartCoroutine("ClawAttack");
    }

    IEnumerator ClawAttack()
    {
        actionRunning = true;

        for (int i = 0; i < lookingFrames; i++)
        {
            var VectorToPlayer = (Vector2)(PlayerHealth.singleton.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.identity;
            transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(-Vector2.right, VectorToPlayer)));
            yield return new WaitForEndOfFrame();
        }
        for (int x = 0; x < windUpFrames; x++)
        {
            transform.Rotate(new Vector3(0, 0, -45f / windUpFrames), Space.Self);           //This makes the sprite rotate 45 degreees counter clokwise in 40 frames
            yield return new WaitForEndOfFrame();
        }
        for (int x = 0; x < attackingFrames; x++)
        {
            if (x < (attackingFrames / 3))  transform.Rotate(new Vector3(0, 0, (67.5f / (attackingFrames / 3f))), Space.Self);                     //This makes the sprite rotate 67.5 degreees clokwise in 20 frames                                                                                                   //This lets the bulletSpawner spawn a bullet every 1/3 of those 20 frames or avery 6.66 frames
            else if (x >= (attackingFrames / 3) && x <(attackingFrames / 3) * 2)    transform.Rotate(new Vector3(0, 0, -(45f / (attackingFrames / 3f))), Space.Self);
            else  transform.Rotate(new Vector3(0, 0, (22.5f / (attackingFrames / 3f))), Space.Self);


            if (x == 0) claw1.enabled = true;
            else if (x == (attackingFrames / 3)-1)
            {
                claw1.enabled = false;
                claw2.enabled = true;
            }
            else if (x == (attackingFrames / 3)*2 -1)
            {
                claw2.enabled = false;
                claw3.enabled = true;
            }
            yield return new WaitForEndOfFrame();
        }
        claw3.enabled = false;


        actionRunning = false;                                                                  //Lets the BossBehaviorController its done with the action
    }
}
