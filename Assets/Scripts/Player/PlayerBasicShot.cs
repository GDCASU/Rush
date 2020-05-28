using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicShot : MonoBehaviour 
{
    public bool charging = false;
    public GameObject chargedLight;
    public float decreaseRate;
    public float chargeUpFrames;
    public float bulletSpeed = 1;
    public Sprite bulletSprite;
    public float scale = 0.25f;
    public float shootFrameReset;
    private IInputPlayer player;
    private float charge = 0;
    private bool shot = false;
    private float originalSpeed;
    public float chargingSpeed;

    private PlayerMovement pMovement;

     void Start()
    {
        player = GetComponent<IInputPlayer>();   
        pMovement = GetComponent<PlayerMovement>();
        originalSpeed=pMovement.speed;
    }

    public void Update()
    {
        if (charging) pMovement.anim.tryNewAnimation("Range_charging", true);
        if (InputManager.GetButtonUp(PlayerInput.PlayerButton.Shoot, player) && charging)
        {
            if (charge > chargeUpFrames)
            {
                pMovement.anim.tryNewAnimation("Range_release", false, 15, false);
                pMovement.faceMouse();
                Vector2 direction = pMovement.overRideFacing;

                var sp = BulletPool.rent();
                sp.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                sp.GetComponent<Bullet>().Init(direction, 0, bulletSpeed, Bullet.MoveFunctions.None, Bullet.SpawnFunctions.None, bulletSprite, Color.green, false, 0.15f, null);
                sp.transform.right = -direction;
                sp.transform.localScale = Vector3.one * scale;
                shot = true;
            }
            resetShot();
        }
        if (GetComponent<PlayerHealth>().inv) resetShot();
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Shoot, player) && !charging)
        {
            pMovement.speed =chargingSpeed;
            pMovement.faceMouse();
            charging = true;
            if (charge < 0)
            {
                charge = 0;
            } 
        }
        if (InputManager.GetButton(PlayerInput.PlayerButton.Shoot, player) && charging)
        {
            if (!shot) charge++;
            if (charge > chargeUpFrames)
            {
                chargedLight.GetComponent<Light>().enabled = true;
                chargedLight.GetComponent<Light>().color = Color.green;
            }
        }
        if (!charging && charge >= 1) charge = charge - decreaseRate;
    }
    void resetShot()
    {
        pMovement.speed = originalSpeed;
        chargedLight.GetComponent<Light>().enabled = false;
        charge = (!shot && !GetComponent<PlayerHealth>().inv) ? charge : 0;
        shot = false;
        charging = false;
    }
}
