using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the laser attack by the Wisp boss in phase 2. Note that unlike other
/// WispAttackModel's this one does not use a bullet spawner
/// </summary>
public class WispLaserAttack : WispAttackModel
{
    public GameObject laserPrefab;
    /**
     * Manual variable that sets the local position of the laser on spawn and makes it so that the parent is on one edge of the lazer.
     * If possible do some sort of calculation of how wide the obj is instead (yes I tried and yes I failed)
     */
    public float distToParent = 40f;

    [Header("Rotation")]
    public float rotationSpeed;
    public float rotationCount; //This is how many times the laser sort of swipes around. Ex: 2 would cause the laser to go "there and back"
    public float rotationAngle; //This is how much the laser attack rotates in degrees
    public bool rotatingClockwise;

    private GameObject spawnedLaser;

    /**
     * As a temp way to tell if the laser if in full effect I made it so that
     * the laser fades in and when it's fully faded in the player can take damage.
     * Feel free to change this later
     */
    [Header("Fade")]
    public float fadeSpeed;

    private void Start()
    {
        _attackRoutine = AttackRoutine();
    }

    private IEnumerator AttackRoutine()
    {
        while(true)
        {
            spawnedLaser = Instantiate(laserPrefab, transform);
            spawnedLaser.transform.localPosition = new Vector3(distToParent, 0, 0);    //This makes it so that the lazer now hinges on the parent obj

            Vector3 directionVector = Vector3.Normalize(PlayerHealth.singleton.transform.position - transform.position);

            //Gets the angle between the player and boss
            float angle = Vector2.Angle(new Vector2(1, 0), new Vector2(directionVector.x, directionVector.y));
            //Vector2.Angle only returns a value of 0 - 180. If statement compensates for that
            if (directionVector.y < 0)
                angle *= -1;
            Vector3 newAngle = transform.eulerAngles;
            newAngle.z += angle;
            transform.eulerAngles = newAngle;

            SpriteRenderer renderer = spawnedLaser.GetComponent<SpriteRenderer>();

            //Fade in
            while(renderer.color.a < 1)
            {
                Color newColor = renderer.color;
                newColor.a += fadeSpeed * Time.deltaTime;
                renderer.color = newColor;

                yield return new WaitForEndOfFrame();
            }

            spawnedLaser.GetComponent<WispLaserControl>().canDamagePlayer = true;

            for(int x = 0; x < rotationCount; x++)
            {
                float countRotation = 0;

                int rotationDirection = 1;
                if (rotatingClockwise)
                    rotationDirection = -1;

                while(Mathf.Abs(countRotation) < rotationAngle)
                {
                    float addRotation = rotationSpeed * rotationDirection;

                    Vector3 newRotation = transform.eulerAngles;
                    newRotation.z += addRotation;
                    transform.eulerAngles = newRotation;

                    countRotation += addRotation;

                    yield return new WaitForEndOfFrame();
                }

                rotatingClockwise = !rotatingClockwise;
            }

            //Fade out
            while (renderer.color.a > 0)
            {
                Color newColor = renderer.color;
                newColor.a -= fadeSpeed * Time.deltaTime;
                renderer.color = newColor;

                yield return new WaitForEndOfFrame();
            }

            spawnedLaser.GetComponent<WispLaserControl>().canDamagePlayer = false;

            //Resets the rotation of the parent obj
            transform.eulerAngles = Vector3.zero;

            Destroy(spawnedLaser);
            spawnedLaser = null;

            yield return new WaitForEndOfFrame();
        }
    }

    public void StopAttacking()
    {
        base.StopAttacking();

        if (spawnedLaser != null)
        {
            Destroy(spawnedLaser);
            spawnedLaser = null;
        }

    }
}
