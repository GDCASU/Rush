using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraA1 : BossAction
{
    public int windupFrames;
    public int followFrames;
    public int chargeFrames;
    public float lungeSpeed;
    public bool collided = false;

    public BoxCollider2D box;
    private void OnEnable()
    {
                StartCoroutine("charge");
    }

    private void OnTriggerEnter2D(Collider2D collision)
        {
        if (collision.tag == "Player" && !collision.GetComponent<PlayerDash>().inDash) collided = true;
        }
    IEnumerator charge()
    {
        actionRunning = true;
        for (int x = 0; x < windupFrames; x++)
        {
            if (x < (windupFrames / 2)) transform.Rotate(new Vector3(0, 0, (-45f / (windupFrames / 2f))), Space.Self);
            else transform.Rotate(new Vector3(0, 0, (45f / (windupFrames / 2f))), Space.Self);
            yield return new WaitForEndOfFrame();
        }
        
        for (int x = 0; x < followFrames; x++)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, PlayerHealth.singleton.transform.position, .1f);
            var VectorToPlayer = (Vector3)(PlayerHealth.singleton.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.identity;
            transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(-Vector2.right, VectorToPlayer)));
            yield return new WaitForEndOfFrame();
            //add an i
        }
        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        for (int x = 0; x < 20; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        GetComponent<SpriteRenderer>().color = new Color(255, 255,255 );
        Vector3 chargePosition = (Vector3)(PlayerHealth.singleton.transform.position - transform.position).normalized;

        box.enabled = true;
        int y= 0;
        while(y<chargeFrames && !collided)
        {
            transform.position +=  chargePosition* lungeSpeed;
            y++;
            yield return new WaitForEndOfFrame();
        }
        box.enabled = false;
        collided = false;
        actionRunning = false;
    }
}
