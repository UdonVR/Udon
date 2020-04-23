/*
//Script by Child of the beast
//V 1.0
//https://github.com/ChildoftheBeast/VRC-UdonSharp-Scripts
*/
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;

public class BeerPong : UdonSharpBehaviour
{
    public GameObject ParentOfCups;
    public GameObject Ball;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == ParentOfCups.transform) other.gameObject.SetActive(false);
    }

    public GameObject BallSpawn;
    private Vector3 Ball_Spawn;

    private void Start()
    {
        Ball_Spawn = BallSpawn.transform.position;
    }

    public void RespawnBall()
    {
        Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Ball.transform.position = Ball_Spawn;
    }
    public void RespawnCups()
    {
        int cups = ParentOfCups.transform.childCount;
        for (int v = 0; v < cups; v++)
        {
            ParentOfCups.transform.GetChild(v).gameObject.SetActive(true);
        }
    }
}
