using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoltendwarfA1 : BossAction
{
    const float tau = 2 * Mathf.PI;
    public Sprite bulletSprite;
    public Color bulletTint = Color.white;
    public Vector3 scale = new Vector3(0.05F, 0.05F, 0.05F);
    private GameObject hammer;
    public Collider2D col;
    public int windUpAnimation;
    public int hammerIsOut;
    public float ArcOffset;
    public float bulletSpeed;
    public float returnSpeed;
    public float bulletArc = tau;
    public float colliderRadius=0.5f;
    private float currentOffset;
    private bool isBack;


    public void OnEnable() => StartCoroutine(HammerThrow());
    IEnumerator HammerThrow()
    {
        actionRunning = true;
        for (int x = 0; x < windUpAnimation; x++)
        {
            if (x < 40) transform.Rotate(new Vector3(0, 0, 1.125f), Space.Self);
            else transform.Rotate(new Vector3(0, 0, -2.25f), Space.Self);
            yield return new WaitForEndOfFrame();
        }
        col.enabled = true;
        spawnBullet();
        for (int x = 0; x < hammerIsOut; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(HammerBack());
    }
    public void spawnBullet()
    {
        Vector2 VectorToPlayer = PlayerHealth.singleton.transform.position - transform.position;
        ArcOffset = (float)(Vector2.SignedAngle(Vector2.right, VectorToPlayer) * Math.PI / 180.0);
        float angle = ArcOffset + currentOffset;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        hammer = BulletPool.rent();
        hammer.name = "Hammer";
        hammer.GetComponent<Bullet>().onScreenDestroy = false;
        hammer.transform.position = transform.position;
        hammer.GetComponent<Bullet>().Init(direction, angle * (float)180.0 / Mathf.PI , bulletSpeed, Bullet.MoveFunctions.None, Bullet.SpawnFunctions.None, bulletSprite, bulletTint, true, colliderRadius, new List<float>());
        hammer.transform.localScale = scale;
    }
    public void Update()
    {
        currentOffset %= bulletArc;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hammer") isBack = true;
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hammer") isBack = false;
    }
    IEnumerator HammerBack()
    {
        if (hammer != null) hammer.GetComponent<Bullet>().enabled = false;
        Vector2 VectorToDwarf = transform.position - hammer.transform.position;
        ArcOffset = (float)(Vector2.SignedAngle(Vector2.right, VectorToDwarf) * Math.PI / 180.0);
        float angle = ArcOffset + currentOffset;

        Vector2 MoveVector = VectorToDwarf.normalized * returnSpeed;
        while (!isBack && GameObject.Find("Hammer"))
        {
            Vector2 m = MoveVector * Time.deltaTime;
            hammer.transform.localPosition = hammer.transform.localPosition + new Vector3(m.x, m.y, 0);

            yield return new WaitForEndOfFrame();
        }
        col.enabled = false;
        Destroy(hammer);
        actionRunning = false;
    }
}
