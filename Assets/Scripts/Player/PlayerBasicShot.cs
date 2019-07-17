using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicShot : MonoBehaviour {
	public float bulletSpeed = 1;
    public Sprite bulletSprite;
    public float scale = 0.25f;
    public float shootFrameReset;
    private PlayerMovement pMovement;
    public void Start () => pMovement = GetComponent<PlayerMovement>();
    private float timer = 0;
    public void Update () {
        if (Input.GetButton("Fire1") && timer < 0)
        {
            Vector2 direction = pMovement.facing;

            var sp = BulletPool.rent();
            sp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            sp.GetComponent<Bullet>().Init(direction, 0, bulletSpeed, Bullet.MoveFunctions.Spin, bulletSprite, Color.white, false, 0.15f);

            sp.transform.localScale = Vector3.one * scale;
            timer = shootFrameReset;
        }
        timer--;
    }
}
