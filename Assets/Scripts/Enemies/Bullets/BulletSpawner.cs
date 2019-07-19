using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BulletSpawner : MonoBehaviour {
	const float tau = 2*Mathf.PI;
	private GameObject bulletTemplate;
	public float bulletSpeed;
	public int bulletAmount;
	public float bulletArc = tau;
	public float ArcOffset = 0;
    public bool offsetFacesPlayer; // makes the center of the stream face player
	private float currentOffset;
	public float bulletfrequency;
	public float spin;
	public float wave;
	private float counter;
	public Sprite bulletSprite;
	public Color bulletTint = Color.white;
	public Quaternion rotation = Quaternion.AngleAxis(0, Vector3.forward);
	public Vector3 scale = new Vector3(0.05F,0.05F,0.05F);

    public Bullet.MoveFunctions moveFunc;
    public Bullet.SpawnFunctions spawnFunc;
    public List<int> SpawnFunctionParams = new List<int>();
    public bool bulletsFaceOutward = true;
    public float colliderRadius = 0.5f;
	void Awake () => bulletTemplate = Resources.Load("bullet") as GameObject;
	void Update () {
		counter += wave;
		currentOffset += spin*Mathf.Cos(counter);
		currentOffset %= bulletArc;
	}
	void Start() => StartCoroutine(SpawnLoop());

	IEnumerator SpawnLoop() { while(true) {
        if(offsetFacesPlayer) {
            Vector2 VectorToPlayer = PlayerHealth.singleton.transform.position - transform.position;
            ArcOffset = (float) (Vector2.SignedAngle(Vector2.right, VectorToPlayer)* Math.PI/180.0);
        }
		for (int i = 1; i <= bulletAmount; i++) {
			float angle = ArcOffset + (bulletArc/bulletAmount)*((float)(i-1)-(bulletAmount-1)/(float)2.0) + currentOffset;
			Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            
			var sp = BulletPool.rent();
			sp.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			sp.GetComponent<Bullet>().Init(direction, (bulletsFaceOutward) ? angle * (float)180.0 / Mathf.PI : 0, bulletSpeed, moveFunc, spawnFunc, bulletSprite, bulletTint, true, colliderRadius,SpawnFunctionParams);
    
        	sp.transform.localScale = scale;
		}
		yield return new WaitForSeconds(bulletfrequency);
	}}
}
