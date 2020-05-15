using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public KeyCode Direction;
    public KeyCode ControllerDirection;
    public PlayerDance NoteDestroyPoint;
    public Renderer rend;
    public Color ArrowColor;
    public Color LeftArrowColor;
    public Color RightArrowColor;
    public Color UpArrowColor;
    public Color DownArrowColor;

    public ParticleSystem DestroyNoteEffect;

    // Start is called before the first frame update
    void Start()
    {
        ChooseArrowColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Direction))
        {
            NoteDestroyPoint.DestroyNote(transform);
        }
    }

    private void ChooseArrowColor()
    {
        switch(Direction)
        {
            case KeyCode.LeftArrow:
                SetColorArrow(LeftArrowColor);
                break;

            case KeyCode.RightArrow:
                SetColorArrow(RightArrowColor);
                break;

            case KeyCode.UpArrow:
                SetColorArrow(UpArrowColor);
                break;

            case KeyCode.DownArrow:
                SetColorArrow(DownArrowColor);
                break;
        }
    }

    private void SetColorArrow(Color color)
    {
        ArrowColor = color;
        rend.material.color = ArrowColor;
        rend.material.EnableKeyword("_EMISSION");
        rend.material.SetColor("_EmissionColor", color);
    }

    private void OnDestroy()
    {
        GameObject newEffect = Instantiate(DestroyNoteEffect.gameObject, transform.position, transform.rotation);
        ParticleSystem.MainModule effectMain = newEffect.GetComponent<ParticleSystem>().main;

        Gradient gradient = new Gradient();
        GradientColorKey[] keys =
        {
            new GradientColorKey(ArrowColor, 1)
        };
        gradient.colorKeys = keys;

        effectMain.startColor = gradient;
        Destroy(newEffect, 2);
    }
}
