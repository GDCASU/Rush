using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfHammerSlam : MoltenDwarfParent
{
    public float attackDistance = 5.0f;
    public Transform attackPOS; //position for attack area of effect
    public float attackRange = 2.0f; //radius of attack
    public LayerMask playerToHit; //checks for objects with a layers (dropdown menu)

    void OnEnable()
    {
        actionRunning = true;

        dwarfAnim = GetComponent<Animator>();
        dwarfTransform = GetComponent<Transform>();

        if (Vector2.Distance(transform.position, myPlayer.transform.position) > attackDistance)
        {
            isAttackingSet(1);
        }
        else
        {
            actionRunning = false;
        }
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

    void isAttackingSet(int attacking) //for setting attack bool for animation
    {
        if (attacking == 1)
            dwarfAnim.SetBool("isAttacking", true); // activates attack animation

        if (attacking == 0)
        {
            dwarfAnim.SetBool("isAttacking", false);
            actionRunning = false;
        }
    }

    private void OnDrawGizmosSelected() //for attack area of effect
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPOS.position, attackRange);
    }
}
