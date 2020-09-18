using UnityEngine;
using UdonSharp;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace UdonVR.Takato
{
    public class MasterOnly : UdonSharpBehaviour
    {
        public VRCUrlInputField videoURLInputField;
        public UdonSyncVideoPlayer videoPlayer;
        public Toggle masterToggle;
        public Button playButton;
        public Button pauseButton;
        public Button stopButton;
        public Slider timeBar;
        [UdonSynced] public bool syncedMasterOnly;
        private bool _masterOnly;


        private void Start()
        {
            if (Networking.IsMaster && syncedMasterOnly)
            {
                masterToggle.isOn = syncedMasterOnly;
                //MasterTogle();
            }
        }

        public void MasterToggleButton() //Call RunProgram on this method
        {
            Debug.Log("[UdonVR] MasterToggle!");
            if (Networking.IsMaster)
            {
                syncedMasterOnly = masterToggle.isOn;
                videoPlayer.TakeOwner();
            }
            else
                masterToggle.isOn = syncedMasterOnly;
        }

        private void MasterTogle()
        {
            _masterOnly = syncedMasterOnly;

            masterToggle.isOn = _masterOnly;
            playButton.interactable = !_masterOnly;
            pauseButton.interactable = !_masterOnly;
            stopButton.interactable = !_masterOnly;
            videoURLInputField.interactable = !_masterOnly;
            if (videoPlayer.EnableTimeBar())
                timeBar.interactable = !_masterOnly;
        }

        public override void OnOwnershipTransferred()
        {
            if (Networking.IsMaster)
            {
                playButton.interactable = true;
                pauseButton.interactable = true;
                stopButton.interactable = true;
                videoURLInputField.interactable = true;
                timeBar.interactable = videoPlayer.EnableTimeBar();
            }
        }

        public override void OnDeserialization()
        {
            if (!Networking.IsMaster)
            {
                if (_masterOnly != syncedMasterOnly)
                {
                        MasterTogle();
                }
            }
        }
    }
}