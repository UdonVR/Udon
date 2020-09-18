
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.Childofthebeast
{
    public class Settings_Button : UdonSharpBehaviour
    {
        public GameObject SettingsScreen;
        public GameObject HelpScreen;

        public override void Interact()
        {
            SettingsScreen.SetActive(!SettingsScreen.activeSelf);
            HelpScreen.SetActive(false);
        }
    }
}
