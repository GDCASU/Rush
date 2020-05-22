using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light OnStageLight1;
    public Light OnStageLight2;

    public Color[] ColorChoices;

    public void UpdateLightColors()
    {
        OnStageLight1.color = ColorChoices[Random.Range(0, ColorChoices.Length)];
        OnStageLight2.color = ColorChoices[Random.Range(0, ColorChoices.Length)];
    }
}
