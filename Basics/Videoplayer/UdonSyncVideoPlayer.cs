using System;
using System.Security.Policy;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;

namespace UdonVR.Takato
{
    public class UdonSyncVideoPlayer : UdonSharpBehaviour
    {
        public BaseVRCVideoPlayer videoPlayer;
        public VRCUrl videoURL;
        public bool autoPlay;
        public VRCUrlInputField videoURLInputField;

        public Text videoTime;
        public Slider videoTimeBar;
        public Text masterText;
        public Text ownerText;
        public float syncFrequency = 5;
        public float syncThreshold = 1;

        private float _lastSyncTime = 0;
        private float _delayTime;
        [UdonSynced] private float _videoStartNetworkTime = 0;
        [UdonSynced] private VRCUrl _syncedURL;
        private VRCUrl _loadedVideoURL;
        [UdonSynced] private int _videoNumber = 0;
        private int _loadedVideoNumber = 0;
        [UdonSynced] private bool _ownerPlaying = false;
        [UdonSynced] private bool _ownerPaused = false;
        private bool _paused;
        private bool _waitForSync = false;
        private string _videoDuration = "3:00:000";
        private string _timeFormat = @"m\:ss";
        private bool _isTooLong;
        private bool _forcePlay = false;
        private int _retries;

        private bool _debug = false;

        private void Start()
        {
            //videoPlayer.Loop = false;

            if (Networking.IsMaster && autoPlay)
            {
                _syncedURL = videoURL;
                //videoURLInputField.SetUrl(videoURL);
                //OnURLChanged();
                _videoNumber = 1;
                _delayTime = Time.time + 1f;
                _forcePlay = true;
            }
            if (Networking.LocalPlayer != null)
            {
                masterText.text = Networking.GetOwner(masterText.gameObject).displayName;
                ownerText.text = Networking.GetOwner(gameObject).displayName;
                if (Networking.LocalPlayer.displayName == "Takato" || Networking.LocalPlayer.displayName == "Takato65" || Networking.LocalPlayer.displayName == "child of the beast")
                    _debug = true;
            }
        }
        #region URL_Methods


        public void ChangeVideoUrlVRC(VRCUrl url)
        {
            if (url.Get() != "" && url != _syncedURL)
            {
                _syncedURL = url;
                ChangeVideoUrl();
            }
        }

        public void ChangeVideoUrl()
        {
            //When the Owner changes the URL
            //Debug.Log("[UdonSyncVideoPlayer] URL Changed Start");
            if (Networking.IsOwner(gameObject))
            {
                //Debug.Log("[UdonSyncVideoPlayer] URL Changed Owner");
                
                    videoTimeBar.interactable = false;
                    _videoNumber = _videoNumber + 1;
                    _loadedVideoNumber = _videoNumber;
                    videoPlayer.Stop();

                    videoPlayer.LoadURL(_syncedURL);

                    _ownerPlaying = false;
                    _ownerPaused = false;
                    _videoStartNetworkTime = float.MaxValue;

                    videoURLInputField.SetUrl(VRCUrl.Empty);
                    Debug.Log(string.Format("[UdonSyncVideoPlayer] Video URL Changed to {0}", _syncedURL)); 
            }
        }

        public void OnURLChanged()
        {//When the Owner changes the URL
            //Debug.Log("[UdonSyncVideoPlayer] URL Changed Start");
            if (Networking.IsOwner(gameObject))
            {
                //Debug.Log("[UdonSyncVideoPlayer] URL Changed Owner");
                if (videoURLInputField.GetUrl().Get() != "" && videoURLInputField.GetUrl() != _syncedURL)
                {
                    _syncedURL = videoURLInputField.GetUrl();
                    ChangeVideoUrl();
                }
            }
        }

        #endregion
        public bool EnableTimeBar()
        {
            return _paused && !_isTooLong;
        }

        public override void OnOwnershipTransferred()
        {
            ownerText.text = Networking.GetOwner(gameObject).displayName;
            //Debug.Log($"[UdonSyncVideoPlayer] Owner changed to {Networking.LocalPlayer.displayName}");
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            masterText.text = Networking.GetOwner(masterText.gameObject).displayName;
        }

