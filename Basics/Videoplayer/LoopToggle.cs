using UnityEngine;
using UdonSharp;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.SDK3.Video.Components.Base;

namespace UdonVR.Takato
{
    public class LoopToggle : UdonSharpBehaviour
    {
        public BaseVRCVideoPlayer videoPlayer;


        [UdonSynced] bool _loopSynced;
        bool _loop;

        public Toggle loopToggle;

        private void ToggleValue(bool value)
        {
            Debug.Log("[UdonVR] LoopToggleValue!");
            loopToggle.enabled = false;
            loopToggle.isOn = value;
            loopToggle.enabled = true;
        }
        private void Start()
        {
            if (Networking.IsMaster)
            {
                _loopSynced = videoPlayer.Loop;
                _loop = _loopSynced;
                ToggleValue(_loopSynced);
            }
        }

        public void ToggleButton() //Call RunProgram on this method
        {
            Debug.Log("[UdonVR] LoopToggle!");
            if (Networking.IsMaster)
            {
                _loopSynced = loopToggle.isOn;
                DoToggle();
            }
            else
                ToggleValue(_loopSynced);
        }

        private void DoToggle()
        {
            _loop = _loopSynced;
            videoPlayer.Loop = _loopSynced;
            if (!Networking.IsMaster)
            {
                ToggleValue(_loopSynced);
            }
        }



        public override void OnDeserialization()
        {
            if (!Networking.IsMaster)
            {
                if (_loop != _loopSynced)
                {
                    DoToggle();
                }
            }
        }
    }
}