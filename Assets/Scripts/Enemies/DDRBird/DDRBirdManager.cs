using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDRBirdManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _ddrBird;

    [SerializeField]
    private PlayerHealth _player;

    [SerializeField]
    private int _increaseHealthCombo;

    [SerializeField]
    private Text _comboText;

    [SerializeField]
    private int _comboCounter = 0;

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
        _comboCounter++;
        UpdateUI();
        AddHeart();
    }

    private void AddHeart()
    {
        if (_comboCounter % _increaseHealthCombo == 0)
        {
            _player.GainHealth();
        }
    }

    private void UpdateUI()
    {
        _comboText.text = "x" + _comboCounter;
    }

    public void ResetCombo()
    {
        _comboCounter = 0;
        UpdateUI();
    }
}