        private void DebugLog()
        {
            Debug.Log($"[UdonSyncVideoPlayer] ==========================================");
            Debug.Log($"[UdonSyncVideoPlayer] _loadedVideoNumber>> {_videoNumber}");
            Debug.Log($"[UdonSyncVideoPlayer] _ownerPlaying>> {_ownerPlaying}");
            Debug.Log($"[UdonSyncVideoPlayer] _ownerPaused>> {_ownerPaused}");
            Debug.Log($"[UdonSyncVideoPlayer] _paused>> {_paused}");
            Debug.Log($"[UdonSyncVideoPlayer] _waitForSync>> {_waitForSync}");
            Debug.Log($"[UdonSyncVideoPlayer] _syncedURL>> {_syncedURL}");
            Debug.Log($"[UdonSyncVideoPlayer] videoPlayer.IsPlaying>> {videoPlayer.IsPlaying}");
            Debug.Log($"[UdonSyncVideoPlayer] videoPlayer.IsReady>> {videoPlayer.IsReady}");
            Debug.Log($"[UdonSyncVideoPlayer] videoPlayer Owner>> {Networking.GetOwner(gameObject).displayName}");
            Debug.Log($"[UdonSyncVideoPlayer] _forcePlay>> {_forcePlay}");
            Debug.Log($"[UdonSyncVideoPlayer] ==========================================");
        }

        private void Update()
        {
            if (Networking.IsOwner(gameObject))
            {
                SyncVideoIfTime();
            }
            else
            {
                if (_waitForSync)
                {
                    if (_ownerPlaying)
                    {
                        videoPlayer.Play();
                        _waitForSync = false;
                        SyncVideo();
                    }
                }
                else
                {
                    SyncVideoIfTime();
                }
            }
            if (_ownerPlaying && !_ownerPaused && !_isTooLong)
            {
                //videoTime.text = string.Format("{0:N2}/{1:N2}",videoPlayer.GetTime(),videoPlayer.GetDuration());
                if (TimeSpan.MaxValue.TotalSeconds >= videoPlayer.GetTime())
                    videoTimeBar.value = videoPlayer.GetTime();
            }
            if (_debug)
            {
                if (Input.GetKeyDown(KeyCode.P))
                    DebugLog();
            }
            if (Networking.IsMaster)
            {
                if (_forcePlay && autoPlay && Time.time > _delayTime)
                {
                    Debug.Log($"[UdonSyncVideoPlayer] Auto Play URL {_syncedURL}");
                    videoPlayer.PlayURL(_syncedURL);
                    _delayTime = Time.time + 5f;
                    _retries += 1;
                    if (_retries > 5)
                        _forcePlay = false;
                }
            }
            else
            {
                if (_forcePlay && Time.time > _delayTime)
                {
                    Debug.Log($"[UdonSyncVideoPlayer] Watcher Load URL {_syncedURL}");
                    videoPlayer.LoadURL(_syncedURL);
                    _delayTime = Time.time + 5f;
                    _retries += 1;
                    if (_retries > 5)
                        _forcePlay = false;
                }
            }
        }

        public void UpdateDisplay()
        {
            if (!_isTooLong)
                videoTime.text = TimeSpan.FromSeconds(videoTimeBar.value).ToString(_timeFormat) + "/" + _videoDuration;
            else
                videoTime.text = _videoDuration;

            if (videoTimeBar.interactable && _ownerPaused)
            {
                videoPlayer.SetTime(videoTimeBar.value);
                TakeOwner();
            }
        }

        public void TakeOwner()
        {
            //Debug.Log("[UdonSyncVideoPlayer] TakeOWner Called!");
            if (!Networking.IsOwner(gameObject))
            {
                //Debug.Log($"[UdonSyncVideoPlayer] Setting Owner to {Networking.LocalPlayer.displayName}");
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
                ownerText.text = Networking.LocalPlayer.displayName;
                
            }
        }

