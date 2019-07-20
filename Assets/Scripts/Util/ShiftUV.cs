using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftUV : MonoBehaviour
{
    MeshRenderer ObjectRender;
    Vector2 UV_Offset;

    public float U_Speed = 0.0f;
    public float V_Speed = 0.0f;

    // Use this for initialization
    void Start()
    {
        UV_Offset = new Vector2(0, 0);
        ObjectRender = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UV_Offset = new Vector2(UV_Offset.x + U_Speed, UV_Offset.y + V_Speed);
        for (int i = 0; i < ObjectRender.materials.Length; i++)
        {
            ObjectRender.materials[i].SetTextureOffset("_MainTex", UV_Offset);
        }
    }
}

