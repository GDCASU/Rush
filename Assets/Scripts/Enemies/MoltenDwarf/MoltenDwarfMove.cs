using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfMove : MoltenDwarfParent
{
    WaitForSeconds ws = new WaitForSeconds(1f / 60f);

    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float distanceToStop = 5.0f;
    private float step;

    private SpriteRenderer dwarfSprite;

    void OnEnable()
    {
        actionRunning = true;

        dwarfSprite = GetComponent<SpriteRenderer>();   //get the sprite renderer
        dwarfTransform = GetComponent<Transform>();

        playerPosition = myPlayer.transform.position;  //position of player to use

        StartCoroutine("MoveToPlayer");
    }

    IEnumerator MoveToPlayer()
    {
        if (Vector2.Distance(this.transform.position, playerPosition) < distanceToStop)
        {
            for (int x = 0; x < 60; x++)
            {
                yield return ws;
            }
            CheckFacing();
        }
        else
        {
            while (Vector2.Distance(this.transform.position, playerPosition) > distanceToStop)
            {
                CheckFacing();
                playerPosition = myPlayer.transform.position;  //position of player to use

                step = speed * Time.deltaTime; //movement speed

                transform.position = Vector2.MoveTowards(this.transform.position, playerPosition, step); //moves dwarf to player
                yield return ws;
            }
        }
        
        actionRunning = false;
    }

    void CheckFacing()
    {
        if(playerPosition.x > gameObject.transform.position.x) //if player is on the RIGHT side of the dwarf
        {
            dwarfTransform.rotation = Quaternion.Euler(0, 180, 0); //flips dwarf's transform
        }
        else //if player is on left side of dwarf
        {
            dwarfTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
