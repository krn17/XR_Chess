//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Core.AudioManager
// Author: Sagar Ahirrao
//=====================================================================

using UnityEngine;

namespace MediaServices
{
    /// <summary>
    /// Native audio manager services for device audio management
    /// </summary>
    public class NativeAudioManager
    {

        /// <summary>
        /// Audio manager native manager 
        /// </summary>
        private AndroidJavaObject m_AudioManager;

        /// <summary>
        /// Native audio event listner
        /// </summary>
        private AudioEventListener audioEventListener;

        /// <summary>
        /// Audio manager class for Action passing
        /// </summary>
        private AudioManager _audioManager;

        /// <summary>
        /// Native audio manager object constructor 
        /// </summary>
        /// <param name="audioManager"></param>
        public NativeAudioManager(AudioManager audioManager, AndroidJavaObject mediaPlayer)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            _audioManager = audioManager;
            m_AudioManager = new AndroidJavaObject(MediaPlayerConstants.MEDIA_AUDIO_MANAGER, MediaPlayerConstants.UnityActivity, mediaPlayer);
        }

        /// <summary>
        /// Toggle mute/unmute
        /// </summary>
        public void ToggleMuteUnMute()
        {
            m_AudioManager.Call("toggleMuteUnMute");
        }

        /// <summary>
        /// Is audio mute
        /// </summary>
        /// <returns></returns>
        public bool isMute()
        {
            return m_AudioManager.Call<bool>("isMute");
        }

        /// <summary>
        /// Get max volum
        /// </summary>
        /// <returns></returns>
        public int GetAudioMaxVolume()
        {
            return m_AudioManager.Call<int>("getMusicStreamMaxVolume");
        }
        /// <summary>
        /// Get max volum
        /// </summary>
        /// <returns></returns>
        public int GetAudioMinVolume()
        {
            return m_AudioManager.Call<int>("getMusicStreamMinVolume");
        }

        /// <summary>
        /// Get Audio level in 0-1 float value
        /// </summary>
        /// <returns></returns>
        public float GetAudioLevel()
        {
            return (float)GetAudioCurrentVolume() / ((float)GetAudioMaxVolume() - (float)GetAudioMinVolume());
        }

        /// <summary>
        /// Set audio level by 0-1 float value
        /// </summary>
        /// <param name="newVolumeLevel"></param>
        public void SetAudioLevel(float newVolumeLevel)
        {
            SetAudioVolume((int)((GetAudioMaxVolume() - GetAudioMinVolume()) * newVolumeLevel));
        }

        /// <summary>
        /// get current audio level
        /// </summary>
        /// <returns></returns>
        public int GetAudioCurrentVolume()
        {
            return m_AudioManager.Call<int>("getMusicStreamCurrentVolume");
        }

        /// <summary>
        /// Set Audio level
        /// </summary>
        /// <returns></returns>
        public void SetAudioVolume(int musicVolume)
        {
            m_AudioManager.Call("setMusicStreamVolume",musicVolume);
        }

        /// <summary>
        /// Add default audio listner to use callback with action events
        /// 
        /// </summary>
        public void AddAudioEventListener()
        {
            audioEventListener = new AudioEventListener(_audioManager);
            m_AudioManager.Call("addAudioEventListener", audioEventListener);
        }

        /// <summary>
        /// Add audio event listner with AudioEventListnerInterface 
        /// Class need to have implemented audioeventlistner object 
        /// </summary>
        /// <param name="iAudioEventListner"></param>
        public void AddAudioEventListener(IAudioEventListner iAudioEventListner)
        {
            audioEventListener = new AudioEventListener(_audioManager, iAudioEventListner);
            m_AudioManager.Call("addAudioEventListener", audioEventListener);
        }

        /// <summary>
        /// Remove Audio event listner
        /// </summary>
        public void RemoveEventListener()
        {
            m_AudioManager.Call("removeAudioEventListener");
        }
                
        internal void OnResume()
        {
            m_AudioManager.Call("onResume");
        }
        
        internal void OnPause()
        {
            m_AudioManager.Call("onPause");
        }
    }
}
