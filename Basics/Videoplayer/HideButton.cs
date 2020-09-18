
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.Childofthebeast
{
    public class HideButton : UdonSharpBehaviour
    {
        public MeshRenderer Screen;
        public GameObject ScreenUI;
        public GameObject ButtonFill;
        private bool IsActive = true;
        public override void Interact()
        {
            IsActive = !IsActive;
            Screen.enabled = IsActive;
            ScreenUI.SetActive(IsActive);
            ButtonFill.SetActive(!IsActive);
        }
    }
}
