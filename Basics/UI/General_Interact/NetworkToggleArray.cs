/*
//Script by Child of the beast
//https://github.com/ChildoftheBeast/Udon
//
//If you use a script from this github:
//please link back to this github in your world so it can potentially help others.
//Thank you.
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
        public bool Master = false; //Master Toggle
        public bool Owner = false; //Owner Toggle
        public bool Enable_AllowedPlayers; //player list
        public string[] AllowedPlayers; 
        public GameObject[] Objects;
        public override void Interact()
        {
            if (Master) //checking if Master is enabled
            {
                if (Networking.IsMaster) //Checking to see if the player who used the button is the master
                {
                    Use();
                    return;
                }
                
            }

            if (Enable_AllowedPlayers) // checking to see if the player list is enabled
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