        public void SyncVideo()
        {//SyncVideo Event: Check if Offset is greater than syncThreshold (video is too far out of sync)
            //Get Offset Time
            float offsetTime;
            offsetTime = Mathf.Clamp(Convert.ToSingle(Networking.GetServerTimeInSeconds()) - _videoStartNetworkTime, 0, videoPlayer.GetDuration());

            if (Mathf.Abs(videoPlayer.GetTime() - offsetTime) > syncThreshold)
            {//Resync video time and log new value
                videoPlayer.SetTime(offsetTime);
                //Debug.Log(string.Format("[UdonSyncVideoPlayer] Syncing Video to {0:N2}", offsetTime));
            }
        }

        public void SyncVideoIfTime()
        {
            if (_ownerPlaying)
            {
                if (!_ownerPaused)
                {
                    if (Time.realtimeSinceStartup - _lastSyncTime > syncFrequency)
                    {
                        _lastSyncTime = Time.realtimeSinceStartup;
                        SyncVideo();
                    }
                }
            }
        }

        private void SetUpTimeBar()
        {
            if (TimeSpan.MaxValue.TotalSeconds >= videoPlayer.GetDuration())
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(videoPlayer.GetDuration());
                if (Networking.IsOwner(gameObject))
                    _isTooLong = false;
                videoTimeBar.maxValue = videoPlayer.GetDuration();

                if (timeSpan.TotalHours >= 1)
                    _timeFormat = @"h\:mm\:ss";
                else if (timeSpan.TotalMinutes >= 1)
                    _timeFormat = @"m\:ss";
                else
                    _timeFormat = @"%s";

                //Debug.Log($"[[UdonSyncVideoPlayer] Time Format is (({_timeFormat}))");
                _videoDuration = timeSpan.ToString(_timeFormat);
            }
            else
            {
                if (Networking.IsOwner(gameObject))
                    _isTooLong = true;
                videoTimeBar.maxValue = 1;
                _videoDuration = "Streaming!";
                videoTimeBar.value = 1;
            }
        }

        #region OnVideo_Overrides

        
        public override void OnVideoLoop()
        {
            Debug.Log("[UdonSyncVideoPlayer] Video Looped");
            if (Networking.IsOwner(gameObject))
            {
                _videoStartNetworkTime = Convert.ToSingle(Networking.GetServerTimeInSeconds());
            }
        }

        public override void OnVideoReady()
        {
            //Debug.Log(string.Format("[UdonSyncVideoPlayer] OnVideoReady {0}", _syncedURL));
            if (Networking.IsOwner(gameObject))
            {//The Owner Plays the video when it's ready
                videoPlayer.Play();
                Debug.Log(string.Format("[UdonSyncVideoPlayer] Owner Play URL {0}", _syncedURL));
                SetUpTimeBar();
            }
            else
            {
                //If the Owner is playing the video, Play it and run SyncVideo
                if (_ownerPlaying)
                {
                    Debug.Log(string.Format("[UdonSyncVideoPlayer] Watcher Play URL {0}", _syncedURL));
                    videoPlayer.Play();
                    SetUpTimeBar();

                    SyncVideo();
                }
                else
                {
                    _waitForSync = true;
                }
                //Turn off forcePlay as video is ready for watcher
                _forcePlay = false;
            }
        }

        public override void OnVideoStart()
        {//Handle OnVideoStart for Owner and Watchers
            //Debug.Log(string.Format("[UdonSyncVideoPlayer] OnVideoStart {0}", _syncedURL));
            videoTimeBar.interactable = false;
            if (Networking.IsOwner(gameObject))
            {//The Owner saves the start time and sets playing to true
                if (!_ownerPaused)
                    _videoStartNetworkTime = Convert.ToSingle(Networking.GetServerTimeInSeconds());
                _ownerPlaying = true;
                _forcePlay = false;
                _ownerPaused = false;
                SetUpTimeBar();
            }
            else
            {//The Watchers pause it and wait for sync
                if (!_ownerPlaying)
                {
                    videoPlayer.Pause();
                    _waitForSync = true;
                }
                SetUpTimeBar();
            }
        }

