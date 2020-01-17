using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicShot : MonoBehaviour {

    public bool charging = false;
    public float chargeUpFrames;
	public float bulletSpeed = 1;
    public Sprite bulletSprite;
    public float scale = 0.25f;
    public float shootFrameReset;
    private IInputPlayer player;
    private float charge=0;
    
    private PlayerMovement pMovement;

     void Start()
    {
        player = GetComponent<IInputPlayer>();   
        pMovement = GetComponent<PlayerMovement>();
    }
    private float timer = 0;
    public void Update () 
    {
        if (InputManager.GetButtonUp(PlayerInput.PlayerButton.Shoot, player) && charging)
        {
            if (charge > chargeUpFrames)
            {
                pMovement.faceMouse();
                Vector2 direction = pMovement.overRideFacing;

                var sp = BulletPool.rent();
                sp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                sp.GetComponent<Bullet>().Init(direction, 0, bulletSpeed, Bullet.MoveFunctions.Spin, Bullet.SpawnFunctions.None, bulletSprite, Color.white, false, 0.15f);

                sp.transform.localScale = Vector3.one * scale;
            }
            resetShot();
        }
        if (GetComponent<PlayerHealth>().inv) resetShot();
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Shoot, player) && !charging && timer < 0)
        {
            charging = true;
            charge = 0;
        } 
        if (InputManager.GetButton(PlayerInput.PlayerButton.Shoot, player) && charging )
        {
            charge++;
        }
        
        timer--;
    }
    void resetShot()
    {
        charging = false;
        timer = (charge < chargeUpFrames) ? charge : shootFrameReset;
        charge = 0;
    }
}
