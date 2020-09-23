
using UdonSharp;
using UdonVR.Takato;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.Takato
{


    public class VideoListManager : UdonSharpBehaviour
    {
        public VRCUrl[] videoUrls;
        public UdonSyncVideoPlayer videoPlayer;

        public Slider _slider;

        public void SendVideoUrl()
        {
            if (_slider.value != -1)
            {
                int index = (int)_slider.value;
                videoPlayer.ChangeVideoUrlVRC(videoUrls[index]);
                _slider.value = -1;
            }
        }
    } 
}
