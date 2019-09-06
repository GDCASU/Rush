using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatbossA3 : MonoBehaviour
{    
    public GameObject racerL; //left
    public GameObject racerR; //right
    public GameObject racerT; //top
    public GameObject racerLB; //left bottom
    public GameObject racerRB; //right bottom

    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject topWall;
    public GameObject leftBottomWall;
    public GameObject rightBottomWall;

    BoxCollider2D leftCollider;
    BoxCollider2D rightCollider;
    BoxCollider2D topCollider;
    BoxCollider2D leftBottomCollider;
    BoxCollider2D rightBottomCollider;

    float yPosLR; //y position of wall for left and right
    float xPosTop; //x position of wall for top
    float xPosLB; //x position of wall for left bottom
    float xPosRB; //x position of wall for right bottom

    //lengths of walls themselves
    float xLengthLR;
    float yLengthLR;
    float xLengthTop;
    float yLengthTop;
    float xLengthLB;
    float yLengthLB;
    float xLengthRB;
    float yLengthRB;

    float leftSpawn;
    float rightSpawn;
    float topSpawn;
    float leftBottomSpawn;
    float rightBottomSpawn;

    private RatbossA0 A0;

    void Awake()
    {
        leftCollider = leftWall.GetComponent<BoxCollider2D>();
        rightCollider = rightWall.GetComponent<BoxCollider2D>();
        topCollider = topWall.GetComponent<BoxCollider2D>();
        leftBottomCollider = leftBottomWall.GetComponent<BoxCollider2D>();
        rightBottomCollider = rightBottomWall.GetComponent<BoxCollider2D>();

        yPosLR = leftWall.transform.position.y;
        //Debug.Log(leftWall.transform.position.y);

        xPosTop = topWall.transform.position.x;
        //Debug.Log(topWall.transform.position.x);

        xPosLB = leftBottomWall.transform.position.x;
        //Debug.Log(leftBottomWall.transform.position.x);

        xPosRB = rightBottomWall.transform.position.x;
        //Debug.Log(rightBottomWall.transform.position.x);


        xLengthLR = leftCollider.bounds.size.x;
        yLengthLR = leftCollider.bounds.size.y;
        xLengthTop = topCollider.bounds.size.x;
        yLengthTop = topCollider.bounds.size.y;
        xLengthLB = leftBottomCollider.bounds.size.x;
        yLengthLB = leftBottomCollider.bounds.size.y;
        xLengthRB = rightBottomCollider.bounds.size.x;
        yLengthRB = rightBottomCollider.bounds.size.y;

        racerL.GetComponent<RacerWall>().wall = rightWall;
        racerR.GetComponent<RacerWall>().wall = leftWall;
        racerT.GetComponent<RacerWall>().wall = leftBottomWall;
        racerLB.GetComponent<RacerWall>().wall = topWall;
        racerRB.GetComponent<RacerWall>().wall = topWall;

        A0 = GetComponent<RatbossA0>();
    }

    private void OnEnable() => StartCoroutine(Racers());

    IEnumerator Racers()
    {
        yield return A0.shakedoors();

        leftSpawn = Random.Range(yPosLR - (yLengthLR / 2), yPosLR + (yLengthLR / 2));
        racerL.transform.position = new Vector3(leftWall.transform.position.x + (xLengthLR / 2)+0.5f, leftSpawn, -2);
        //Debug.Log(yPosLR + " " + yLengthLR);

        rightSpawn = Random.Range(yPosLR - (yLengthLR / 2), yPosLR + (yLengthLR / 2));
        racerR.transform.position = new Vector3(rightWall.transform.position.x - (xLengthLR / 2)-0.5f, rightSpawn, -2);

        topSpawn = Random.Range(xPosTop - (xLengthTop / 2), xPosTop + (xLengthTop / 2));
        racerT.transform.position = new Vector3(topSpawn, topWall.transform.position.y - (yLengthTop / 2)-0.5f, -2);
        //Debug.Log(xPosTop + " " + xLengthTop);

        leftBottomSpawn = Random.Range(xPosLB - (xLengthLB / 2), xPosLB + (xLengthLB / 2));
        racerLB.transform.position = new Vector3(leftBottomSpawn, leftBottomWall.transform.position.y + (yLengthLB / 2)+0.5f, -2);
        //Debug.Log(xPosLB + " " + xLengthLB);

        rightBottomSpawn = Random.Range(xPosRB - (xLengthRB / 2), xPosRB + (xLengthRB / 2));
        racerRB.transform.position = new Vector3(rightBottomSpawn, rightBottomWall.transform.position.y + (yLengthRB / 2)+0.5f, -2);
       
        Instantiate(racerL);
        Instantiate(racerR);
        Instantiate(racerT);
        Instantiate(racerLB);
        Instantiate(racerRB);


        yield return A0.returnIntoDoor();
        //Debug.Log(xPosRB + " " + xLengthRB);

        //Debug.Log("left " + leftSpawn);
        //Debug.Log("right " + rightSpawn);
        //Debug.Log("top " + topSpawn);
        //Debug.Log("LB " + leftBottomSpawn);
        //Debug.Log("RB " + rightBottomSpawn);
    }
}
