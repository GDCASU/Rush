using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public KeyCode Direction;
    public PlayerDance NoteDestroyPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Direction))
        {
            NoteDestroyPoint.DestroyNote(transform);
        }
    }
}
