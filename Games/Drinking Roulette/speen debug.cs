
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Childofthebeast.Games.speen
{
    public class speendebug : UdonSharpBehaviour
    {
        public spEEEn uwu;

        public GameObject OwnerObj;
        public Transform Arrow;
        public Text Owner;
        public Text rotation;
        public Text CurrentY;

        private void Update()
        {
            Owner.text = "Owner:" + Networking.GetOwner(OwnerObj).displayName;
            rotation.text = "Target:" + uwu.rotation.ToString();
            CurrentY.text = "Current:" + Arrow.eulerAngles.y.ToString();
        }
    }
}
