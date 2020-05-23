using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Announcer : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _positiveShouts;

    [SerializeField]
    private AudioClip[] _negativeShouts;

    [SerializeField]
    private AudioSource _speaker;

    [SerializeField]
    private Text[] _shoutTextLocations;

    public void PlayPositiveShout()
    {
        if (_speaker.isPlaying) _speaker.Stop();

        _speaker.clip = _positiveShouts[Random.Range(0, _positiveShouts.Length)];
        _speaker.Play();
        ShowAnnouncement(_speaker.clip.name);
    }

    public void PlayNegativeShout()
    {
        if (_speaker.isPlaying) _speaker.Stop();

        _speaker.clip = _negativeShouts[Random.Range(0, _negativeShouts.Length)];
        _speaker.Play();
        ShowAnnouncement(_speaker.clip.name);
    }

    private void ShowAnnouncement(string text)
    {
        Text shoutText = _shoutTextLocations[Random.Range(0, _shoutTextLocations.Length)];

        shoutText.gameObject.SetActive(true);
        shoutText.text = text;
        StartCoroutine(HideAnnouncement(shoutText));
    }

    private IEnumerator HideAnnouncement(Text shoutText)
    {
        yield return new WaitForSeconds(1);
        
        shoutText.gameObject.SetActive(false);
        shoutText.text = "";
    }
}
