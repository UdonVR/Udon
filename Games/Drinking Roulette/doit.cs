
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using Childofthebeast.Games.speen;

namespace Childofthebeast.Games.speen
{
    public class doit : UdonSharpBehaviour
    {
        public spEEEn uwu;
        public override void Interact()
        {
            Debug.LogWarning("SPEEN: doit");
            uwu.Speen();
        }
    }
}
