using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashLazy : MonoBehaviour {
    SpriteRenderer rend;
    public void Awake()=> rend = GetComponent<SpriteRenderer>();
	public void Enable (Sprite s, Vector3 pos, Vector2 dir) {
        this.enabled = true;
        rend.enabled = true;
        rend.sprite = s;

        transform.position = pos;
        transform.rotation = Quaternion.identity;
        transform.Rotate ( 0, 0 , (float)(Vector2.SignedAngle(Vector2.right, dir)) );
        transform.position += (Vector3)dir*1.5f;

        StartCoroutine(disable());
    }
    public IEnumerator disable () {
        for(int i = 0; i < 5; i++) { 
            transform.position += transform.forward/20;
            yield return GameManager.singleton.ws;
        }
        rend.enabled = false;
        this.enabled = false;
    }
}
