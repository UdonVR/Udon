using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;

namespace childofthebeast.basics
{
    public class animationtrigger : UdonSharpBehaviour
    {
        public Animator Controller;
        public float Speed;
        private void OnTriggerEnter(Collider uwu)
        {
            Debug.Log("Open");
            Controller.SetBool("Open", true);
            Controller.SetBool("StartMe", true);
        }

        private void OnTriggerExit(Collider uwu)
        {
            Debug.Log("Close");
            Controller.SetBool("Open",false);
        }
    }
}
