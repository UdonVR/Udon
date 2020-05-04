using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Childofthebeast.Basics.Mirror
{
    public class Mirror : UdonSharpBehaviour
    {
        public GameObject[] Mirrors;
        private InputField MirrorValue;
        private bool Debuger = false;

        private void Start()
        {
            if (Debuger) Debug.Log("Mirrorinitalize");
            MirrorValue = this.gameObject.GetComponent<InputField>();
            if (Debuger) Debug.Log(MirrorValue.ToString());
        }
        public override void Interact() 
        {
            if (MirrorValue.text == "") return;
            if (Debuger) Debug.Log("Interact");
            int v = int.Parse(MirrorValue.text);
            if (Debuger) Debug.Log("MirrorValue = "+v+" = "+Mirrors[v].gameObject.activeSelf.ToString());
            if (Mirrors[v].gameObject.activeSelf)
            {
                if (Debuger) Debug.Log("TurnOff");
                AllOff();
            } else
            {
                if (Debuger) Debug.Log("TurnOn");
                AllOff();
                Mirrors[v].gameObject.SetActive(true);
            }
            MirrorValue.text = "";
        }

        private void AllOff()
        {
            if (Debuger) Debug.Log("AllOff ran");
            foreach (GameObject v in Mirrors)
            {
                v.SetActive(false);
            }
        }
    }
}
