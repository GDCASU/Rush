using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfLavaSlam : MoltenDwarfParent
{
    public float hitmarkerSpeed = 8f;

    public Transform attackPOS; //position for attack area of effect
    public float attackRange = 5.0f; //radius of attack
    public LayerMask playerToHit; //checks for objects with a layers (dropdown menu)

    private bool hitmarkerTrack; //does the hitmarker track player or not
    private float step;     //variable for Area of Effect movement speed

    void OnEnable()
    {
        actionRunning = true;

        dwarfAnim = GetComponent<Animator>();
        dwarfTransform = GetComponent<Transform>();

        attackPOS.position = myPlayer.transform.position;

        LavaSlamSet(1);

        StartCoroutine(HitmarkerFollow());
    }

    void DamagePlayer()
    {
        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPOS.position, attackRange, playerToHit); //gets objects with layer "default"
        try
        {
            playerToDamage.GetComponent<PlayerHealth>().takeDamage(); //check for PlayerHealth script on objects collideda
        }
        catch { }

        HitmarkerTrackSet(0);
    }

    void LavaSlamSet(int attacking) //for setting attack bool for animation
    {
        if (attacking == 1)
        {
            dwarfAnim.SetBool("lavaSlam", true); // activates attack animation
            HitmarkerTrackSet(1);
        }

        if (attacking == 0)
        {
            dwarfAnim.SetBool("lavaSlam", false);
            actionRunning = false;
        }
    }

    IEnumerator HitmarkerFollow()
    {
        while(hitmarkerTrack)
        {
            playerPosition = myPlayer.transform.position;

            step = hitmarkerSpeed * Time.deltaTime; //movement speed

            attackPOS.position = Vector2.MoveTowards(attackPOS.position, playerPosition, step);

            yield return new WaitForEndOfFrame();
        }
    }

    void HitmarkerTrackSet(int canTrack)
    {
        if(canTrack == 1)
        {
            hitmarkerTrack = true;
        }
        else
        {
            hitmarkerTrack = false;
        }
    }

    private void OnDrawGizmosSelected() //for attack area of effect
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPOS.position, attackRange);
    }
}
