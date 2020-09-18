
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Video.Components.AVPro;


namespace UdonVR.Childofthebeast
{
    public class Emission : UdonSharpBehaviour
    {
        public MeshRenderer ScreenMesh;
        private Material ScreenMaterial;
        public int Material_Index;
        public bool SharedMerial = true;
        private float CurrentEmission = 1;
        private bool IsOn = true;

        public GameObject[] ButtonFills;

        private void Start()
        {
            if (SharedMerial)
            {
                ScreenMaterial = ScreenMesh.sharedMaterials[Material_Index];
            }
            else
            {
                ScreenMaterial = ScreenMesh.materials[Material_Index];
            }

            ScreenMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

        }
        public void SetHide()
        {
            if (!IsOn)
            {
                SetEmission();
            } else
            {
                ScreenMaterial.SetFloat("_Emission", 0);
                IsOn = false;
                if (ScreenMesh != null)
                    RendererExtensions.UpdateGIMaterials(ScreenMesh);
            }
        }
        public void SetOff()
        {
            if (CurrentEmission != .1f)
            {
                ButtonFills[0].SetActive(true);
                ButtonFills[1].SetActive(false);
                ButtonFills[2].SetActive(false);
                CurrentEmission = .1f;
                SetEmission();
            }
            else Set0();
        }
        public void Set1()
        {
            if (CurrentEmission != 1f)
            {
                ButtonFills[1].SetActive(true);
                ButtonFills[0].SetActive(false);
                ButtonFills[2].SetActive(false);
                CurrentEmission = 1f;
                SetEmission();
            }
            else Set0();
        }
        public void Set2()
        {
            if (CurrentEmission != 2f)
            {
                ButtonFills[2].SetActive(true);
                ButtonFills[1].SetActive(false);
                ButtonFills[0].SetActive(false);
                CurrentEmission = 2f;
                SetEmission();
            }
            else Set0();
        }
        public void Set0()
        {
            ButtonFills[2].SetActive(false);
            ButtonFills[1].SetActive(false);
            ButtonFills[0].SetActive(false);
            CurrentEmission = 0f;
            ScreenMaterial.SetFloat("_Emission", CurrentEmission);
            IsOn = false;
            RendererExtensions.UpdateGIMaterials(ScreenMesh);
        }

        public void SetEmission()
        {
            ScreenMaterial.SetFloat("_Emission", CurrentEmission);
            IsOn = true;
        }
        private void Update()
        {
            if (ScreenMesh != null && IsOn)
                RendererExtensions.UpdateGIMaterials(ScreenMesh);
        }
    }
}
