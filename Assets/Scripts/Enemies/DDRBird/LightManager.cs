using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light LeftSpotLight;
    public Light MiddleSpotLight;
    public Light RightSpotLight;

    [SerializeField]
    private float _lerpFactor = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MiddleSpotLight.color = Color.Lerp(MiddleSpotLight.color, Color.green, _lerpFactor);

        if (MiddleSpotLight.color == Color.green)
        {
            _lerpFactor *= -1;
        }
    }
}
