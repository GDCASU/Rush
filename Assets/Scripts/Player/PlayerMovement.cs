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
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// A shmup has no physics so we actually prefer to have our movement slowed if we drop frames
    /// Therefore we use the Update for movement instead of fixed update which is tied to time
    /// </summary>
    void Update ()
    {
        CheckMovementInput();
	}

    void CheckMovementInput()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        Vector3 velocity = direction * speed;
        // Update location of the player checking collisions
        rb.MovePosition(rb.position+velocity);
    }
}
