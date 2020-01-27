using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action that control's the spawning of the turrets
/// </summary>
public class WispBossA0 : MonoBehaviour
{
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

    private void Start()
    {
        //This is mostly a test method that spawns turrets at all locations
        SpawnTurrets(TurretLocations);
    }

    /// <summary>
    /// Spawns a turret for each specified target location
    /// </summary>
    /// <param name="targetLocations">Array of gameobject used as a target reference for where the turret goes to</param>
    public void SpawnTurrets(GameObject[] targetLocations)
    {
        if(targetLocations != null)
        {
            foreach (GameObject location in targetLocations)
            {
                TurretControl control = Instantiate(turretPrefab, transform.position, transform.rotation).GetComponent<TurretControl>();

                control.targetLocationObj = location;
            }
        }
    }
}
