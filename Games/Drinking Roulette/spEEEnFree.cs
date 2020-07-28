
using System.Collections;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Childofthebeast.Games.speen
{
    public class spEEEn : UdonSharpBehaviour
    {
        [UdonSynced] public float rotation = 3.75f;
        private Transform Arrow;
        private float wait = 0;
        private int OldSpeen;
        private int ThisSpeen;
        private Vector3 targetrotation = new Vector3(0, 3.75f, 0);
        private Animator animate;
        public GameObject LoliStep;
        private void Start()
        {
            Arrow = transform.GetChild(0);
            animate = transform.GetComponent<Animator>();
            PatreonStart();
        }
        public override void Interact()
        {
            SyncSpeen();
        }
        private void Update()
        {
            if (wait > 0)
            {
                wait -= Time.deltaTime;
            }
            if (transform.eulerAngles.y != rotation)
            {
                targetrotation.y = rotation;
                transform.eulerAngles = targetrotation;
            }
            PatreonUpdate();
        }
        public void Speen()
        {
            if (wait <= 0)
            {
                Debug.LogWarning("SPEEN: Speen");
                SendCustomNetworkEvent(NetworkEventTarget.All, "SetWait");
                //SetWait();
                SendCustomNetworkEvent(NetworkEventTarget.Owner, "SyncSpeen");
                //SyncSpeen();
            } else
            {
                Debug.LogWarning("SPEEN: Speen > 0");
            }
        }
        public void SyncSpeen()
        {
            Debug.LogWarning("SPEEN: SyncSpeen");
            ThisSpeen = Random.Range(0, 48);
            rotation = 3.75f + (7.5f * ThisSpeen);
        }
        public void SetWait()
        {
            Debug.LogWarning("SPEEN: SetWait");
            animate.SetTrigger("SPEEN");
            wait = 3;
        }

        public void toggleLoliStep()
        {
            LoliStep.SetActive(!LoliStep.activeSelf);
        }
    }
}