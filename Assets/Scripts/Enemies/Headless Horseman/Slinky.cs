using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slinky : BossAction
{
    public float speed;
    public float increment;
    private bool thisAction = false;
    private bool move = false;
    Vector3 pos;
    Vector3 playerPos;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() => StartCoroutine(Boing());

    IEnumerator Boing()
    {
        actionRunning = true;
        thisAction = true;
        playerPos = PlayerHealth.singleton.transform.position;
        Vector3 vector = playerPos - transform.position;
        float distance = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);

        flip();

        move = true;
        pos = transform.position;
        float init = speed;
        while (move)
        {
            transform.position = transform.position + new Vector3(speed * 0.2f * vector.x / distance, speed * 0.2f * vector.y / distance, 0);
            speed = speed - increment;
            yield return new WaitForEndOfFrame();
        }
        while (Mathf.Abs((Mathf.Abs(pos.x) - Mathf.Abs(transform.position.x))) >= 0.1 || Mathf.Abs((Mathf.Abs(pos.y) - Mathf.Abs(transform.position.y))) >= 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * 0.2f);
            speed = speed + increment;
            yield return new WaitForEndOfFrame();
        }
        speed = init;
        thisAction = false;
        actionRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (thisAction)
        {
            move = false;         
        }
    }
    
    void flip()
    {
        if (transform.position.x > playerPos.x)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }
}
