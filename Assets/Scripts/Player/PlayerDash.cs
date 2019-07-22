using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput;

public class PlayerDash : MonoBehaviour {

    private IInputPlayer player;
    public int dashFrames;
    private PlayerMovement mov;
    private SpriteRenderer sp;
    private bool inDash = false;
	void Start () {
		mov= GetComponent<PlayerMovement>();
        sp = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if (!inDash && InputManager.GetButtonDown(PlayerButton.Dash, player))
        {
            inDash = true;
            mov.speed = mov.speed * 2;
            StartCoroutine(dash());
        }
	}
    
    public IEnumerator  dash()
    {
        sp.color = new Color(0.5f, 0.5f, 0.5f, 1);
        GetComponent<PlayerHealth>().inv = true;
        for (int i = dashFrames; i > 0; i--)
        {
            yield return new WaitForEndOfFrame();
        }
        sp.color = new Color(1,1, 1, 1);
        mov.speed = mov.speed / 2;
        GetComponent<PlayerHealth>().inv = false;
        inDash = false;
    }
}
