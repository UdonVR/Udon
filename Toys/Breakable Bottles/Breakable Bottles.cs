/*
//Script by Child of the beast
//https://github.com/ChildoftheBeast/Udon
//
//This still has bugs in it being worked out, be warned
//
*/

using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;

public class Bottlebreak : UdonSharpBehaviour
{
    public Rigidbody Bottle;
    public GameObject Bottle_Whole;
    public GameObject Bottle_Broken;
    public float Tolerance;
    public float Chance;
    VRCPlayerApi localPlayerApi;
    public bool Debug;
    public Text Debug_Mag;
    public Text Debug_Vel;
    private float oldVelocity;
    public bool InEditor;
    public Text Debug_Broken;
    [UdonSynced] public bool Bottle_broken;

    // Start is called before the first frame update

    private void Start()
    {
        localPlayerApi = Networking.LocalPlayer;
        oldVelocity = Bottle.velocity.magnitude;
    }
    private void Update()
    {
        if (Debug == true) DebugMe();
        if (Bottle_broken == true)
        {
            Break();
            return;
        }

        if (Networking.IsOwner(gameObject) == true)
        {
            if (oldVelocity > Tolerance && Bottle.velocity.magnitude < 1)
            {
                BreakCheck();
            }
            oldVelocity = Bottle.velocity.magnitude;
        }

            

    }
    private void BreakCheck()
    {
        //localPlayerApi.isMaster
        if (Debug == true) Debug_Mag.color = Color.blue;
        float val = Random.Range(1, 100);
        if (Chance >= val) Break();
    }
    private void Break()
    {
        Bottle_broken = true;
        Bottle_Whole.SetActive(false);
        Bottle_Broken.SetActive(true);
    }

    private void DebugMe()
    {
        Debug_Mag.color = Color.white;
        Vector3 Bottle_Velocity = Bottle.velocity;
        Debug_Vel.text = Bottle_Velocity.ToString();
        Debug_Mag.text = Bottle_Velocity.magnitude.ToString();
        Debug_Broken.text = Bottle_broken.ToString();
    }
}
