using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds basic player movement functions
/// Any advanced movement or upgrades should be seperated out into seperate classes
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private SpriteRenderer sp;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// A shmup has no physics so we actually prefer to have our movement slowed if we drop frames
    /// Therefore we use the Update for movement instead of fixed update which is tied to time
    /// </summary>
    void Update ()
    {
        CheckMovementInput();
        flipSprite();
	}

    public Vector2 facing = Vector2.up;

    void CheckMovementInput()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 direction = input.normalized;
        if(direction != Vector3.zero) facing = direction;

        Vector2 velocity = direction * speed;
        // Update location of the player checking collisions
        rb.MovePosition(rb.position+velocity);
    }

    void flipSprite () {
       sp.flipX = (facing.x < 0);
    }
}
