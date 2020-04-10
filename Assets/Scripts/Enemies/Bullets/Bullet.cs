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
    public float speed;
    public bool onScreenDestroy=true;
	public void Init(Vector2 dir, float rot, float spd, MoveFunctions act, SpawnFunctions spwn, Sprite spr, Color color, bool enemy, float colliderRadius = 0.5f, List<float> SpawnFunctionParams = null) {
		SpriteRenderer sp = GetComponent<SpriteRenderer>();
		MoveVector = dir.normalized * spd; 
        speed = spd;
		Rotation = rot; 
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0,0,rot));
		MoveFunc = getMoveFunction(act); sp.sprite = spr;
		sp.color = color; hostile = enemy;
        if (GetComponent<CircleCollider2D>()) GetComponent<CircleCollider2D>().radius = colliderRadius;
        else Debug.Log("Missing Collider Component, please add a CircleCollider2d!");


        getSpawnFunction(spwn)?.Invoke(SpawnFunctionParams.ToArray());
	}

    public void Init(Vector2 dir, float rot, float spd, MoveFunctions act, SpawnFunctions spwn, Sprite spr, Color color, bool enemy, float colliderX = 0.5f, float colliderY = 0.5f, List<float> SpawnFunctionParams = null)
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        MoveVector = dir.normalized * spd;
        speed = spd;
        Rotation = rot;
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, 0, rot));
        MoveFunc = getMoveFunction(act); sp.sprite = spr;
        sp.color = color; hostile = enemy;
        if (GetComponent<BoxCollider2D>()) GetComponent<BoxCollider2D>().size = new Vector2(colliderX, colliderY);
        else Debug.Log("Missing Collider Component, please add a BoxCollider2D!");

        getSpawnFunction(spwn)?.Invoke(SpawnFunctionParams.ToArray());
    }


    void Update () {
		if (onScreenDestroy && !Utils.IsOnScreen(gameObject)) BulletDestroy ();
		
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
    public enum MoveFunctions: byte {
        None,
        LeftSine,
        RightSine,
        Spin,
        FollowPlayer,
    }

    public Action getMoveFunction (MoveFunctions f){
        switch (f) {
            case MoveFunctions.LeftSine: return LeftSine;
            case MoveFunctions.RightSine: return RightSine;
            case MoveFunctions.Spin: return Spin;
            case MoveFunctions.FollowPlayer: return FollowPlayer;
            default: return null;
        }
    }

    //Move functions 
    public void LeftSine () => MoveVector = MoveVector * -Mathf.Sin(transform.localPosition.y * 25F) * 0.85F;
    public void RightSine () => MoveVector = MoveVector * Mathf.Sin(transform.localPosition.y * 25F) * 0.85F;
    public void Spin () => transform.Rotate(new Vector3(0,0,5f));
    public void FollowPlayer() => StartCoroutine(FollowRoutine());

    public IEnumerator FollowRoutine()
    {
        YieldInstruction delay = new WaitForEndOfFrame();

        while(true)
        {
            MoveTowardsPlayer();

            yield return delay;
        }

        yield return null;
    }

    // Spawn functions 
    /// <summary>
    /// These are ment to happen when the bullet spawns but they can be used for delayed effects
    /// such as timed direction change or bullet self destruction
    /// </summary>
    public delegate void SpawnFunction (params float [] spawnParameters);
    public enum SpawnFunctions: byte {
        None,
        ChangeDirectionToPlayer,
        ChangeDirection,
        DestroyOnTimer,
    }

    
    public SpawnFunction getSpawnFunction (SpawnFunctions f){
        switch (f) {
            case SpawnFunctions.ChangeDirectionToPlayer: return ChangeDirectionToPlayer;
            case SpawnFunctions.DestroyOnTimer: return StartDestroyTimer;
            default: return null;
        }
    }

    //Move functions 
    public void ChangeDirectionToPlayer (float [] p) => StartCoroutine(playerDir(p[0])); // not safe

    public IEnumerator playerDir (float t) { 
        yield return new WaitForSeconds (t);
        MoveTowardsPlayer();
        GetComponent<SpriteRenderer>().color = Color.red; //optional
    }

    private void MoveTowardsPlayer()
    {
        var VectorToPlayer = (Vector2)(PlayerHealth.singleton.transform.position - transform.position).normalized;
        MoveVector = VectorToPlayer * speed;
        setFacingToVector(VectorToPlayer);
    }

    private void StartDestroyTimer(float[] p) => StartCoroutine(DestroyTimer(p[0]));

    /// <summary>
    /// Coroutine that destroys the bullet after a specified amount
    /// of time in seconds
    /// </summary>
    /// <param name="t">Time in seconds to destroy the bullet</param>
    private IEnumerator DestroyTimer(float t)
    {
        yield return new WaitForSeconds(t);

        Destroy(gameObject);
    }

    public void setFacingToVector(Vector2 dir) {
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0,0, Vector2.SignedAngle(Vector2.right, dir) ));
    }
}
