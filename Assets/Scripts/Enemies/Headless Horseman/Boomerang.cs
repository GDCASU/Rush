using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : BossAction
{
    public int boomerangFrames;
    public GameObject arena;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() => StartCoroutine(Bangerang());

    IEnumerator Bangerang()
    {
        actionRunning = true;
        System.Random rng = new System.Random();
        int wall = rng.Next(4);
        Transform random = arena.transform.GetChild(wall);
        int adder = rng.Next(10);
        int sign = 1;
        if (rng.Next(2) == 1)
            sign = -1;
        for (int i = 0; i < boomerangFrames; i++)
        {
            if (((Mathf.Abs(transform.position.x - (sign * adder + random.position.x)) > 5f) || (Mathf.Abs(transform.position.y - random.position.y) > 5f)) && wall > 1)
            {
                print("shit");
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(sign * adder + random.position.x,random.position.y), speed*0.5f);
                yield return new WaitForEndOfFrame();
            }
            else if (((Mathf.Abs(transform.position.x - random.position.x) > 5f) || (Mathf.Abs(transform.position.y - (sign * adder + random.position.y)) > 5f)) && wall <= 1)
            {
                print("fuck");
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(random.position.x, (sign * adder + random.position.y)), speed*0.5f);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                int temp = wall;
                wall = rng.Next(4);
                while (temp == wall)
                    wall = rng.Next(4);
                adder = rng.Next(20);
                sign = 1;
                if (rng.Next(2) == 1)
                    sign = -1;
                random = arena.transform.GetChild(wall);
            }
        }
        actionRunning = false;
    }
}
