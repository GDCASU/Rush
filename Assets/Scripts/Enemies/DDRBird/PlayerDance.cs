using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerDance : MonoBehaviour
{
    public KeyCode HitDirection;

    public Transform TopHitZone;
    public Transform BottomHitZone;
    public float HitZoneDistance = 1f;

    public Color PushButtonColor;
    public Color BaseColor;
    public Renderer rend;

    public DDRBirdManager ddrBirdManager;
    public LightManager lightManager;

    public GameObject UnderLight;

    void Start()
    {
        // This diactivates the zone marker's renderers in the game only when the game is running.
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            TopHitZone.GetComponent<Renderer>().enabled = false;
            BottomHitZone.GetComponent<Renderer>().enabled = false;
        }
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(HitDirection))
        {
            rend.material.color = PushButtonColor;
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", PushButtonColor);
            UnderLight.SetActive(true);
        }

        if (Input.GetKeyUp(HitDirection))
        {
            rend.material.color = BaseColor;
            rend.material.DisableKeyword("_EMISSION");
            UnderLight.SetActive(false);
        }
    }

    public void DestroyNote(Transform note)
    {
        // Compares the y values of the note to the destroy point y's
        if (Mathf.Sqrt(Mathf.Pow(TopHitZone.position.y - BottomHitZone.position.y, 2))  ==
            Mathf.Sqrt(Mathf.Pow(TopHitZone.position.y - note.position.y, 2)) + Mathf.Sqrt(Mathf.Pow(BottomHitZone.position.y - note.position.y, 2)))
        {
            Destroy(note.gameObject);
            ddrBirdManager.ChangeDance();
            lightManager.UpdateLightColors();
        }
    }
}
