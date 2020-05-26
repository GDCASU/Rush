using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossesStare : MonoBehaviour
{
    public bool inverted;
    void Update()
    {
        var VectorToPlayer = (Vector3)(PlayerHealth.singleton.transform.position - transform.position).normalized * (inverted? -1:1 );
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, 0, Vector2.SignedAngle(-Vector2.right, VectorToPlayer)));
    }
}
