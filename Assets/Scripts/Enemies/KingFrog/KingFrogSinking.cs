using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//********************************************************
// MAKE SURE THIS SCRIPT IS ON EACH UPDATE OF PLAYER
// GIVE EACH UPDATE OF PLAYERDEFAULT A "SinkingObject" AS CHILD
//********************************************************
public class KingFrogSinking : MonoBehaviour
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private GameObject sinkingObject;

    [SerializeField]
    private float hurtSpeed = 3.0f;
    private float maxWaterHeight;
    private float timer;

    private bool sinking;

    private Vector3 sinkVector;
    private Vector3 startSize;

    private void OnEnable()
    {
        maxWaterHeight = this.gameObject.transform.localScale.x / 2.8f;
        timer = 0;

        sinkVector = new Vector3(0, 0.00093f, 0);
        startSize = new Vector3(sinkingObject.transform.localScale.x, 0, 1);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("KingFrogLilyPad"))
        {
            if(collision.transform.localScale.x >= collision.gameObject.GetComponent<KingFrogLilyPad>().GetMaxPadSize())
            {
                sinking = false;
                sinkingObject.transform.localScale = startSize;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("KingFrogLilyPad"))
        {
            sinking = true;
            StartCoroutine(Sink());
        }
    }

    IEnumerator Sink()
    {
        while(sinking && sinkingObject.transform.localScale.y < maxWaterHeight)
        {
            sinkingObject.transform.localScale += sinkVector;
            yield return ws;
        }

        if(sinkingObject.transform.localScale.y >= maxWaterHeight - 0.1)
        {
            StartCoroutine(Drown());
        }
        yield return ws;
    }

    IEnumerator Drown()
    {
        gameObject.GetComponent<PlayerHealth>().takeDamage();
        while (sinking)
        {
            if (timer > hurtSpeed)
            {
                gameObject.GetComponent<PlayerHealth>().takeDamage();

                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
            yield return ws;
        }
    }
}
