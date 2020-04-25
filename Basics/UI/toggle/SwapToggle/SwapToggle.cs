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

public class UIToggle : UdonSharpBehaviour
{
    public Toggle thistoggle;
    public GameObject Default_On;
    public GameObject Default_Off;
    public override void Interact()
    {
        Default_Off.SetActive(thistoggle.isOn);
        Default_On.SetActive(!thistoggle.isOn);
    }

}
