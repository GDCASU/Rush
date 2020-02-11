using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the making of the chain and it's movement
/// </summary>
public class ChainHandler : MonoBehaviour
{
    [Header("Object References")]
    public GameObject linkModel;
    public GameObject targetObj;
    public GameObject bossObj;

    [Header("Chain Setup")]
    private Vector3 directionVector;  //Vector from this to the target
    public float speed;
    private bool isMoving;
    public float chainLinkAdjuster; //Increase this value to make the chains links farther apart.
    public float LinkWidth  //This is the width of each link in unity units
    {
        get
        {
            return linkModel.transform.localScale.x * linkModel.GetComponent<BoxCollider2D>().size.x;
        }
    }

    [Header("Player Damage")]
    [SerializeField]
    private float damageDelay = 1f;  //Delay until player can be damaged again
    private IEnumerator damageRoutine = null;

    private void OnDestroy()
    {
        //When the chain is destroyed the boss can take damage
        if (bossObj != null)
        {
            WispHealth health = bossObj.GetComponent<WispHealth>();
            health.SetCanTakeDamage();
        }

        //Destroyes associated turret/target
        if (targetObj != null)
            Destroy(targetObj);
    }

    private void FixedUpdate()
    {
        if(isMoving)
        {
            Vector3 newPos = transform.position;
            newPos += directionVector.normalized * speed;
            transform.position = newPos;
        }
    }

    /// <summary>
    /// Stops moving the chain if we hit the turret/targetObj
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetObj)
        {
            isMoving = false;
        }

        /**
         * This starts the damagin routine for the player when the player has collided with the chain
         * 
         * Note: By checking isMoving this means that even if the chain is visible it will not damage the player till it stops moving.
         * Since the chain is moving fast and it was more of a safe method to me I decided to do it this way. Feel free to change this
         * if needed
         */
        if (!isMoving && collision.gameObject.tag.Equals("Player") && damageRoutine == null)
        {
            damageRoutine = DelayDamage(collision.gameObject.GetComponent<PlayerDash>());
            StartCoroutine(damageRoutine);
        }
    }

    /// <summary>
    /// This sets up the chain by spawning them in and
    /// </summary>
    /// <param name="width">The </param>
    public void SetupChain(float width)
    {
        //Calculates how many chains to spawn
        int chainCount = (int) Mathf.Floor(width / LinkWidth);
        //setX is used as the local x coord to set each new chain as they spawn
        //Ex: If each chain were 5 units wide then the next chains would spawn at setX = 5, then 10, etc
        float setX = 0;

        for(int x = 0; x < chainCount; x++)
        {
            //Creates chain and sets references
            Transform linkTransform = Instantiate(linkModel, transform).GetComponent<Transform>();
            linkTransform.gameObject.GetComponent<ChainVisibleHandler>().bossObj = bossObj;
            linkTransform.gameObject.GetComponent<ChainVisibleHandler>().targetObj = targetObj;

            //Sets starting position value
            Vector3 linkPos = linkTransform.localPosition;
            linkPos.x = setX;
            linkTransform.localPosition = linkPos;

            setX -= LinkWidth + chainLinkAdjuster;
        }

        directionVector = targetObj.transform.position - transform.position;

        //This changes the angle of the chain to match the target turret
        float angle = Vector2.Angle(new Vector2(1, 0), new Vector2(directionVector.x, directionVector.y));
        //Vector2.Angle only returns a value of 0 - 180. If statement compensates for that
        if (directionVector.y < 0)
            angle *= -1;
        Vector3 newAngle = transform.eulerAngles;
        newAngle.z += angle;
        transform.eulerAngles = newAngle;

        //This moves the chain back with the boss in between the chain and the turret
        Vector3 newPos = transform.position - directionVector;
        transform.position = newPos;

        isMoving = true;
    }

    /// <summary>
    /// This handles damage to the player
    /// </summary>
    /// <param name="dash">Reference to tell if the player is currently dashing or not</param>
    private IEnumerator DelayDamage(PlayerDash dash)
    {
        if(!dash.inDash)
        {
            PlayerHealth.singleton.takeDamage();

            yield return new WaitForSeconds(damageDelay);
        }

        damageRoutine = null;

        yield return null;
    }
}
