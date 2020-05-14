using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using UnityEditor;
using System.Collections;

namespace Childofthebeast.Toys
{
    public class PartyDicePickup : UdonSharpBehaviour
    {
        public GameObject DiceBoi;
        private UdonBehaviour uwu;

        private void Start()
        {
            uwu = (UdonBehaviour)DiceBoi.transform.GetComponent(typeof(UdonBehaviour));
        }
        public override void OnDrop()
        {
            uwu.SendCustomEvent("Dropped");
        }
    }
}
