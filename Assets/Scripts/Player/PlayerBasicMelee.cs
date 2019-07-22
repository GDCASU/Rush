using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Animator anim;
    private PlayerMovement mov;    
    void Start ()
    {
        combo = 1;
        col=GetComponent<BoxCollider2D>();
        anim=GetComponent<Animator>();
        mov=GetComponent<PlayerMovement>();
        col.enabled=false;
	}
    public bool inAttackStun {
        get=>(attackStunFrames[combo] > framesSinceAttack);
    }
	private int framesSinceAttack=9999;
	void Update ()
    {
        framesSinceAttack++;
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Melee, player) && !inAttackStun)
        {
            framesSinceAttack = 0;
            if(combo != 0 && framesSinceAttack < comboFrames[combo]) combo++;
            else combo = 0;
            if (combo > comboFrames.Count) combo = 0;

            anim.Play("PlayerMeleeAttack");

            var hits=Physics2D.CircleCastAll(transform.position, attackRadius[combo],mov.facing,0f).Where(x => x.transform.tag == "Enemy").Select(e => e.transform.GetComponent<EnemyHealth>());
            foreach(EnemyHealth enemy in hits){
                enemy.takeDamage(attackDamage[combo]);
            }
            if(combo==0)combo++;
        }
	}
    void LateUpdate () {
        // put code here for drawing hitboxes
    }
}
