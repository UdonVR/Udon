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

public class BeerPong_Ball : UdonSharpBehaviour
{
    public GameObject ParentOfCups;
    public GameObject Ball;

    private void onCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.parent == ParentOfCups) other.gameObject.SetActive(false);
    }
}
