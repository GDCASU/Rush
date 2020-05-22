using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDRBirdManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _ddrBird;

    [SerializeField]
    private Sprite[] _birdDances;
    private int _currentDanceIndex = 0;


    public void ChangeDance()
    {
        int randomIndex = Random.Range(0, _birdDances.Length);

        do
        {
            randomIndex = Random.Range(0, _birdDances.Length);
        } while (_currentDanceIndex == randomIndex);

        _currentDanceIndex = randomIndex;
        _ddrBird.sprite = _birdDances[_currentDanceIndex];
    }
}
