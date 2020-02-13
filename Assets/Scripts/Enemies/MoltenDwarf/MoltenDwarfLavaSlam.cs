using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfLavaSlam : MoltenDwarfParent
{
    private float timeBtwAttack; //timer
    public float startTimeBtwAttack = 4.0f; //attack delay
    public float hitmarkerSpeed = 2.5f;

    public Transform attackPOS; //position for attack area of effect
    public float attackRange = 2.0f; //radius of attack
    public LayerMask playerToHit; //checks for objects with a layers (dropdown menu)

    private bool hitmarkerTrack; //does the hitmarker track directly on the player or not
    private bool canMove;   //is the hitmarker allowed to move
    private float step;     //variable for Area of Effect movement speed

    // Start is called before the first frame update
    void Start()
    {
        hitmarkerTrack = true; //stays on player
        canMove = false; //false because it is allowed to move (will change later)

        dwarfAnim = GetComponent<Animator>();
        dwarfTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = myPlayer.transform.position; //player position

        if(!hitmarkerTrack) //if it can't be directly on player, MoveTowards player instead
        {   
            HitmarkerFollow(myPlayer.transform);
        }
        else if(hitmarkerTrack && !canMove) //stay directly on player
        {
            attackPOS.position = playerPosition;
        }

        if (timeBtwAttack <= 0)  //checks if dwarf is allowed to slam again
        {
            LavaSlamSet(1);
        }
        else
        {
            timeBtwAttack -= Time.deltaTime; //decreased timer so dwarf can attack
        }
    }

    private void OnDrawGizmosSelected() //for attack area of effect
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPOS.position, attackRange);
    }

    void DamagePlayer()
    {
        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPOS.position, attackRange, playerToHit); //gets objects with layer "default"
        try
        {
            playerToDamage.GetComponent<PlayerHealth>().takeDamage(); //check for PlayerHealth script on objects collided
        }
        catch { }
    }

    void LavaSlamSet(int attacking) //for setting attack bool for animation
    {
        if (attacking == 1)
        {
            dwarfAnim.SetBool("lavaSlam", true); // activates attack animation
        }

        if (attacking == 0)
        {
            timeBtwAttack = startTimeBtwAttack; //resets attack timer
            HitmarkerTrackSet(1);   //lets marker track player
            canMove = false; //marker can track
            dwarfAnim.SetBool("lavaSlam", false);
        }
    }

    void HitmarkerFollow(Transform trackPOS)
    {
        step = hitmarkerSpeed * Time.deltaTime; //movement speed

        attackPOS.position = Vector2.MoveTowards(attackPOS.position, trackPOS.position, step);
    }

    void HitmarkerTrackSet(int set) //sets if marker stays directly on player or not
    {
        if(set == 1)
        {
            hitmarkerTrack = true;
        }
        else
        {
            hitmarkerTrack = false;
            canMove = true;
        }
    }
}
