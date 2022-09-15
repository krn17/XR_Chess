//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Audio Manager
// Author: Sagar Ahirrao
//=====================================================================

using UnityEngine;
using System;
namespace MediaServices
{
    public class AudioEventListener : AndroidJavaProxy
    {
        private IAudioEventListner audioEventListner;
        private AudioManager _audioManager;
        public AudioEventListener(AudioManager audioManager) : base(MediaPlayerConstants.MEDIA_AUDIO_EVENT_LISTNER)
        {
            _audioManager = audioManager;
        }
        public AudioEventListener(AudioManager audioManager,IAudioEventListner _audioEventListner) : base(MediaPlayerConstants.MEDIA_AUDIO_EVENT_LISTNER)
        {
            _audioManager = audioManager;
            audioEventListner = _audioEventListner;
        }

        void onInitialize(int minMusicStreamVolume, int maxMusicStreamVolume)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => audioEventListner?.onInitialize(minMusicStreamVolume, maxMusicStreamVolume));
            MediaPlayerConstants.MainThreadEnqueue(() => _audioManager.onInitialize?.Invoke(minMusicStreamVolume, maxMusicStreamVolume));
        }
        void onVolumeChange(int currentVolume)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => audioEventListner?.onVolumeChange(currentVolume));
            MediaPlayerConstants.MainThreadEnqueue(() => _audioManager.onVolumeChange?.Invoke(currentVolume));
        }
        void onVolumeMuteUnMute(bool isMute)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => audioEventListner?.onVolumeMuteUnMute(isMute));
            MediaPlayerConstants.MainThreadEnqueue(() => _audioManager.onVolumeMuteUnMute?.Invoke(isMute));
        }
    }

    public interface IAudioEventListner
    {
        void onInitialize(int minMusicStreamVolume, int maxMusicStreamVolume);
        void onVolumeChange(int currentVolume);
        void onVolumeMuteUnMute(bool isMute);
    }
}
