﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfHammerSlam : MoltenDwarfParent
{
    [SerializeField]
    private float attackDistance = 5.0f;

    [SerializeField]
    private Transform attackPOS; //position for attack area of effect

    [SerializeField]
    private float attackRange = 2.0f; //radius of attack

    [SerializeField]
    private LayerMask playerToHit; //checks for objects with a layers (dropdown menu)

    void OnEnable()
    {
        actionRunning = true;


        dwarfAnim = GetComponent<Animator>();
        dwarfAnim.enabled = true;
        dwarfTransform = GetComponent<Transform>();

        StartCoroutine("slam");
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
    IEnumerator slam()
    {
        while (Vector2.Distance(transform.position, myPlayer.transform.position) > attackDistance)
        {
            yield return new WaitForFixedUpdate();
        }
        isAttackingSet(1);
        for(int x=0;x<45;x++)
        {
            yield return new WaitForEndOfFrame();
        }
        dwarfAnim.enabled = false;
        actionRunning = false;
    }

    private void OnDrawGizmosSelected() //for attack area of effect
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPOS.position, attackRange);
    }
}

