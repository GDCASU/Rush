using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector2 MoveVector;
	float Rotation;
	Action MoveFunc; 
	public bool hostile; //0=hurts enemys 1=hurts player
    public bool solid; //collides with enviroment walls
	public void Init(Vector2 dir, float rot, float spd, MoveFunctions act, Sprite spr, Color color, bool enemy, float colliderRadius = 0.5f) {
		SpriteRenderer sp = GetComponent<SpriteRenderer>();
		MoveVector = dir.normalized * spd; 
		Rotation = rot; 
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0,0,rot));
		MoveFunc = getMoveFunction(act); sp.sprite = spr;
		sp.color = color; hostile = enemy;
        GetComponent<CircleCollider2D>().radius = colliderRadius;
	}

	void Update () {
		if (!Utils.IsOnScreen(gameObject)) BulletDestroy ();
		
		Vector2 m = MoveVector * Time.deltaTime;
		transform.localPosition = transform.localPosition+new Vector3(m.x,m.y,0);
        MoveFunc?.Invoke();
    }

    public void BulletDestroy () => BulletPool.recall(this.gameObject);
    
    /// <summary>
    /// To add to the move functions write an Action (void method takes no params)
    /// and then add the name to the list here 
    /// that way you can set it from the editor
    /// </summary>
    public enum MoveFunctions: int {
        None,
        LeftSine,
        RightSine,
        Spin
    }

    public Action getMoveFunction (MoveFunctions f){
        switch (f) {
            case MoveFunctions.LeftSine: return LeftSine;
            case MoveFunctions.RightSine: return RightSine;
            case MoveFunctions.Spin: return Spin;
            default: return null;
        }
    }

    //Move functions 
    public void LeftSine () => MoveVector = MoveVector * -Mathf.Sin(transform.localPosition.y * 25F) * 0.85F;
    public void RightSine () => MoveVector = MoveVector * Mathf.Sin(transform.localPosition.y * 25F) * 0.85F;
    public void Spin () => transform.Rotate(new Vector3(0,0,5f)); 
}
