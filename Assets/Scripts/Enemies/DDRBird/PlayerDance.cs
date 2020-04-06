using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDance : MonoBehaviour
{
    public KeyCode HitDirection;
    public Color PushButtonColor;
    public Color BaseColor;
    public Renderer rend;

    private void OnTriggerStay(Collider other)
    {
        DestroyNote(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyNote(other);
    }

    private void OnTriggerExit(Collider other)
    {
        DestroyNote(other);
    }

    private void DestroyNote(Collider other)
    {
        if (Input.GetKeyDown(HitDirection) && other.tag.Equals("Note"))
        {
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(HitDirection))
        {
            rend.material.color = PushButtonColor;
            rend.material.EnableKeyword("_EMISSION");
            rend.material.SetColor("_EmissionColor", PushButtonColor);
        }

        if (Input.GetKeyUp(HitDirection))
        {
            rend.material.color = BaseColor;
            rend.material.DisableKeyword("_EMISSION");
        }
    }
}
