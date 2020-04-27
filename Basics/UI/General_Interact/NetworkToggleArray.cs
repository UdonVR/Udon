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
                if (Networking.IsMaster)
                {
                    Use();
                    return;
                }
                
            }

            if (Enable_AllowedPlayers)
            {
                string Cur_Player = Networking.LocalPlayer.displayName.ToLower();
                foreach (string Player in AllowedPlayers)
                {
                    if (Player.ToLower() == Cur_Player)
                    {
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
                Trogger();
            }
            else
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Trogger");
            }
        }

        public void Trogger()
        {
            if (Owner)
            {
                foreach (GameObject Item in Objects)
                {
                    if (Networking.IsOwner(Networking.LocalPlayer, Item)) Item.SetActive(!Item.activeSelf);
                }

            } else
            {
                foreach (GameObject Item in Objects)
                {
                    Item.SetActive(!Item.activeSelf);
                }
            }

        }
    }
}