        public override void OnVideoEnd()
        {
            Debug.Log(string.Format("[UdonSyncVideoPlayer]Video ended URL: {0}", _syncedURL));
        }

        public override void OnVideoError()
        {//On Video Error, log what went wrong
            videoPlayer.Stop();
            Debug.Log(string.Format("[UdonSyncVideoPlayer] Video failed: {0}", _syncedURL));
            //Turn off forcePlay since video has error
            _forcePlay = false;
        }

        #endregion
        public override void OnDeserialization()
        {//Load new video when _videoNumber is changed
            if (!Networking.IsOwner(gameObject))
            {
                if (_videoNumber != _loadedVideoNumber)
                {
                    videoPlayer.Stop();
                    if (_loadedVideoURL != _syncedURL)
                    {
                        videoPlayer.LoadURL(_syncedURL);
                        SyncVideo();
                        _loadedVideoNumber = _videoNumber;
                        _loadedVideoURL = _syncedURL;
                        Debug.Log(string.Format("[UdonSyncVideoPlayer] Playing synced: {0}", _syncedURL));

                        //Turn on forcePlay and set delayTime for repeat tries
                        _delayTime = Time.time + 5f;
                        _forcePlay = true;
                        _retries = 0;
                    }
                    else
                    {
                        Debug.Log("[UdonSyncVideoPlayer] synced Url is the same as last url, url is most likely too long to sync.");
                    }
                }
                if (_ownerPaused != _paused)
                {
                    _paused = _ownerPaused;
                    if (_ownerPaused)
                    {
                        videoPlayer.Pause();
                        if (!_isTooLong)
                            videoTimeBar.interactable = true;
                    }
                    else if (_ownerPlaying)
                    {
                        //videoTimeBar.interactable = false;
                        videoPlayer.Play();
                        SyncVideo();
                    }
                }
            }
        }



        public void StopVideo()
        {
            if (Networking.IsOwner(gameObject))
            {
                _videoStartNetworkTime = 0;
                _ownerPlaying = false;
                _ownerPaused = false;
                videoPlayer.Stop();
                _syncedURL = VRCUrl.Empty;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StopVideoWatcher");
            }
            else
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "StopVideo");
                //TakeOwner();
            }
        }

        public void StopVideoWatcher()
        {
            if (!Networking.IsOwner(gameObject))
            {
                videoPlayer.Stop();
            }
        }

        public void PlayVideo()
        {
            if (!videoPlayer.IsPlaying)
            {
                PauseVideo();
            }
        }

        public void PauseVideo()
        {
            if (Networking.IsOwner(gameObject))
            {
                if (videoPlayer.IsPlaying)
                {
                    videoPlayer.Pause();
                    if (!_isTooLong)
                        videoTimeBar.interactable = true;
                    //_ownerPlaying = false;
                    //_videoPausedNetworkTime = Convert.ToSingle(Networking.GetServerTimeInSeconds());
                    _ownerPaused = true;
                }
                else if (_videoStartNetworkTime != 0)
                {
                    videoTimeBar.interactable = false;

                    float videoCurrentTime = videoPlayer.GetTime();

                    if (!_isTooLong && videoTimeBar.value != videoCurrentTime)
                    {
                        videoCurrentTime = videoTimeBar.value;
                        _videoStartNetworkTime = Convert.ToSingle(Networking.GetServerTimeInSeconds()) - videoCurrentTime;
                        _lastSyncTime = Time.realtimeSinceStartup;
                        videoPlayer.Play();
                        SyncVideo();
                    }
                    else
                    {
                        _videoStartNetworkTime = Convert.ToSingle(Networking.GetServerTimeInSeconds()) - videoCurrentTime;
                        _lastSyncTime = Time.realtimeSinceStartup;
                        videoPlayer.Play();
                    }
                    //_ownerPlaying = true;
                    //_ownerPaused = false;
                }
            }
            else
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "PauseVideo");
                //TakeOwner();
            }
        }
    }
}