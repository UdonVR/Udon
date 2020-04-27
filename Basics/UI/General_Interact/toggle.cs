/*
  SCRIPT NOT FINISHED
  by Child of the Beast
*/
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using UnityEditor;
using System.Collections;

namespace Childofthebeast.Basics
{
    public class NetworkToggleArray : UdonSharpBehaviour
    {
        public bool LocalOnly = false;
        public bool Master = false;
        public bool Owner = false;
        public bool Enable_AllowedPlayers;
        public string[] AllowedPlayers; 
        public GameObject[] Objects;
        public override void Interact()
        {
            if (Master)
            {
                Debug.Log("Master = True");
                if (Networking.IsMaster)
                {
                    Debug.Log("Is Master");
                    Use();
                    return;
                }
                
            }

            if (Enable_AllowedPlayers)
            {
                Debug.Log("Enable_AllowedPlayers = True");
                string Cur_Player = Networking.LocalPlayer.displayName;
                foreach (string Player in AllowedPlayers)
                {
                    if (Player == Cur_Player)
                    {
                        Debug.Log("PlayerCheck Passed");
                        Use();
                        return;
                    }
                }
            }

            if ((Master == false) && (Enable_AllowedPlayers == false)) Use();
        }

        private void Use()
        {
            if (LocalOnly)
            {
                Debug.Log("Local");
                Trigger();
            }
            else
            {
                Debug.Log("Network");
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Trigger");
            }
        }

        private void Trigger()
        {
            Debug.Log("Trigger");
            if (Owner)
            {
                Debug.Log("Owner");
                foreach (GameObject Item in Objects)
                {
                    if (Networking.IsOwner(Networking.LocalPlayer, Item)) Item.SetActive(!Item.activeSelf);
                }

            } else
            {
                Debug.Log("NotOwner");
                foreach (GameObject Item in Objects)
                {
                    Item.SetActive(!Item.activeSelf);
                }
            }

        }
    }
}
