using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DDRBirdLoader;

public class Conductor : MonoBehaviour
{
    public static float CurrentSongTime;

    public AudioSource Speaker;
    public PlayerMovement Player;

    public Transform LeftNoteSpawnPoint;
    public PlayerDance LeftNoteDestroyPoint;
    public Transform DownNoteSpawnPoint;
    public PlayerDance DownNoteDestroyPoint;
    public Transform UpNoteSpawnPoint;
    public PlayerDance UpNoteDestroyPoint;
    public Transform RightNoteSpawnPoint;
    public PlayerDance RightNoteDestroyPoint;
    public GameObject NoteParent;
    public GameObject Music;

    public float NoteSpeed;

    [SerializeField]
    private float _distance;

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
        _distance = Vector3.Distance(new Vector3(-13.68f, 7.02f, -1f), LeftNoteDestroyPoint.transform.position);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Speaker.Play();
            Player.freezeInPlace = true;
        }


        if (Speaker.isPlaying)
        {
            // Does math to spawn the note ahead of time.
            if (NoteTimeStamps.Count > 0 && Speaker.time >= NoteTimeStamps[0].TimeStamp - _distance/ NoteSpeed)
            {
                GameObject newNote;

                // Runs through every direction to spawn the notes in the correct positions.
                foreach (Direction direction in NoteTimeStamps[0].Directions)
                {
                    SpawnNote(direction, out newNote);
                    newNote.GetComponent<Rigidbody>().velocity = Vector3.down * NoteSpeed;

                    Destroy(newNote, 10f);  // Destroys the note after 10 seconds.
                }
                NoteTimeStamps.RemoveAt(0);
            }

            CurrentSongTime = Speaker.time;
        }
    }

    private void SpawnNote(Direction direction, out GameObject newNote)
    {
        newNote = Instantiate(NoteParent);

        switch(direction)
        {
            case Direction.Left:
                newNote.transform.position = LeftNoteSpawnPoint.position;
                newNote.transform.Rotate(Vector3.down, 90);
                newNote.GetComponent<Note>().Direction = KeyCode.LeftArrow;
                newNote.GetComponent<Note>().NoteDestroyPoint = LeftNoteDestroyPoint;
                break;

            case Direction.Right:
                newNote.transform.position = RightNoteSpawnPoint.position;
                newNote.transform.Rotate(Vector3.down, -90);
                newNote.GetComponent<Note>().Direction = KeyCode.RightArrow;
                newNote.GetComponent<Note>().NoteDestroyPoint = RightNoteDestroyPoint;
                break;

            case Direction.Down:
                newNote.transform.position = DownNoteSpawnPoint.position;
                newNote.transform.Rotate(Vector3.down, 180);
                newNote.GetComponent<Note>().Direction = KeyCode.DownArrow;
                newNote.GetComponent<Note>().NoteDestroyPoint = DownNoteDestroyPoint;
                break;

            case Direction.Up:
                newNote.transform.position = UpNoteSpawnPoint.position;
                newNote.GetComponent<Note>().Direction = KeyCode.UpArrow;
                newNote.GetComponent<Note>().NoteDestroyPoint = UpNoteDestroyPoint;
                break;
        }
    }
}
