using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMageA5 : MonoBehaviour
{
    public int chargePunchFrames;
    void Start()
    {
        
    }

   IEnumerator punch()
    {
        for (int x = 0; x < chargePunchFrames; x++)
        {
            yield return new WaitForSeconds(.1666666666666f);
        }

    
    }
    
}
