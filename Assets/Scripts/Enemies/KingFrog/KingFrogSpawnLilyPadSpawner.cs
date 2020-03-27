using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogSpawnLilyPadSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject lilyPad;
    private GameObject temp;

    [SerializeField]
    private Vector2 center;

    [SerializeField]
    private float arenaRadius = 3.0f;

    private bool padCollided;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SpawnPad();
        }
    }

    private void SpawnPad()
    {
        Vector2 pos = center + new Vector2(Random.Range(-arenaRadius, arenaRadius), Random.Range(-arenaRadius, arenaRadius));

        Instantiate(lilyPad, pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, arenaRadius);
    }
}
