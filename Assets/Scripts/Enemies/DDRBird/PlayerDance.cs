using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDance : MonoBehaviour
{
    public KeyCode HitDirection;
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(HitDirection) && other.tag.Equals("Note"))
        {
            Destroy(other.gameObject);
        }
    }
}
