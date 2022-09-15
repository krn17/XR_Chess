using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaServices
{
    /// <summary>
    /// Audio Manager  
    /// </summary>
    public class AudioManager 
    {
        public Action<int, int> onInitialize;
        public Action<int> onVolumeChange;
        public Action<bool> onVolumeMuteUnMute;
        private NativeAudioManager nativeAudioManger;

        public AudioManager(MediaPlayer mediaPlayerObject)
        {
            nativeAudioManger = new NativeAudioManager(this, mediaPlayerObject.GetNativeObject());
        }

        /// <summary>
        /// Toggle Mute for native  
        /// </summary>
        public void ToggleMuteUnMute()
        {
            nativeAudioManger.ToggleMuteUnMute();
        }

        /// <summary>
        /// Is audio manager 
        /// </summary>
        /// <returns></returns>
        public bool isMute()
        {
            return nativeAudioManger.isMute();
        }

        /// <summary>
        /// change volume 
        /// </summary>
        /// <param name="newVolume"></param>
        public void changeVolume(int newVolume)
        {
            nativeAudioManger.SetAudioVolume(newVolume);
        }

        /// <summary>
        /// Change volume level
        /// </summary>
        /// <param name="value"></param>
        public void changeVolumeLevel(float value)
        {
            nativeAudioManger.SetAudioLevel(value);
        }

        /// <summary>
        /// Add audio event listner 
        /// </summary>
        public void AddAudioEventListener()
        {
            nativeAudioManger.AddAudioEventListener();
        }

        public float GetAudioLevel() {
            return nativeAudioManger.GetAudioLevel();
        }
        public int GetAudioCurrentVolume() {
            return nativeAudioManger.GetAudioCurrentVolume();
        }

        public int GetMaxVolume() { 
            return nativeAudioManger.GetAudioMaxVolume();
        }
        public int GetMinVolume() { 
            return nativeAudioManger.GetAudioMinVolume();
        }
        /// <summary>
        /// Audio event listner as interface
        /// </summary>
        /// <param name="iAudioEventListner"></param>
        public void AddAudioEventListener(IAudioEventListner iAudioEventListner)
        {
            nativeAudioManger.AddAudioEventListener(iAudioEventListner);
        }

        /// <summary>
        /// Remove Event listner
        /// </summary>
        public void RemoveAudioEventListener()
        {
            nativeAudioManger.RemoveEventListener();
        }

        public void AppPause()
        {
            if (nativeAudioManger != null)
                nativeAudioManger.OnPause();
        }

        public void AppResume()
        {
            if (nativeAudioManger != null)
                nativeAudioManger.OnResume();
        }
    }
}