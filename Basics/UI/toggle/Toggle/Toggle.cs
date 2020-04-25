/*
//Script by Child of the beast
//https://github.com/ChildoftheBeast/Udon
*/
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;

public class Toggle : UdonSharpBehaviour
{
    public Toggle thistoggle;
    public GameObject Object;
    public override void Interact()
    {
        Default_Off.SetActive(thistoggle.isOn);
    }

}
