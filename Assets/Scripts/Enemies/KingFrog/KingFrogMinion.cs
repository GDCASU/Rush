using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogMinion : KingFrogParent
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private float jumpSpeed = 0.25f;

    [SerializeField]
    private float jumpWaitTime = 3.0f;
    private float jumpTimer;
    private float rotateAngle;
    private float randomX;
    private float randomY;
    [SerializeField]
    private float randomMax = 1.6f;
    [SerializeField]
    private float accurateDistance = 1.0f;

    private Vector3 playerPos;
    private Vector3 randomPos;

    private void Start()
    {
        myPlayer = GameObject.FindGameObjectWithTag("Player");
        jumpTimer = 0;

        //increase minion count
        GameObject.Find("KingFrog").GetComponent<KingFrogMinionAttack>().minionCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpTimer > jumpWaitTime)
        {
            StartCoroutine(JumpToPlayer());
            jumpTimer = 0.0f;
        }
        else
        {
            jumpTimer += Time.deltaTime;
        }
    }

    IEnumerator JumpToPlayer()
    {
        RotateToPlayer();
        //while not at randomPos
        while (Vector3.Distance(transform.position, randomPos) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, randomPos, jumpSpeed);
            yield return ws;
        }
    }

    void RotateToPlayer()
    {
        playerPos = myPlayer.transform.position; //get current player position once

        randomX = playerPos.x;
        randomY = playerPos.y;

        //if frogs out of accurate distance, add randomness to jump
        if (Vector3.Distance(transform.position, myPlayer.transform.position) > accurateDistance)
        {
            //add randomness to it
            randomX += Random.Range(0, randomMax);
            randomY += Random.Range(0, randomMax);
        }
        
        randomPos = new Vector3(randomX, randomY, 1);

        //check angle compared to randomPos
        float rotateX = randomPos.x - transform.position.x;
        float rotateY = randomPos.y - transform.position.y;
        rotateAngle = Mathf.Rad2Deg * Mathf.Atan2(rotateY, rotateX); //get angle in radians then turn to degrees

        transform.rotation = Quaternion.Euler(0, 0, rotateAngle);

        CheckSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == myPlayer)
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage();
        }
    }

    //flip sprite if needed
    void CheckSprite()
    {
        if (randomPos.x > gameObject.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
    }

    private void OnDestroy()
    {
        //decrease minion count
        try
        {
            GameObject.Find("KingFrog").GetComponent<KingFrogMinionAttack>().minionCount--;
        }
        catch { }
    }
}
