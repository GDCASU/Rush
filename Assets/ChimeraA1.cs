using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraA1 : MonoBehaviour
{
    public int windupFrames;
    public int followFrames;
    public int chargeFrames;

    public BoxCollider2D box;
    void Start()
    {
        StartCoroutine("charge");
    }

    IEnumerator charge()
    {
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
        Vector3 chargePosition = PlayerHealth.singleton.transform.position;

        box.enabled = true;
        for (int x = 0; x < chargeFrames; x++)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, chargePosition, .75f);
            yield return new WaitForEndOfFrame();
        }
        box.enabled = false;



    }
}
