
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.Childofthebeast
{
    public class MuteButton : UdonSharpBehaviour
    {
        public AudioSource Audio;
        public GameObject ButtonFill;
        private bool Muted = false;
        public override void Interact()
        {
            Muted = !Muted;
            ButtonFill.SetActive(Muted);
            Audio.mute = Muted;
        }
    }
}
