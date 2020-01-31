using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the making of the chain and it's movement
/// </summary>
public class ChainHandler : MonoBehaviour
{
    public GameObject linkModel;
    public GameObject targetObj;

    private BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Note: Not done, finish summary later
    /// </summary>
    /// <param name="width"></param>
    public void SetupChain(float width)
    {
        Vector2 newSize = collider.size;
        newSize.x = width;

        collider.size = newSize;
    }
}
