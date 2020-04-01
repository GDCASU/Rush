using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DDRBirdLoader;

public class Conductor : MonoBehaviour
{
    public AudioSource Speaker;
    public PlayerMovement Player;

    public Transform NoteSpawnPoint;
    public Transform NoteDestroyPoint;
    public GameObject NoteParent;
    public GameObject Music;

    public float NoteSpeed;

    private float SpawnDistance
    {
        get
        {
            return Vector3.Distance(NoteSpawnPoint.position, NoteDestroyPoint.position);
        }
    }

    private List<Beat> NoteTimeStamps
    {
        get
        {
            return Music.GetComponent<Song>().Notes;
        }
    }

    private void Start()
    {
        Music = Instantiate(Music); // This is so whatever happens, it doesn't override the prefab.
        Music.GetComponent<Song>().Notes = DDRBirdLoader.GetBeats();
        Speaker.clip = Music.GetComponent<Song>().Music;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Speaker.Play();
            Player.freezeInPlace = true;
        }


        if (Speaker.isPlaying)
        {
            // Does math to spawn the note ahead of time.
            if (NoteTimeStamps.Count > 0 && Speaker.time >= NoteTimeStamps[0].TimeStamp - SpawnDistance/NoteSpeed)
            {
                GameObject newNote;

                // Runs through every direction to spawn the notes in the correct positions.
                foreach (Direction direction in NoteTimeStamps[0].Directions)
                {
                    SpawnNote(direction, out newNote);
                    newNote.GetComponent<Rigidbody>().velocity = Vector3.forward * NoteSpeed;
                    Destroy(newNote, 10f);  // Destroys the note after 10 seconds.
                }
                NoteTimeStamps.RemoveAt(0);
            }
        }
    }

    private void SpawnNote(Direction direction, out GameObject newNote)
    {
        newNote = Instantiate(NoteParent);
        newNote.transform.position = NoteSpawnPoint.position;
        
        switch(direction)
        {
            case Direction.Left:

                newNote.transform.Rotate(Vector3.forward, -90);
                break;

            case Direction.Right:
                newNote.transform.Rotate(Vector3.forward, 90);
                break;

            case Direction.Down:
                newNote.transform.Rotate(Vector3.forward, 180);
                break;
        }
    }
}
