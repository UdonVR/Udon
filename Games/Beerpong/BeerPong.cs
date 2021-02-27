/*
//Script by Child of the beast and Takato65
//V 1.0
//https://github.com/ChildoftheBeast/Udon
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == ParentOfCups.transform) 
        {
            other.gameObject.SetActive(false);
            // Ball should respawn when in cup
            RespawnBall();
        }
    }

    public GameObject BallSpawn;
    private Vector3 Ball_Spawn;
    
    private float maxDistance = 6.5f;
    private float lastMove = 0;

    private void Start()
    {
        Ball_Spawn = BallSpawn.transform.position;
    }
    
    /// <summary>
    /// Automatic Ball Respawn on:
    /// - > 2 Seconds no Movement but also not in hand of a player
    /// - Ball is under the table
    /// - Ball has a distance larger then necessary
    /// </summary>
    private void Update()
    {
        float currentDistance = (BallSpawn.transform.position - gameObject.transform.position).magnitude;
        float currentVelocity = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        VRC_Pickup pickup = (VRC_Pickup) gameObject.GetComponent(typeof(VRC_Pickup));

        if (currentDistance > 0.2 && currentVelocity == 0 && !pickup.IsHeld)
        {
            if (Time.time - lastMove > 2)
            {
                RespawnBall();
            }
        }
        else
        {
            lastMove = Time.time;
        }

        if (
            currentDistance > maxDistance || 
            gameObject.transform.localPosition.y < -12
        )
        {
            RespawnBall();
        }
    }

    public void RespawnBall()
    {
        Networking.SetOwner(Networking.LocalPlayer, Ball);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.position = Ball_Spawn;
    }
    public void Network_RespawnCups()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "RespawnCups");
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
