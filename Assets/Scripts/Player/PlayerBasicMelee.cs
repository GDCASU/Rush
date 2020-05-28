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
        public int startupFrames; // frames between hitting the button and actually having the attack come out
        public int windowFrames;// the frames before leaving the combo
        public float radius;
        public float damage;
        public float pushMultiplier; // percent of run speed
        public string animationName;
        public string chargeName;
        public Sprite effectSprite;
    }
    public List<attackData> comboData;
    public bool reflectBullets = true;
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
    public bool inAttackStun { // time before attack animations end
        get=>(combo > 0 && (comboData[combo-1].startupFrames + comboData[combo-1].stunFrames) > framesSinceAttack);
    }
	private int framesSinceAttack=9999;
    private bool attackBuffered=false;
	void Update ()
    {
        framesSinceAttack++;
        if ( attackBuffered || InputManager.GetButtonDown(PlayerInput.PlayerButton.Melee, player) )
        {
            if(!inAttackStun) {
                if(combo >= comboData.Count || framesSinceAttack > comboData[combo].windowFrames) combo = 0; // reset combo counter
                attackBuffered = false;
                framesSinceAttack = 0;
                mov.faceMouse();
                anim.tryNewAnimation(comboData[combo].chargeName, false, comboData[combo].startupFrames, false, () => applyAttack());
                curVel = mov.overRideFacing.normalized * mov.speed * comboData[combo].pushMultiplier;
                mov.freezeInPlace=true;
                combo++;
            }
            else {
                if(combo < 3) attackBuffered = true;
            }
        }
        else if(combo > 0 && framesSinceAttack > comboData[combo-1].startupFrames && inAttackStun ) applyMovement();
	}
    private Vector2 curVel;
    void applyMovement () {
        rb.MovePosition(rb.position+curVel);
        curVel/=1.5f;
    }
    public float overlapRange = 0.8f; // 1 means the player is on the edge of the hitbox, 0 means the center
    public SlashLazy slashEffect;
    void applyAttack() {
        anim.tryNewAnimation(comboData[combo-1].animationName, false, comboData[combo-1].stunFrames, false, ()=> {anim.tryNewAnimation("Idle", true); mov.freezeInPlace=false;} );

        // Get the list of game objects in our hitbox
        
        var hits=Physics2D.CircleCastAll((Vector2)transform.position + new Vector2(0f, -.5f) + mov.facing * comboData[(combo > 0) ? combo - 1 : combo].radius * overlapRange * 1.5f, comboData[(combo > 0) ? combo - 1 : combo].radius, mov.overRideFacing, 0f);

        // Damage all hit enemies
        var enemies = hits?.Where(x => x.transform.tag == "Enemy")?.Select(e => e.transform.GetComponent<EnemyHealth>());
        foreach(EnemyHealth enemy in enemies) enemy.takeDamage(comboData[combo-1].damage);

        // If we want we can reflect all the bullets
        if(reflectBullets){
            var bullets = hits?.Where(x => x.transform.tag == "Bullet")?.Select(b => b.transform.GetComponent<Bullet>());
            foreach(Bullet bullet in bullets) reflectBulletFromPlayer(bullet);
        }

        slashEffect.Enable(comboData[combo-1].effectSprite, this.transform.position, mov.facing);
    }
    void OnDrawGizmos ()
    {
        // put code here for drawing hitboxes
        if( Application.isPlaying ) Gizmos.DrawWireSphere((Vector2)transform.position +new Vector2(0f, -.5f) + mov.facing*comboData[(combo>0) ? combo-1 : combo].radius*overlapRange * 1.5f, comboData[(combo>0)?combo-1 : combo].radius);  
    }

    private void reflectBulletFromPlayer(Bullet bullet){
        bullet.speed*=1.5f; 
        bullet.MoveVector = (bullet.transform.position - transform.position).normalized * bullet.speed; 
        bullet.setFacingToVector(bullet.MoveVector); 
        bullet.hostile = false;
    }
}
