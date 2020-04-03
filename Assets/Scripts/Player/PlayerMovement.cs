﻿using System.Collections;
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
    public float speed;
    private Rigidbody2D rb;
    private IInputPlayer player;
    private SpriteRenderer sp;
    private AnimationController anim;
    private bool collided;
    public Vector2 velocity;
    public bool inControl=true; // player has direct control over movement
    public bool freezeInPlace; // player cannot move at all

    private void Start()
    {
        inControl=true;
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<IInputPlayer>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<AnimationController>();
    }

    /// <summary>
    /// A shmup has no physics so we actually prefer to have our movement slowed if we drop frames
    /// Therefore we use the Update for movement instead of fixed update which is tied to time
    /// </summary>
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            playerKnockback(-transform.right, 2f, .5f, 1f);
        }
        if(!freezeInPlace) CheckMovementInput();
        checkFaceMouse();
        flipSprite();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collided = true;
    }

    public Vector2 overRideFacing = Vector2.zero;
    public Vector2 facing = Vector2.up;
    private Vector2 lastVel;
    void CheckMovementInput()
    {
        Vector3 input = new Vector3(InputManager.GetAxis(PlayerAxis.MoveHorizontal,player), InputManager.GetAxis(PlayerAxis.MoveVertical, player), 0);
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
       sp.flipX = (facing.x < 0);
    }
    void playerKnockback(Vector3 direction, float knockBackSeconds, float stunSeconds, float speed)
    {
        StartCoroutine(knockback(direction,knockBackSeconds,stunSeconds,speed));
    }
    IEnumerator knockback(Vector3 direction, float knockBackSeconds,float stunSeconds, float speed)
    {
        float framerate=60;
        WaitForSeconds ws = new WaitForSeconds(1 / framerate);
        GetComponent<PlayerMovement>().freezeInPlace = true;
        GetComponent<PlayerBasicMelee>().enabled = false;
        GetComponent<PlayerBasicShot>().enabled = false;
        GetComponent<PlayerDash>().enabled = false;
        int x = 0;
        while(x < knockBackSeconds * framerate && !collided)
        {
            rb.MovePosition(transform.position+(direction * speed));
            x++;
            yield return ws;
        }
        for (int y = 0; y < stunSeconds * framerate; y++) yield return ws;
        GetComponent<PlayerMovement>().freezeInPlace = false;
        GetComponent<PlayerBasicMelee>().enabled = true;
        GetComponent<PlayerBasicShot>().enabled = true;
        GetComponent<PlayerDash>().enabled = true;
        collided = false;
    }
    void LateUpdate () {
        // Update animations
        if(lastVel.magnitude > 0)
            anim.tryNewAnimation("PlayerRun", true);
        else
            anim.tryNewAnimation("PlayerIdle", true);
            
    }
}
