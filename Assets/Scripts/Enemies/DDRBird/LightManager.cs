using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Conductor conductor
    {
        get
        {
            return GetComponent<Conductor>();
        }
    }

    public GameObject Player;

    public Light OnStageLight1;
    public Light OnStageLight2;

    public Light LeftArrowLight;
    public Light RightArrowLight;
    public Light UpArrowLight;
    public Light DownArrowLight;

    public Color[] ColorChoices;

    private void Start()
    {
        Invoke("TurnOnHelpLights", 1);
        Invoke("LightsLookAtPlayer", 5);
        Invoke("TurnOffHelpLights", 8);
        Invoke("TurnOnStageLights", 8);
    }

    public void UpdateLightColors()
    {
        OnStageLight1.color = ColorChoices[Random.Range(0, ColorChoices.Length)];
        OnStageLight2.color = ColorChoices[Random.Range(0, ColorChoices.Length)];
    }

    private void TurnOnHelpLights()
    {
        LeftArrowLight.gameObject.SetActive(true);
        RightArrowLight.gameObject.SetActive(true);
        UpArrowLight.gameObject.SetActive(true);
        DownArrowLight.gameObject.SetActive(true);
    }

    private void LightsLookAtPlayer()
    {
        LeftArrowLight.transform.LookAt(Player.transform, Vector3.left);
        RightArrowLight.transform.LookAt(Player.transform, Vector3.left);
        UpArrowLight.transform.LookAt(Player.transform, Vector3.left);
        DownArrowLight.transform.LookAt(Player.transform, Vector3.left);
    }

    private void TurnOffHelpLights()
    {
        LeftArrowLight.gameObject.SetActive(false);
        RightArrowLight.gameObject.SetActive(false);
        UpArrowLight.gameObject.SetActive(false);
        DownArrowLight.gameObject.SetActive(false);
    }

    private void TurnOnStageLights()
    {
        OnStageLight1.gameObject.SetActive(true);
        OnStageLight2.gameObject.SetActive(true);
    }
}
