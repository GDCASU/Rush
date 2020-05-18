using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsemanSpecial : BossAction
{
    public GameObject lance;
    public GameObject ghosts;
    public int raisingFraames;
    public float speed;
    public GameObject spawnpointsObject;
    public GameObject spawnpointsObject2;
    private System.Random rng = new System.Random();
    public int remove;
    public int remove2;

    private void OnEnable()
    {       
        StartCoroutine("apparitions");
    }
    IEnumerator apparitions()
    {
        List<Transform> spawnpoints = new List<Transform>();

        if (GetComponent<BossBehaviorController>().currentPhase == 1)
        {
            spawnpointsObject = spawnpointsObject2;
        }
        for (int i = 0; i < spawnpointsObject.transform.childCount; i++)
        {
            spawnpoints.Add(spawnpointsObject.transform.GetChild(i));
        }

        if (transform.position.x > 0)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        while ((Mathf.Abs(transform.position.x) > 0.5f) && (Mathf.Abs(transform.position.y) > 0.5f))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, transform.position.z), 0.25f);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        actionRunning = true;

        remove = rng.Next(spawnpoints.Count - 1);
        remove2 = -1;
        if (GetComponent<BossBehaviorController>().currentPhase == 1)
        {
            remove2 = rng.Next(spawnpoints.Count - 1);
            while (remove2 == remove)
                remove2 = rng.Next(spawnpoints.Count - 1);
        }
        int real = rng.Next(spawnpoints.Count - 1);
        while (real == remove || real == remove2)
            real = rng.Next(spawnpoints.Count - 1);
        if (GetComponent<BossBehaviorController>().currentPhase == 1)
        {
            print("check 1 " + spawnpoints.Count);
            if (remove > remove2)
            {
                print("check 2");

                spawnpoints.RemoveAt(remove); if (real > remove) real--;
                print("check 3");

                spawnpoints.RemoveAt(remove2); if (real >= remove2) real--;
                print("check 4");

            }
            else
            {
                print("check 5");

                spawnpoints.RemoveAt(remove2); if (real > remove2) real--;
                print("check 6");

                spawnpoints.RemoveAt(remove); if (real >= remove) real--;
                print("check 7");

            }
        }
        else
            spawnpoints.RemoveAt(remove);

        for (int x = 0; x < raisingFraames; x++)
        {
            lance.transform.Rotate(new Vector3(0, 0, 120f / raisingFraames));
            yield return new WaitForEndOfFrame();
        }
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
    }
}
