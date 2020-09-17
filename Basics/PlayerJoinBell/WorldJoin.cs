using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorldJoin : UdonSharpBehaviour
{
    public AudioSource Bell;
    public AudioClip Join;
    public AudioClip Leave;
    private bool JoinEnable = true;
    private bool LeaveEnable = true;
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Join != null && JoinEnable)
        {
            Bell.clip = Join;
            Bell.Play();
        }
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Leave != null && LeaveEnable)
        {
            Bell.clip = Leave;
            Bell.Play();
        }
    }

    public void JoinToggle()
    {
        JoinEnable = !JoinEnable;
    }
    public void LeaveToggle()
    {
        LeaveEnable = !LeaveEnable;
    }
}
