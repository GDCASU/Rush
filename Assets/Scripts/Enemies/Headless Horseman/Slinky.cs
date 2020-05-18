using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slinky : BossAction
{
    public float speed;
    private bool thisAction = false;
    private bool move = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() => StartCoroutine(Boing());

    IEnumerator Boing()
    {
        actionRunning = true;
        thisAction = true;
        Vector3 playerPos = PlayerHealth.singleton.transform.position;
        Vector3 vector = playerPos - transform.position;
        float distance = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
        if (transform.position.x > playerPos.x)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        move = true;
        while (move)
        {
            transform.position = transform.position + new Vector3(speed * 0.2f * vector.x / distance, speed * 0.2f * vector.y / distance, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (thisAction)
        {
            move = false;

            //MOVE BACK TO BODY

            thisAction = false;
            actionRunning = false;
        }
    }
}
