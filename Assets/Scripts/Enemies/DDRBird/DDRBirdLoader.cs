using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRBirdLoader : MonoBehaviour
{
    public static Beat[] GetBeats()
    {
        var testTextFile = Resources.Load("BeatMaps") as TextAsset;
        Wrapper<Beat> wrapper = JsonUtility.FromJson<Wrapper<Beat>>(testTextFile.text);
        Beat[] beats = wrapper.array;
        return beats;
    }

    /// <summary>
    /// Wrapper classed used for Json Parsing array's cause this
    /// isn't a basic FUCKING functionality by unity's JsonUntility.
    /// </summary>
    /// <typeparam name="T">Type of array you want to parse.</typeparam>
    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }

    /// <summary>
    /// This has to be in the same class as the wrapper apparently?? 
    /// Ask Unity why
    /// </summary>
    [Serializable]
    public class Beat
    {
        public float TimeStamp;
        public Direction[] Directions;
    }

    /// <summary>
    /// This has to be in the same class as the wrapper apparently?? 
    /// Ask Unity why
    /// </summary>
    [Serializable]
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
}
