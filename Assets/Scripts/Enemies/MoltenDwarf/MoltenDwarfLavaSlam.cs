using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MoltenDwarfLavaSlam : MoltenDwarfParent
{
    WaitForSeconds ws = new WaitForSeconds(1f / 60f);

    [SerializeField]
    private float hitmarkerSpeed = 8f;

    [SerializeField]
    private Transform attackPOS; //position for attack area of effect

    [SerializeField]
    private float attackRange = 5.0f; //radius of attack

    [SerializeField]
    private LayerMask playerToHit; //checks for objects with a layers (dropdown menu)

    [SerializeField]
    private GameObject rangeSprite;
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
            rangeSprite.SetActive(true);
            HitmarkerTrackSet(1);
        }

        if (attacking == 0)
        {
            dwarfAnim.SetBool("lavaSlam", false);
            rangeSprite.SetActive(false);
            actionRunning = false;
        }
    }

    //hitmarker follows player
    IEnumerator HitmarkerFollow()
    {
        //while it is following the player
        while(hitmarkerTrack)
        {
            playerPosition = myPlayer.transform.position;

            step = hitmarkerSpeed * Time.deltaTime; //movement speed

            attackPOS.position = Vector2.MoveTowards(attackPOS.position, playerPosition, step);

            yield return ws;
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

    //hitmarker
    private void OnDrawGizmosSelected() //for attack area of effect
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPOS.position, attackRange);
    }
}
