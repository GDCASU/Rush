using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;
using System.Linq;

/// <summary>
/// Holds basic player movement functions
/// Any advanced movement or upgrades should be seperated out into seperate classes
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    public bool stayInPlace;
    public bool winWalk;
    public float speed;
    private float scaleX;
    private Rigidbody2D rb;
    private IInputPlayer player;
    public GameObject sprite;
    private SpriteRenderer sp;
    public AnimationController anim;
    public MenuOptions mo;
    public Vector2 velocity;
    public bool inControl=true; // player has direct control over movement
    public bool freezeInPlace; // player cannot move at all

    private void Start()
    {
        scaleX = sprite.transform.localScale.x;
        inControl =true;
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<IInputPlayer>();
        //sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<AnimationController>();
    }

    /// <summary>
    /// A shmup has no physics so we actually prefer to have our movement slowed if we drop frames
    /// Therefore we use the Update for movement instead of fixed update which is tied to time
    /// </summary>
    void Update ()
    {
        if(!freezeInPlace && !winWalk) CheckMovementInput(false);
        checkFaceMouse();
        flipSprite();
	}
    public Vector2 overRideFacing = Vector2.zero;
    public Vector2 facing = Vector2.up;
    private Vector2 lastVel;
    public void CheckMovementInput(bool win)
    {
        Vector3 input = win? new Vector3(0,1,0): new Vector3(InputManager.GetAxis(PlayerAxis.MoveHorizontal,player), InputManager.GetAxis(PlayerAxis.MoveVertical, player), 0);
        Vector3 direction = input.normalized;
        if(direction != Vector3.zero) facing = direction;

        if(inControl) velocity = direction * speed;
        // Update location of the player checking collisions
        rb.MovePosition(rb.position+velocity);

        lastVel = velocity;
    }
    public void faceMouse()
    {
        Vector3 position = Input.mousePosition;
        position.z = 10;
        position = Camera.main.ScreenToWorldPoint(position);
        overRideFacing = (position - transform.position).normalized;
    }
    public void checkFaceMouse() {
        if(overRideFacing != Vector2.zero){
            facing = overRideFacing;
            overRideFacing = Vector2.zero;
        }
    }
    void flipSprite () {
        if (!GetComponent<PlayerDash>().inDash)
        {
            if (facing.x < 0) sprite.transform.localScale = new Vector3(scaleX, sprite.transform.localScale.y, sprite.transform.localScale.z);
            else sprite.transform.localScale = new Vector3(0 - scaleX, sprite.transform.localScale.y, sprite.transform.localScale.z);
            //sp.flipX = (facing.x < 0);
        }

    }

    public void StopMovement()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerDash>().enabled = false;
        GetComponent<PlayerBasicShot>().enabled = false;
        GetComponent<PlayerBasicMelee>().enabled = false;
        anim.tryNewAnimation("Idle", true);
    }
    public void RestoreMovement()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerDash>().enabled = true;
        GetComponent<PlayerBasicShot>().enabled = true;
        GetComponent<PlayerBasicMelee>().enabled = true;
    }
    void LateUpdate () {
        // Update animations
        if (!GetComponent<PlayerBasicShot>().charging)
        {
            if (lastVel.magnitude > 0)
                anim.tryNewAnimation("Running", true);
            else
                anim.tryNewAnimation("Idle", true);
        }
        
    }
}
