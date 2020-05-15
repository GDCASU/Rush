﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public Light SpotLight;
    public ColorChangeEvent Colors;

    public class ColorChangeEvent
    {
        public Color Color;
        public float ColorChangeSpeed;
    }
}