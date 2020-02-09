using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfMove : MoltenDwarfParent
{
    

    private float speed;
    private float step;

    private SpriteRenderer dwarfSprite;

    // Start is called before the first frame update
    void Start()
    {
        dwarfSprite = GetComponent<SpriteRenderer>();   //get the sprite renderer
        dwarfTransform = GetComponent<Transform>();

        speed = 1.5f;  //movement speed set
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = myPlayer.transform.position;  //position of player to use

        if (Vector2.Distance(this.transform.position, playerPosition) > 3.0f)  //distance between dwarf and player
        {
            MoveToPlayer(playerPosition);
        }

        CheckFacing();
    }

    void MoveToPlayer(Vector2 target)
    {
        step = speed * Time.deltaTime; //movement speed

        transform.position = Vector2.MoveTowards(this.transform.position, playerPosition, step); //moves dwarf to player
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
