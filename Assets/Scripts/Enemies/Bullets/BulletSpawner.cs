using System.Collections;
using UnityEngine;
using System;
public class BulletSpawner : MonoBehaviour {
	const float tau = 2*Mathf.PI;
	private GameObject bulletTemplate;
	public float bulletSpeed;
	public int bulletAmount;
	public float bulletArc = tau;
	public float ArcOffset = 0;
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

	void Awake () => bulletTemplate = Resources.Load("bullet") as GameObject;
	void Update () {
		counter += wave;
		currentOffset += spin*Mathf.Cos(counter);
		currentOffset %= bulletArc;
	}
	void Start() => StartCoroutine(SpawnLoop());

	IEnumerator SpawnLoop() { while(true) {
		for (int i = 0; i < bulletAmount; i++) {
			float angle = ArcOffset + (bulletArc/bulletAmount)*i - bulletArc/4 + currentOffset;
			Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            
			var sp = BulletPool.rent();
			sp.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			sp.GetComponent<Bullet>().Init(direction, 0,bulletSpeed, moveFunc,bulletSprite, bulletTint, true);
		    
        	sp.transform.localScale = scale;
		}
		yield return new WaitForSeconds(bulletfrequency);
	}}
}
