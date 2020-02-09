using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfHammerSlam : MoltenDwarfParent
{
    private float timeBtwAttack; //timer
    public float startTimeBtwAttack = 0.3f; //attack delay

    public Transform attackPOS; //position for attack area of effect
    public float attackRange = 2.0f; //radius of attack
    public LayerMask playerToHit; //checks for objects with a layers (dropdown menu)

    // Start is called before the first frame update
    void Start()
    {
        dwarfAnim = GetComponent<Animator>();
        dwarfTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = myPlayer.transform.position; //player position

        if (timeBtwAttack <= 0)  //checks if dwarf is allowed to slam again
        {
            HammerSlam();
        }
        else
        {
            timeBtwAttack -= Time.deltaTime; //decreased timer so dwarf can attack
        }
    }

    void HammerSlam()
    {
        if(Vector2.Distance(this.transform.position, playerPosition) < 3.0f) //distance between dwarf and player
        {
            isAttackingSet(1); //attack anim
        }
        else
        {
            isAttackingSet(0); //turn attack off
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

        timeBtwAttack = startTimeBtwAttack; //resets attack timer
    }

    void isAttackingSet(int attacking) //for setting attack bool for animation
    {
        if (attacking == 1)
            dwarfAnim.SetBool("isAttacking", true); // activates attack animation

        if (attacking == 0)
            dwarfAnim.SetBool("isAttacking", false);
    }
}
