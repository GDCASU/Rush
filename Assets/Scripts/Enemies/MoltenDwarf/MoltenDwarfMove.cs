using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfMove : MonoBehaviour
{
    public GameObject myPlayer;

    private float speed;
    private float step;

    private Vector2 playerPosition;

    private Animator dwarfAnim;
    private SpriteRenderer dwarfSprite;

    // Start is called before the first frame update
    void Start()
    {
        dwarfAnim = GetComponent<Animator>();   //get the animator
        dwarfSprite = GetComponent<SpriteRenderer>();   //get the sprite renderer

        speed = 1.5f;  //movement speed set
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = myPlayer.transform.position;  //position of player to use

        if(Vector2.Distance(this.transform.position, playerPosition) > 3.0f)  //distance between dwarf and player
        {
            MoveToPlayer(playerPosition);
            dwarfAnim.SetBool("isAttacking", false); // activates attack animation
        }
        else
        {
            dwarfAnim.SetBool("isAttacking", true);
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
            dwarfSprite.flipX = true;
        }
        else //if player is on left side of dwarf
        {
            dwarfSprite.flipX = false;
        }
    }
}
