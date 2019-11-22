using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This action is used to make the boss duplicate into copies in a circle
/// and then converge into the center.
/// 
/// Note: This script will be attached to the boss copies for the movement control
/// </summary>
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

    public GameObject CopyPrefab;

    [Header("Converge Variables")]
    public int ringRadius;
    public int convergeRadius;  //This is the radius that the lady and clones stop converging to the center
    public int copyCount;
    public Vector2 centerPoint; //The center that the lady and clones converge into

    [Header("Between Water Variables")]
    public float sinkTransitionZ;  //This is the z position for when to stop the disappear sequence. This is a positive number
    public float riseTransitionZ;
    public float betweenWaterSpeed;    //Note: This is a positive number

    private void OnEnable()
    {
        bool isRising = true;

        //This indicates we are the main boss and need to sink first. Note that the copies will likely not have to sink
        if (GetComponent<BossBehaviorController>() != null)
        {
            isRising = false;
        }

        //Whether this is a copy or boss it starts off by moving between the water
        StartCoroutine(TransitionBetweenWater(isRising));
    }

    /// <summary>
    /// Coroutine that slowly lowers the boss under the water.
    /// 
    /// Note: This will likely be replaced with an animation later or
    /// something equivelent
    /// </summary>
    /// <param name="isRising">This determines whether the boss or copies will be rising or sinking into the water</param>
    /// <returns></returns>
    private IEnumerator TransitionBetweenWater(bool isRising)
    {
        float adjustedSpeed = 0;

        //Note: Negative is the actual sign to "rise" vice versa for sinking
        if(isRising)
        {
            adjustedSpeed = -1 * Mathf.Abs(betweenWaterSpeed);
        }
        else
        {
            adjustedSpeed = Mathf.Abs(betweenWaterSpeed);
        }

        //This is just a temp delay I added to prevent it from acting on it's own too quickly. Remove later one
        int delay = 3;
        while(delay > 0)
        {
            delay--;
            yield return new WaitForSeconds(1f);
        }

        //Adjusts the objects z position to sink
        if (!isRising)
        {
            while (LadyTransform.position.z <= sinkTransitionZ)
            {
                MoveBetweenWater(adjustedSpeed);

                yield return new WaitForFixedUpdate();
            }
        }
        //Adjusts the objects z position to rise
        else
        {
            while (LadyTransform.position.z >= riseTransitionZ)
            {
                MoveBetweenWater(adjustedSpeed);

                yield return new WaitForFixedUpdate();
            }
        }

        //If this is the main boss
        if (!isRising && GetComponent<BossBehaviorController>() != null)
        {
            CreateAndPlace();
        }

        yield return null;
    }

    /// <summary>
    /// Simple method that handles moving the boss
    /// between the water based on the input speed
    /// </summary>
    /// <param name="adjustedSpeed">The speed to move the boss</param>
    private void MoveBetweenWater(float adjustedSpeed)
    {
        Vector3 newLadyPosition = LadyTransform.position;

        newLadyPosition.z += adjustedSpeed;

        LadyTransform.position = newLadyPosition;
    }

    /// <summary>
    /// Creates copies and positions them 
    /// around a circle
    /// 
    /// Note: This method uses a lot of "copyCount + 1"
    /// because the amount of copies does not account for
    /// the boss itself
    /// </summary>
    private void CreateAndPlace()
    {
        float angleDiff = 2 * Mathf.PI / (copyCount + 1);   //The angle between each copy
        float currentAngle = angleDiff; //This keeps track of the angle we are currently at

        int bossPlacement = Random.Range(0, copyCount + 1); //Random int to help determine the position of the boss

        for(int x = 0; x < copyCount + 1; x++)
        {
            float xPos = centerPoint.x + ringRadius * Mathf.Cos(currentAngle);
            float yPos = centerPoint.y + ringRadius * Mathf.Sin(currentAngle);

            Vector3 newPos = new Vector3(xPos, yPos, LadyTransform.position.z);

            //Either repositions boss or makes copies
            if(x == bossPlacement)
            {
                LadyTransform.position = newPos;
            }
            else
            {
                Instantiate(CopyPrefab, newPos, LadyTransform.rotation);
            }

            currentAngle += angleDiff;
        }
    }
}
