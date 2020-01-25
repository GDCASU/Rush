using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenDwarfMove : MonoBehaviour
{
    public GameObject myPlayer;

    private float speed;
    private float step;

    private Vector2 playerPosition;

    private Animator dwarfAnim;

    // Start is called before the first frame update
    void Start()
    {
        dwarfAnim = GetComponent<Animator>();

        speed = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = myPlayer.transform.position;

        if(Vector2.Distance(this.transform.position, playerPosition) > 3.0f)
        {
            MoveToPlayer(playerPosition);
            dwarfAnim.SetBool("isAttacking", false);
        }
        else
        {
            dwarfAnim.SetBool("isAttacking", true);
        }
    }

    void MoveToPlayer(Vector2 target)
    {
        step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(this.transform.position, playerPosition, step);
    }
}
