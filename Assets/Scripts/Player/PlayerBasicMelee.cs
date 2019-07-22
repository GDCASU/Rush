using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class PlayerBasicMelee : MonoBehaviour
{
    private IInputPlayer player;
    private BoxCollider2D col;
    public List<int> attackStunFrames = new List<int>(); //the frames for each swing
    public List<int> comboFrames = new List<int>(); // the frames before leaving the combo
    public List<int> attackRadius = new List<int>(); 
    public List<int> attackDamage = new List<int>(); 
    private int combo;
    private AnimationController anim;
    private PlayerMovement mov;    
    void Start ()
    {
        col=GetComponent<BoxCollider2D>();
        anim=GetComponent<AnimationController>();
        mov=GetComponent<PlayerMovement>();
	}
    public bool inAttackStun {
        get=>(combo > 0 && attackStunFrames[combo-1] > framesSinceAttack);
    }
	private int framesSinceAttack=9999;
	void Update ()
    {
        framesSinceAttack++;
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Melee, player) && !inAttackStun)
        {
            if(combo >= comboFrames.Count || framesSinceAttack > comboFrames[combo]) combo = 0;
            framesSinceAttack = 0;
            
            anim.tryNewAnimation("PlayerMeleeAttack", false, attackStunFrames[combo], false, ()=> {anim.tryNewAnimation("PlayerIdle", true);} );
            var hits=Physics2D.CircleCastAll(transform.position, attackRadius[combo], mov.facing, 0f)?.Where(x => x.transform.tag == "Enemy")?.Select(e => e.transform.GetComponent<EnemyHealth>());
            foreach(EnemyHealth enemy in hits){
                 enemy.takeDamage(attackDamage[combo]);
            }
            combo++;
        }
	}
    void LateUpdate () {
        // put code here for drawing hitboxes
    }
}
