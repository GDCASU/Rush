using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

public class PlayerDash : MonoBehaviour {

    private IInputPlayer player;
    public int dashFrames;
    public int extraInvFrames;
    private PlayerMovement mov;
    private SpriteRenderer sp;
    public float dashSpeedMultiplier = 1.5f;
    public bool inDash = false;
    
    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
		mov= GetComponent<PlayerMovement>();
        sp = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if (!inDash && InputManager.GetButtonDown(PlayerButton.Dash, player))
        {
            inDash = true;
            mov.inControl = false;
            StartCoroutine(dash());
        }
	}
    
    public IEnumerator  dash()
    {
        sp.color = new Color(0.5f, 0.5f, 0.5f, 1);
        GetComponent<PlayerHealth>().inv = true;
        mov.velocity = Vector2.zero;
        mov.faceMouse();

        var dashVel = mov.overRideFacing.normalized * mov.speed * dashSpeedMultiplier;
        var orgRot = transform.rotation;
        for (int i = dashFrames; i > 0; i--)
        {
            rb.MovePosition(rb.position+dashVel);
            transform.Rotate(new Vector3(0,0,25));
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = orgRot;
        mov.inControl=true;
        StartCoroutine(endInv());
    }

    public IEnumerator endInv()
    {
        for (int i = dashFrames; i > 0; i--) yield return new WaitForEndOfFrame();
        sp.color = new Color(1,1, 1, 1);
        GetComponent<PlayerHealth>().inv = false;
        inDash = false;
    }
}
