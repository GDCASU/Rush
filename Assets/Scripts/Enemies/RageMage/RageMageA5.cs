using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageMageA5 : BossAction
{
    public int chargePunchSeconds;
    public float punchSeconds;
    public float punchSpeed;
    public float percentToPunch;
    public float cooldownSeconds;
    public float framerate;
    public bool collided;
    public bool hitWall;

    private System.Random rng = new System.Random();
    public GameObject fistMagic;
    public BoxCollider2D box;
    void OnEnable()
    {
        hitWall = false;
        collided = false;
        StartCoroutine(punch());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.GetComponent<PlayerDash>().inDash) collided = true;
        else if (collision.tag == "Wall") hitWall = true;
    }
    IEnumerator punch()
    {
        actionRunning = true;
        GameObject temp = Instantiate(fistMagic,transform);
        WaitForSeconds ws=new WaitForSeconds(1 / framerate);
        float xr = 0;
        float yr = 0;
        for (int x = 0; x < chargePunchSeconds * framerate; x++)
        {
            if (x % 5 == 0 && x < (chargePunchSeconds * framerate * percentToPunch))
            {
                xr = rng.Next(-1, 1) * (float)rng.NextDouble() / 8;
                yr = rng.Next(-1, 1) * (float)rng.NextDouble() / 8;
                temp.transform.position = new Vector3(transform.position.x + xr, transform.position.y + yr, transform.position.z + .1f);
            }
            else if(x > (chargePunchSeconds * framerate * percentToPunch)) temp.transform.position = transform.position + new Vector3(0,0,.1f);
            temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,(x/(chargePunchSeconds*framerate)));
            yield return ws;
        }
        Vector3 chargePosition = (Vector3)(PlayerHealth.singleton.transform.position - transform.position).normalized;
        box.enabled = true;
        //tag = "Enemy";
        int y = 0;
        while (y < punchSeconds*framerate && (!collided || !hitWall))
        {
            transform.position += chargePosition * punchSpeed;
            y++;
            yield return ws;
        }
        //tag = "Untagged";
        Destroy(temp);
        for (int x = 0; x < cooldownSeconds * framerate; x++)
        {
            yield return ws;
        }
        box.enabled = false;
        actionRunning = false;
    }
}
