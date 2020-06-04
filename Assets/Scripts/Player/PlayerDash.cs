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
    public SpriteRenderer[] sprites;
    public float dashSpeedMultiplier = 1.5f;
    public bool inDash = false;
    
    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
		mov= GetComponent<PlayerMovement>();
        //sp = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if (!inDash && InputManager.GetButtonDown(PlayerButton.Dash, player))
        {
            inDash = true;
            mov.inControl = false;
            StartCoroutine(dash());
        }
	}

    public IEnumerator dash()
    {
        if (sprites.Length != 0) foreach (SpriteRenderer sprite in sprites)sprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
        if (GetComponent<PlayerBasicShot>().charging) mov.speed = GetComponent<PlayerBasicShot>().originalSpeed;
        //sp.color = new Color(0.5f, 0.5f, 0.5f, 1);
        GetComponent<PlayerHealth>().inv = true;
        mov.velocity = Vector2.zero;

        var dashVel = mov.facing.normalized * mov.speed * dashSpeedMultiplier;
        var orgRot = transform.rotation;
        mov.anim.tryNewAnimation("Dash", false,dashFrames);
        for (int i = dashFrames; i > 0; i--)
        {
            rb.MovePosition(rb.position+dashVel);
            //transform.Rotate(new Vector3(0,0,25));
            yield return GameManager.singleton.ws;
        }
        transform.rotation = orgRot;
        mov.inControl=true;
        StartCoroutine(endInv());
    }

    public IEnumerator endInv()
    {
        for (int i = extraInvFrames; i > 0; i--) yield return GameManager.singleton.ws;
        if (sprites.Length!=0)foreach (SpriteRenderer sprite in sprites)sprite.color = new Color(1,1,1,1);
        //sp.color = new Color(1,1, 1, 1);
        GetComponent<PlayerHealth>().inv = false;
        inDash = false;
    }
}
