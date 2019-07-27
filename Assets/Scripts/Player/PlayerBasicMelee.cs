using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class PlayerBasicMelee : MonoBehaviour
{
    private IInputPlayer player;
    private BoxCollider2D col;
    [Serializable]
    public struct attackData {
        public int stunFrames; //the frames for each swing
        public int windowFrames;// the frames before leaving the combo
        public float radius;
        public float damage;
        public float pushMultiplier; // percent of run speed
        public string animationName;
    }
    public List<attackData> comboData;
    private int combo;
    private AnimationController anim;
    private PlayerMovement mov;    
    private Rigidbody2D rb;
    void Start ()
    {
        col=GetComponent<BoxCollider2D>();
        anim=GetComponent<AnimationController>();
        mov=GetComponent<PlayerMovement>();
        rb=GetComponent<Rigidbody2D>();
	}
    public bool inAttackStun {
        get=>(combo > 0 && comboData[combo-1].stunFrames > framesSinceAttack);
    }
	private int framesSinceAttack=9999;
	void Update ()
    {
        framesSinceAttack++;
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Melee, player) && !inAttackStun)
        {
            if(combo >= comboData.Count || framesSinceAttack > comboData[combo].windowFrames) combo = 0;
            framesSinceAttack = 0;
            mov.faceMouse();
            anim.tryNewAnimation(comboData[combo].animationName, false, comboData[combo].stunFrames, false, ()=> {anim.tryNewAnimation("PlayerIdle", true);} );
            var hits=Physics2D.CircleCastAll((Vector2)transform.position+mov.overRideFacing*comboData[combo].radius, comboData[combo].radius, mov.overRideFacing, 0f)?.Where(x => x.transform.tag == "Enemy")?.Select(e => e.transform.GetComponent<EnemyHealth>());
            foreach(EnemyHealth enemy in hits){
                 enemy.takeDamage(comboData[combo].damage);
            }
            curVel = mov.overRideFacing.normalized * mov.speed * comboData[combo].pushMultiplier;
            combo++;
        }
        else if(inAttackStun) applyMovement();
	}
    private Vector2 curVel;
    void applyMovement () {
        rb.MovePosition(rb.position+curVel);
        curVel/=1.5f;
    }
    void OnDrawGizmos () {
        // put code here for drawing hitboxes
        if( Application.isPlaying ) Gizmos.DrawWireSphere((Vector2)transform.position+mov.facing*comboData[(combo>0) ? combo-1 : combo].radius,comboData[(combo>0)?combo-1 : combo].radius);  
    }
}
