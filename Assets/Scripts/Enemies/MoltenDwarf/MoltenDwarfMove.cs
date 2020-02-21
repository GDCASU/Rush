using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfMove : MoltenDwarfParent
{
    public float speed = 3.0f;
    public float distanceToStop = 5.0f;
    private float step;

    private SpriteRenderer dwarfSprite;

    void OnEnable()
    {
        actionRunning = true;

        dwarfSprite = GetComponent<SpriteRenderer>();   //get the sprite renderer
        dwarfTransform = GetComponent<Transform>();

        playerPosition = myPlayer.transform.position;  //position of player to use

        StartCoroutine(MoveToPlayer(playerPosition));
    }

    private void Update()
    {
        CheckFacing();
    }

    IEnumerator MoveToPlayer(Vector2 target)
    {
        while (Vector2.Distance(this.transform.position, playerPosition) > distanceToStop)
        {
            playerPosition = myPlayer.transform.position;  //position of player to use

            step = speed * Time.deltaTime; //movement speed

            transform.position = Vector2.MoveTowards(this.transform.position, playerPosition, step); //moves dwarf to player

            yield return new WaitForEndOfFrame();
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
