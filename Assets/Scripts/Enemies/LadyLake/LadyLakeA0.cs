using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyLakeA0 : MonoBehaviour
{
    public GameObject LadyObject
    {
        get
        {
            return this.gameObject;
        }
    }
    private Transform LadyTransform
    {
        get
        {
            return LadyObject.GetComponent<Transform>();
        }
    }

    [Header("Converge Variables")]
    public int ringRadius;
    public int convergeRadius;  //This is the radius that the lady and clones stop converging to the center
    public int copyCount;
    public Vector2 centerPoint; //The center that the lady and clones converge into

    [Header("Disappear Variables")]
    public float disappearTransitionZ;  //This is the z position for when to stop the disappear sequence. This is a positive number
    public float disappearSpeed;    //Note: This is a positive number

    private void OnEnable()
    {
        StartCoroutine(DisappearIntoWater());
    }

    /// <summary>
    /// Coroutine that slowly lowers the boss under the water.
    /// 
    /// Note: This will likely be replaced with an animation later
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisappearIntoWater()
    {
        int delay = 3;
        while(delay > 0)
        {
            delay--;
            yield return new WaitForSeconds(1f);
        }

        while(LadyTransform.position.z <= disappearTransitionZ)
        {
            Vector3 newLadyPosition = LadyTransform.position;

            /*
             * Note: The z position will start of negative and
             * must rise to a positive number
             */

            newLadyPosition.z += disappearSpeed;

            LadyTransform.position = newLadyPosition;

            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    private void CreateAndPlace()
    {
        for(int x = 0; x < copyCount; x++)
        {

        }
    }
}
