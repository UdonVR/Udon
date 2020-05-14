using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using UnityEditor;
using System.Collections;

namespace Childofthebeast.Toys
{
    public class Dice : UdonSharpBehaviour
    {
        public bool DebugThisBoi = false;
        private float DiceBoiSpeed;
        private int DiceBoiIs = 0;

        private GameObject DiceBoi;
        private Transform placeofDiceBoi;
        private Rigidbody DiceBoiuwu;
        private Vector3 DiceBoiSpawn;

        private InputField OutputUwU;
        private bool Go = false;

        private void Start()
        {
            DiceBoi = gameObject.transform.Find("Dice").gameObject;
            placeofDiceBoi = DiceBoi.transform;
            DiceBoiuwu = DiceBoi.GetComponent<Rigidbody>();
            OutputUwU = gameObject.transform.Find("Output").gameObject.transform.Find("InputField").gameObject.GetComponent<InputField>();
            DiceBoiSpawn = DiceBoi.transform.position;
        }
        private void Update()
        {
            if (DebugThisBoi == true) DebugMe();
            if (Networking.LocalPlayer.IsOwner(DiceBoi) && Go == true)
            {
                DiceBoiSpeed = Mathf.Round((DiceBoiuwu.velocity.magnitude) * 100);
                if (DiceBoiSpeed <= 1)
                {
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PDiceNetworkEvent");
                    Go = false;
                }
            }
        }
        public void PDiceNetworkEvent()
        {
            OutputUwU.text = whatisDiceBoi_Good().ToString();
        }

        private int whatisDiceBoi_Good()
        {
            float floatyDiceBoi = 0;
            for (int i = 0; i < 6; i++)
            {
                if (DiceBoi.transform.GetChild(i).position.y > floatyDiceBoi)
                {
                    floatyDiceBoi = DiceBoi.transform.GetChild(i).position.y;
                    DiceBoiIs = i + 1;
                }
            }
            return (DiceBoiIs);
        }
        public void Dropped()
        {
           Go = true;
        }

        public void RespawnDiceboi()
        {
            Networking.SetOwner(Networking.LocalPlayer, DiceBoi);
            DiceBoi.transform.position = DiceBoiSpawn;
        }

        public Text GoVal;
        public void DebugMe()
        {
            GoVal.text = Go.ToString();
        }

    }
}
