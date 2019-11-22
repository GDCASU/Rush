using System.Collections;
using UnityEngine;

public class ChimeraFly : BossAction
{
    private Transform chimera;
    private System.Random rng = new System.Random();
    public float flySpeed = 0.1f;

    public void Awake()
    {
        chimera = GetComponent<Transform>();
    }

    public void OnEnable() => StartCoroutine(Fly());

    IEnumerator Fly()
    {
        // Fly off screen
        for (int i = 0; i < 120; i++)
        {
            chimera.position += Vector3.back * flySpeed;
            yield return new WaitForEndOfFrame();
        }

        // Wait a few seconds
        for (int i = 0; i < 120; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        float x = -44.35f - ((float)rng.NextDouble() * 16.15f);
        float y = 15.86f - ((float)rng.NextDouble() * 9.79f);
        chimera.position = new Vector3(x, y, chimera.position.z);

        // Fly back to floor
        for (int i = 0; i < 120; i++)
        {
            chimera.position += Vector3.forward * flySpeed;
            yield return new WaitForEndOfFrame();
        }
    }
}
