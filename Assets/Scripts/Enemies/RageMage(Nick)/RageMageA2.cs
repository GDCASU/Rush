using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that spawns in fireballs for 
/// the rage mage boss
/// 
/// /// Description is as follows:
/// (4/2/20)
/// He creates a slow-moving fireball that follows the 
/// player around. This lasts for the duration of the 
/// next magic missile attack before dissipating.
/// </summary>
public class RageMageA2 : MonoBehaviour
{
    public float FireRate;
    private float _fireRateCount;

    public GameObject CurrentFireBall;
    private BulletSpawner _spawner;

    private void Awake()
    {
        _spawner = GetComponent<BulletSpawner>();
    }

    private void Update()
    {
        if (_fireRateCount < 0)
        {
            _fireRateCount = FireRate;

            float angle = (float)(Vector2.SignedAngle(Vector2.right, PlayerHealth.singleton.transform.position - _spawner.transform.position) * Mathf.PI / 180.0);
            CurrentFireBall = _spawner.GenerateBullet(angle, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
        }
        else
            _fireRateCount -= Time.deltaTime;
    }
}
