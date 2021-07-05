using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
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

    public Light[] OnStageLights;

    public Light LeftArrowLight;
    public Light RightArrowLight;
    public Light UpArrowLight;
    public Light DownArrowLight;

    public Color[] ColorChoices;

    private bool lookAtPlayerTrigger = false;
    private bool dimLightsTrigger = false;
    private bool turnOnStageLights = false;

    private void Start()
    {
        Invoke("TurnOnHelpLights", 1);
        Invoke("LightsLookAtPlayer", 5);
        Invoke("TurnOffHelpLights", 6.5f);
        Invoke("TurnOnStageLights", 8);
    }

    public void UpdateLightColors()
    {
        foreach (Light light in OnStageLights)
        {
            light.color = ColorChoices[Random.Range(0, ColorChoices.Length)];
        }
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
        lookAtPlayerTrigger = true;
    }

    private void Update()
    {
        if (lookAtPlayerTrigger)
        {
            LookAtPlayer(LeftArrowLight.gameObject);
            LookAtPlayer(RightArrowLight.gameObject);
            LookAtPlayer(UpArrowLight.gameObject);
            LookAtPlayer(DownArrowLight.gameObject);
        }

        if (dimLightsTrigger)
        {
            float value = Time.deltaTime * 10;
            LeftArrowLight.intensity -= value;
            RightArrowLight.intensity -= value;
            UpArrowLight.intensity -= value;
            DownArrowLight.intensity -= value;

            if (LeftArrowLight.intensity <= 0)
            {
                dimLightsTrigger = false;
            }
        }

        if (turnOnStageLights)
        {
            float value = Time.deltaTime * 15;

            foreach (Light light in OnStageLights)
            {
                light.intensity += value;
            }

            if (OnStageLights[0].intensity >= 9)
            {
                turnOnStageLights = false;
            }
        }
    }

    private void LookAtPlayer(GameObject light)
    {
        Vector3 lTargetDir = Player.transform.position - light.transform.position;
        lTargetDir.y = 0.0f;
        light.transform.rotation = Quaternion.RotateTowards(light.transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * .5f);
    }

    private void TurnOffHelpLights()
    {
        lookAtPlayerTrigger = false;
        dimLightsTrigger = true;
    }

    private void TurnOnStageLights()
    {
        foreach (Light light in OnStageLights)
        {
            light.gameObject.SetActive(true);
        }
        UpdateLightColors();
        turnOnStageLights = true;
    }

    public void FlashLight(GameObject light, float seconds)
    {
        light.SetActive(true);
        StartCoroutine(TurnOffLight(light, seconds));
    }

    private IEnumerator TurnOffLight(GameObject light, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        light.SetActive(false);
    }
}
