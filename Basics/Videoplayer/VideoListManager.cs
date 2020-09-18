
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

        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
        }

        public void SendVideoUrl()
        {
            int index = (int)_slider.value;
            videoPlayer.ChangeVideoUrlVRC(videoUrls[index]);
        }
    } 
}
