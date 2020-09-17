using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorldJoin : UdonSharpBehaviour
{
    public AudioSource Bell;
    public AudioClip Join;
    public AudioClip Leave;
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Join != null)
        {
            Bell.clip = Join;
            Bell.Play();
        }
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Leave != null)
        {
            Bell.clip = Leave;
            Bell.Play();
        }
    }
}
