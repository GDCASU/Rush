﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

/// <summary>
/// Holds basic player movement functions
/// Any advanced movement or upgrades should be seperated out into seperate classes
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private IInputPlayer player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<IInputPlayer>();
    }

    /// <summary>
    /// A shmup has no physics so we actually prefer to have our movement slowed if we drop frames
    /// Therefore we use the Update for movement instead of fixed update which is tied to time
    /// </summary>
    void Update ()
    {
        faceMouse();
        CheckMovementInput();

    }

    void CheckMovementInput()
    {
        Vector3 input = new Vector3(InputManager.GetAxis(PlayerAxis.MoveHorizontal,player), InputManager.GetAxis(PlayerAxis.MoveVertical, player), 0);
        Vector3 direction = input.normalized;
        Vector2 velocity = direction * speed;
        // Update location of the player checking collisions
        rb.MovePosition(rb.position+velocity);
    }
    void faceMouse()
    {
        
        Vector3 position = Input.mousePosition;
        position.z = -10f;
        position = Camera.main.ScreenToWorldPoint(position);
        Vector2 direction = new Vector2((position.x - transform.position.x), (position.y - transform.position.y));
        transform.up = -direction;
    }
}