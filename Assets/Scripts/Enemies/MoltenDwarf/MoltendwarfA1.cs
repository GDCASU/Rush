using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoltendwarfA1 : BossAction
{
    public Sprite bulletSprite;
    public Color bulletTint = Color.white;
    public Vector3 scale = new Vector3(0.05F, 0.05F, 0.05F);
    private GameObject hammer;
    public Collider2D col;
    public int windUpAnimation;
    public int hammerIsOut;
    public float ArcOffset;
    public float bulletSpeed;
    public float returnSpeed;
    public float colliderRadius=0.5f;
    /* ^ Will be replaced by
    public float colliderSizeX=5f;
    public float colliderSizeY =4f;
    public float offsetX = 0.25f;
    public float offsetY = 0.9f;
    */

    private float currentOffset;
    private bool isBack;


    public void OnEnable() => StartCoroutine(HammerThrow());                        //Starts the hammer throwing action 
    IEnumerator HammerThrow()
    {
        actionRunning = true;                                                       //This line is  necessary to let the BossBehaviorController know the action is running.
        for (int x = 0; x < windUpAnimation; x++)
        {
            if (x < 40) transform.Rotate(new Vector3(0, 0, 1.125f), Space.Self);    //This makes the sprite rotate 45 degreees clokwise in 40 frames
            else transform.Rotate(new Vector3(0, 0, -2.25f), Space.Self);           //This makes the sprite rotate 45 degreees counter clockwse in 20 frames
            yield return new WaitForEndOfFrame();
        }
        col.enabled = true;                                                         //Makes the molten dwarf be able to take damage after it's done charging up and technically
        spawnHammer();                                                              //After the hammer has been thrown
        for (int x = 0; x < hammerIsOut; x++)
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(HammerBack());
    }
    public void spawnHammer()                                                                               //A lot of this math is just to get the necessary arguments for the bullet.init
    {
        Vector2 VectorToPlayer = PlayerHealth.singleton.transform.position - transform.position;            //Vector math to get the direction of the player
        ArcOffset = (float)(Vector2.SignedAngle(Vector2.right, VectorToPlayer) * Math.PI / 180.0);          //Math to have the rotation of the hammer
        float angle = ArcOffset;                                                                            // |
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));                                // | 
        hammer = BulletPool.rent();                                                                         //Creates a bullet from the rent method of the bulletpool script
        hammer.name = "Hammer";                                                                             //Renames bullet to hammer, necessary for the hammerBack coroutine
        hammer.GetComponent<Bullet>().onScreenDestroy = false;                                              //Makes it so that the hammer can go out of the screen and not be destroyed, necessary for the hammer to be able to hit the player on the wasy back       
        hammer.transform.position = transform.position;                                                     //Makes the starting position of the hammer the center of the dwarf.
        hammer.GetComponent<Bullet>().Init(direction, angle * (float)180.0 / Mathf.PI, bulletSpeed, Bullet.MoveFunctions.None, Bullet.SpawnFunctions.None, bulletSprite, bulletTint, true, colliderRadius, new List<float>());
        /* ^ Will be replaced byt this once the bullet class gets updated
        Destroy(hammer.GetComponent<CircleCollider2D>());                                                   //Removed the defualt circle collider 
        hammer.AddComponent(typeof(BoxCollider2D));                                                         //Adds a box collider instead
        hammer.GetComponent<BoxCollider2D>().isTrigger = true;                                              //Makes sure the box collider isTrigger
        hammer.GetComponent<Bullet>().Init(direction, angle * (float)180.0 / Mathf.PI , bulletSpeed, Bullet.MoveFunctions.None, Bullet.SpawnFunctions.None, bulletSprite, bulletTint, true, colliderSizeX, colliderSizeY, null); //Initializes the hammer with all the calcualted properties
        hammer.GetComponent<BoxCollider2D>().offset = new Vector2(offsetX, offsetY);                        //Makes sure that the collider is as centered as possible
        */
        hammer.transform.localScale = scale;                                                                //Makes sure that the hammer scale is bigger than the default otherwise it be to small to see
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hammer") isBack = true;                                            //Collision to detect when the hammer is back, necessary for the hammerBack coroutine to stop running          
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hammer") isBack = false;                                           //Collision necessary to let the hammerBack coroutine that the hammer has left the moltenDwarf and start the loop  
    }
    IEnumerator HammerBack()
    {
        if (hammer != null) hammer.GetComponent<Bullet>().enabled = false;                                   //Stops the hammer
        Vector2 VectorToDwarf = transform.position - hammer.transform.position;                              //Vector math for the direction of the hammer towards the moltenDwarf
        Vector2 MoveVector = VectorToDwarf.normalized * returnSpeed;                                         // |   
        while (!isBack && GameObject.Find("Hammer"))                                                         //The reason why we use the isBack variable is because otherwise the hammer would destory itself when its spawned
        {                                                                                                    //Loop that moves the hammer towards the direction 
            Vector2 m = MoveVector * Time.deltaTime;
            hammer.transform.localPosition = hammer.transform.localPosition + new Vector3(m.x, m.y, 0);

            yield return new WaitForEndOfFrame();
        }
        Destroy(hammer);
        col.enabled = false;                                                                                 //Disables the dwarfs hitbox
        actionRunning = false;                                                                               //Lets the BossBehaviorController know that the action is done running
    }
}
