using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicShot : MonoBehaviour {

    public bool charging = false;
    public float decreaseRate;
    public float chargeUpFrames;
	public float bulletSpeed = 1;
    public Sprite bulletSprite;
    public float scale = 0.25f;
    public float shootFrameReset;
    private IInputPlayer player;
    private float charge=0;
    private bool shot = true;
    
    private PlayerMovement pMovement;

     void Start()
    {
        player = GetComponent<IInputPlayer>();   
        pMovement = GetComponent<PlayerMovement>();
    }
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
                sp.GetComponent<Bullet>().Init(direction, 0, bulletSpeed, Bullet.MoveFunctions.Spin, Bullet.SpawnFunctions.None, bulletSprite, Color.white, false, 0.15f,null);

                sp.transform.localScale = Vector3.one * scale;
                shot = true;
            }
            resetShot();
        }
        if (GetComponent<PlayerHealth>().inv) resetShot();
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Shoot, player) && !charging)
        {
            charging = true;
            if (charge < 0)
            {
                charge = 0;
            }

        } 
        if (InputManager.GetButton(PlayerInput.PlayerButton.Shoot, player) && charging )
        {
            if (!shot)charge++;
        }
        
        if(!charging && charge>=1)charge=charge-decreaseRate;
    }
    void resetShot()
    {
        charge = (!shot && !GetComponent<PlayerHealth>().inv) ? charge : 0;
        shot = false;
        charging = false;
    }
}
