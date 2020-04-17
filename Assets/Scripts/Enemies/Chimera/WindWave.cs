using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindWave : MonoBehaviour
{
    public int moveFrames;
    public int speed;
    Vector3 dir;
    public void Start()
    {
        dir = (PlayerHealth.singleton.transform.position - transform.position).normalized * speed;
        StartCoroutine("move");
    }
    IEnumerator move()
    {
        for(int x=0;x<moveFrames;x++)
        {
            Vector2 m = dir * Time.deltaTime;
            transform.localPosition = transform.localPosition + new Vector3(m.x, m.y, 0);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
