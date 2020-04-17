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
    private TurretControl[] _spawnedTurrets;
    private int _turretsToSpawn = 1;
    public int maxTurrets = 3;
    public GameObject[] turretAttacks;  //Array of attack prefabs that are spawned on turrets

    [Header("Chains")]
    [SerializeField]
    private GameObject _chainPrefab;
    private List<ChainHandler> _spawnedChains;

    [Header("Misc")]
    public float damagePhaseDuration = 3;
    private IEnumerator damageRoutine;

    private void Awake()
    {
        _spawnedChains = new List<ChainHandler>(maxTurrets);
    }

    private void Start()
    {
        SpawnTurrets();
    }

    private void OnDisable()
    {
        actionRunning = false;

        StopCoroutine(damageRoutine);

        foreach (TurretControl control in _spawnedTurrets)
        {
            if (control != null)
                Destroy(control.gameObject);
        }

        foreach (ChainHandler handler in _spawnedChains)
        {
            if (handler != null)
                Destroy(handler.gameObject);
        }
    }

    #region TurretSpawn

    /// <summary>
    /// Spawns a turret for each specified target location
    /// </summary>
    /// <param name="targetLocations">Array of gameobject used as a target reference for where the turret goes to</param>
    public void SpawnTurrets(GameObject[] targetLocations)
    {
        if(targetLocations != null)
        {
            //Essentially resets arrays and sets new lengths
            _spawnedTurrets = new TurretControl[targetLocations.Length];

            //Spawns a turret and chain for each given location
            for(int x = 0; x < targetLocations.Length; x++)
            {
                TurretControl control = Instantiate(turretPrefab, transform.position, transform.rotation).GetComponent<TurretControl>();
                _spawnedTurrets[x] = control;

                control.targetLocationObj = targetLocations[x];
                control.isMoving = true;

                //Spawns a random attack onto the turret
                Instantiate(turretAttacks[Random.Range(0, turretAttacks.Length)], control.transform);

                ChainHandler chain = Instantiate(_chainPrefab, transform.position, transform.rotation).GetComponent<ChainHandler>();
                chain.targetObj = control.gameObject;
                chain.bossObj = gameObject;
                _spawnedChains.Add(chain);
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

    /// <summary>
    /// Override that spawns turrets at random locations and based on 
    /// var turretsToSpawn
    /// </summary>
    public void SpawnTurrets()
    {
        GameObject[] randLocations = new GameObject[_turretsToSpawn];
        //List version of all possible locations
        List<GameObject> locationSelection = new List<GameObject>(TurretLocations);

        //For each turret to spawn pick a random location
        for(int x = 0; x < randLocations.Length; x++)
        {
            int randIndex = Random.Range(0, locationSelection.Count);

            randLocations[x] = locationSelection[randIndex];
            locationSelection.RemoveAt(randIndex);
        }

        if (_turretsToSpawn < maxTurrets)
            _turretsToSpawn++;

        SpawnTurrets(randLocations);
    }

    #endregion

    #region Setup

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
        foreach(TurretControl control in _spawnedTurrets)
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
        for(int x = 0; x < _spawnedChains.Count; x++)
        {
            float distance = Vector3.Magnitude(_spawnedTurrets[x].transform.position - _spawnedChains[x].transform.position);

            _spawnedChains[x].SetupChain(distance);
        }
    }

    #endregion

    /// <summary>
    /// Method called when a chain is destroyed. When all chains are destroyed
    /// a routine is activated for the damage phase
    /// </summary>
    /// <param name="chain"></param>
    public void OnChainDestroy(ChainHandler chain)
    {
        _spawnedChains.Remove(chain);

        //Let's boss take damage once all chains are destroyed
        if(_spawnedChains.Count == 0)
        {
            if (damageRoutine == null)
                damageRoutine = DamageRoutine();
            else
                StopCoroutine(damageRoutine);

            StartCoroutine(damageRoutine);
        }
    }

    /// <summary>
    /// Coroutine that enables the boss to take damage for
    /// a set period of time then continues to spawn in more turrets
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageRoutine()
    {
        WispHealth health = GetComponent<WispHealth>();
        health.canTakeDamage = true;

        yield return new WaitForSeconds(damagePhaseDuration);

        health.canTakeDamage = false;

        damageRoutine = null;

        if(actionRunning)  //Check to make sre this action is still running
            SpawnTurrets();

        yield return null;
    }
}
