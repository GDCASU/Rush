using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that control's the spawning of the turrets
/// </summary>
public class WispBossA0 : BossAction
{
    [Header("Turrets")]
    /**
     * Each turret location is under one parent obj. This is the
     * var locationsParent. I am then able to then get a reference to all locations
     * using the parent child relationship
     */
    public GameObject locationsParent;
    private GameObject[] TurretLocations
    {
        get
        {
            GameObject[] locations = new GameObject[locationsParent.transform.childCount];

            for(int x = 0; x < locations.Length; x++)
            {
                locations[x] = locationsParent.transform.GetChild(x).gameObject;
            }

            return locations;
        }
    }
    public GameObject turretPrefab;
    private TurretControl[] spawnedTurrets;

    [Header("Chains")]
    [SerializeField]
    private GameObject chainPrefab;
    private ChainHandler[] spawnedChains;

    private void Start()
    {
        //This is mostly a test method that spawns turrets at all locations
        SpawnTurrets(TurretLocations[3]);
    }

    /// <summary>
    /// Destroys all associated objects when the boss is destroyed
    /// </summary>
    private void OnDestroy()
    {
        foreach(TurretControl control in spawnedTurrets)
        {
            if (control != null)
                Destroy(control.gameObject);
        }

        foreach (ChainHandler handler in spawnedChains)
        {
            if (handler != null)
                Destroy(handler.gameObject);
        }
    }

    /// <summary>
    /// Spawns a turret for each specified target location
    /// </summary>
    /// <param name="targetLocations">Array of gameobject used as a target reference for where the turret goes to</param>
    public void SpawnTurrets(GameObject[] targetLocations)
    {
        if(targetLocations != null)
        {
            //Essentially resets arrays and sets new lengths
            spawnedTurrets = new TurretControl[targetLocations.Length];
            spawnedChains = new ChainHandler[spawnedTurrets.Length];

            //Spawns a turret and chain for each given location
            for(int x = 0; x < targetLocations.Length; x++)
            {
                TurretControl control = Instantiate(turretPrefab, transform.position, transform.rotation).GetComponent<TurretControl>();
                spawnedTurrets[x] = control;

                control.targetLocationObj = targetLocations[x];
                control.isMoving = true;

                ChainHandler chain = Instantiate(chainPrefab, transform.position, transform.rotation).GetComponent<ChainHandler>();
                chain.targetObj = control.gameObject;
                chain.bossObj = gameObject;
                spawnedChains[x] = chain;
            }
        }

        StartCoroutine(SetupTurret());
    }

    /// <summary>
    /// Simple override that spawns a single turret to a single
    /// specified location
    /// </summary>
    /// <param name="targetLocation">Gameobject of the turret location</param>
    public void SpawnTurrets(GameObject targetLocation)
    {
        GameObject[] location = { targetLocation };
        SpawnTurrets(location);
    }

    private IEnumerator SetupTurret()
    {
        //Waits for turret to get to their locations
        while(!AreTurretsSet())
        {
            yield return new WaitForEndOfFrame();
        }

        SetupChains();

        yield return null;
    }

    /// <summary>
    /// Simple bool check to see if all currently spawned turrets
    /// are moving or not
    /// </summary>
    /// <returns>True if all turrets are not moving and false otherwise</returns>
    private bool AreTurretsSet()
    {
        foreach(TurretControl control in spawnedTurrets)
        {
            if (control.isMoving)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Not fully done so re-comment later but this should basically
    /// setup each chain individually (I think)
    /// </summary>
    private void SetupChains()
    {
        for(int x = 0; x < spawnedChains.Length; x++)
        {
            float distance = Vector3.Magnitude(spawnedTurrets[x].transform.position - spawnedChains[x].transform.position);

            spawnedChains[x].SetupChain(distance);
        }
    }
}
