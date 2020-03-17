using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsemanSpecial : BossAction
{
    public GameObject lance;
    public GameObject ghosts;
    public int raisingFraames;
    public float speed;
    public List<Transform> spawnpoints;
    private System.Random rng = new System.Random();

    private void OnEnable()
    {
        StartCoroutine("apparitions");
    }
    IEnumerator apparitions()
    {
        if (transform.position.x > 0)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        while ((Mathf.Abs(transform.position.x) > 0.5f) && (Mathf.Abs(transform.position.y) > 0.5f))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, transform.position.z), 0.25f);
            yield return new WaitForEndOfFrame();
        }

        List<Transform> original = new List<Transform>(spawnpoints);
        transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        actionRunning = true;
        int remove = rng.Next(5);
        int real = rng.Next(4);
        for (int x = 0; x < raisingFraames; x++)
        {
            lance.transform.Rotate(new Vector3(0, 0, 120f / raisingFraames));
            yield return new WaitForEndOfFrame();
        }
        spawnpoints.RemoveAt(remove);
        for (int x = 0; x < spawnpoints.Count; x++)
        {
            GameObject temp;
            if (x == real)
            {
                transform.position = spawnpoints[x].position;
                GetComponent<HorsemanApparition>().enabled = true;
                GetComponent<HorsemanApparition>().speed = speed;
            }
            else
            {
                temp = Instantiate(ghosts, new Vector3(spawnpoints[x].position.x, spawnpoints[x].position.y, spawnpoints[x].position.z + 0.1f), Quaternion.identity);
                temp.GetComponent<HorsemanApparition>().speed = speed;
            }
        }
        while (GameObject.FindGameObjectsWithTag("HorsemanApparition").Length != 0)
        {
            yield return new WaitForEndOfFrame();
        }

        for (int x = 0; x < raisingFraames/2; x++)
        {
            lance.transform.Rotate(new Vector3(0, 0, -120f * 2 / raisingFraames));
            yield return new WaitForEndOfFrame();
        }
        actionRunning = false;
        spawnpoints = original;   
    }
}
