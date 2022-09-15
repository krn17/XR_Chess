//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace MediaServices
{
    /// <summary>
    /// Native Media player manager class handles all media playback and control methods
    /// </summary>
    public class IMediaPlayer
    {
        #region Private Members

        /// <summary>
        /// Media player android java object 
        /// </summary>
        private AndroidJavaObject m_MediaPlayer;

        /// <summary>
        /// Media Player track Selector class object
        /// </summary>
        private MediaTrackSelector mediaTrackSelector;

        /// <summary>
        /// media load controller object
        /// </summary>
        private MediaLoadController mediaLoadController;

        /// <summary>
        /// media render object 
        /// </summary>
        private MediaRenderer mediaRendrer;

        /// <summary>
        /// Media player object
        /// </summary>
        private MediaPlayer mediaPlayer;
        #endregion

        #region Public Members

        /// <summary>
        /// Media player Event listner interface object
        /// </summary>
        public MediaPlayerEventListener mediaPlayerEventListener;

        #endregion

        /// <summary>
        /// Native Media Player default constructor
        /// </summary>
        internal IMediaPlayer(MediaPlayer _mediaPlayer, MediaRenderer _mediaRendrer = null, MediaTrackSelector _mediaTrackSelector = null, MediaLoadController _mediaLoadController = null, string _userAgent = null)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            mediaPlayer = _mediaPlayer;
            if (_mediaTrackSelector == null)
                mediaTrackSelector = new MediaTrackSelector();
            else
                mediaTrackSelector = _mediaTrackSelector;
            if (_mediaLoadController == null)
                mediaLoadController = new MediaLoadController();
            else
                mediaLoadController = _mediaLoadController;
            if (_mediaRendrer == null)
                mediaRendrer = new MediaRenderer();
            else
                mediaRendrer = _mediaRendrer;
            AndroidJavaObject m_MediaPlayerBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_PLAYER_BUILDER, MediaPlayerConstants.UnityActivity);
            if (!string.IsNullOrEmpty(_userAgent))
            {
                m_MediaPlayer = m_MediaPlayerBuilder.Call<AndroidJavaObject>("setMediaRenderer", mediaRendrer.m_MediaRendrer).
                Call<AndroidJavaObject>("setMediaTrackSelector", mediaTrackSelector.m_MediaTrackSelector).
                Call<AndroidJavaObject>("setMediaLoadControl", mediaLoadController.m_MediaLoadController).
                Call<AndroidJavaObject>("setUserAgent", _userAgent).
                Call<AndroidJavaObject>("build");
            }
            else {
                m_MediaPlayer = m_MediaPlayerBuilder.Call<AndroidJavaObject>("setMediaRenderer", mediaRendrer.m_MediaRendrer).
                   Call<AndroidJavaObject>("setMediaTrackSelector", mediaTrackSelector.m_MediaTrackSelector).
                   Call<AndroidJavaObject>("setMediaLoadControl", mediaLoadController.m_MediaLoadController).
                   Call<AndroidJavaObject>("build");
            }
            OnCreate();
        }

        #region Get Media

        public AndroidJavaObject GetNativeObject()
        {
            return m_MediaPlayer;
        }
        public MediaTrackSelector GetMediaTrackSelector()
        {
            return mediaTrackSelector;
        }
        public MediaLoadController GetMediaLoadController()
        {
            return mediaLoadController;
        }
        public MediaRenderer GetMediaRendrer()
        {
            return mediaRendrer;
        }

        #endregion

        /// <summary>
        /// Set is logenabled
        /// </summary>
        /// <param name="isLogEnable"></param>
        public void SetLogEnable(bool isLogEnable)
        {
            m_MediaPlayer.Call("setLogEnable", isLogEnable);
        }

        #region Life Cycle Methods

        /// <summary>
        /// On create method to be called in start method
        /// </summary>
        internal void OnCreate()
        {
            m_MediaPlayer.Call("onCreate");
        }

        /// <summary>
        /// To be called on meida player resumes OnApplicationFocus
        /// </summary>
        /// <param name="willPlayInBackground">is background play was enabled 
        /// set True for yes and False for no</param>
        internal IntPtr OnResume()
        {
            return (IntPtr)m_MediaPlayer.Call<int>("onResume");
        }

        /// <summary>
        /// To be called on application pause
        /// </summary>
        /// <param name="willPlayInBackground">set is background play is enabled 
        /// set True for yes and False for no </param>
        /// <returns></returns>
        internal void OnPause(bool willPlayInBackground = false)
        {
            m_MediaPlayer.Call("onPause", willPlayInBackground);
        }

        /// <summary>
        /// call on application destroy/to release media player
        /// </summary>
        internal void OnDestroy()
        {
            m_MediaPlayer.Call("onDestroy");
        }

        #endregion


        #region Texture Methods

        /// <summary>
        /// get texture id pointer refeance use it to create external texture from IntPtr
        /// </summary>
        /// <returns>IntPtr for texture referance</returns>
        internal IntPtr GetTexture()
        {
            IntPtr texturePtr = (IntPtr)m_MediaPlayer.Call<int>("getTextureId");
            return texturePtr;
        }

        /// <summary>
        /// update texture for video play on surface
        /// </summary>
        internal void UpdateTexture()
        {
            m_MediaPlayer.Call("updateTexture");
        }

        #endregion

        #region Media player control methods

        /// <summary>
        /// Play content with media item object
        /// </summary>
        /// <param name="mediaItem"> set media item details and pass object to play content</param>
        internal void Play(MediaItem mediaItem)
        {
            m_MediaPlayer.Call("play", mediaItem.m_MediaItem);
        }


        /// <summary>
        /// Stop the media player
        /// </summary>
        /// <returns></returns>
        internal void Stop()
        {
            m_MediaPlayer.Call("stop");
        }

        /// <summary>
        /// seekto content for time bar scrubbing and  fast forward play
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="isFastForward"> set true forwad and false for rewind</param>
        public void SeekTo(long milliseconds, bool isFastForward)
        {
            m_MediaPlayer.Call("seekTo", milliseconds, isFastForward);
        }

        /// <summary>
        /// when dragging seekbar update player current playing position
        /// </summary>
        /// <param name="durationInMs"></param>
        public void SeekBarPosition(long durationInMs)
        {
            m_MediaPlayer.Call("seekBarPosition", durationInMs);
        }

        public long GetCurrentBufferProgressPosition()
        {
            return m_MediaPlayer.Call<long>("getCurrentBufferProgressPosition");
        }
        public long GetCurrentProgressPosition()
        {
            return m_MediaPlayer.Call<long>("getCurrentProgressPosition");
        }
        /// <summary>
        /// Toggle play pause mode
        /// </summary>
        internal bool TogglePlayPause()
        {
            return m_MediaPlayer.Call<bool>("togglePlayPause");
        }

        /// <summary>
        /// Get is player is playing
        /// </summary>
        /// <returns> true for if player is playing false for pause or not playing content</returns>
        internal bool IsPlaying()
        {
            return m_MediaPlayer.Call<bool>("isPlaying");
        }
        public bool ChangeVideoQuality(MediaItem mediaItem, int index)
        {
            return m_MediaPlayer.Call<bool>("changeVideoQuality", mediaItem.m_MediaItem, index);
        }
        #endregion

        #region MediaITems PlayList
        /// <summary>
        /// Play song by index from the playlist
        /// </summary>
        /// <param name="index"></param>
        internal void Play(int index)
        {
            m_MediaPlayer.Call("play", index);
        }

        /// <summary>
        /// Get current media index in playlist
        /// </summary>
        /// <returns></returns>
        internal int GetCurrentIndex()
        {
            return m_MediaPlayer.Call<int>("getCurrentIndex");
        }

        /// <summary>
        /// Add single media item to playlist
        /// </summary>
        /// <param name="mediaItem"> media file object</param>
        /// <returns>true for successfully added 
        /// false for error in adding item</returns>
        internal bool AddItem(MediaItem mediaItem)
        {
            return m_MediaPlayer.Call<bool>("addItem", mediaItem.m_MediaItem);
        }

        /// <summary>
        /// Add arraylist of media items to play list
        /// </summary>
        /// <param name="mediaItems">array list of media items</param>
        /// <returns>true for mediaitems successfully added 
        /// false for error in adding mediaitems</returns>
        internal bool AddItems(ArrayList mediaItems)
        {
            AndroidJavaObject mediaArrayObject = new AndroidJavaObject("java.util.ArrayList");
            //for (int i = 0; i < mediaItems.Count; ++i)
            //{
            //    mediaArrayObject.CallStatic<bool>("add", (MediaItem)mediaItems[i].m_MediaItem);
            //}
            foreach (MediaItem item in mediaItems) { 
               mediaArrayObject.Call<bool>("add", item.m_MediaItem);
            }
            return m_MediaPlayer.Call<bool>("addItems", mediaArrayObject);
        }

        /// <summary>
        /// Remove media item from the play list
        /// </summary>
        /// <param name="mediaItem"> media item object</param>
        /// <returns>true for successfully removed 
        /// false for error in removing item</returns>
        public bool RemoveItem(MediaItem mediaItem)
        {
            return m_MediaPlayer.Call<bool>("removeItem", mediaItem.m_MediaItem);
        }

        /// <summary>
        /// Remove media item from play list by index of item
        /// </summary>
        /// <param name="index">index of mediaitem to be removed from list</param>
        /// <returns>true for successfully added 
        /// false for error in adding item</returns>
        public bool RemoveItem(int index)
        {
            return m_MediaPlayer.Call<bool>("removeItem", index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveAllItems()
        {
            return m_MediaPlayer.Call<bool>("removeAllItems");
        }

        /// <summary>
        /// Playlist repeat mode. Value can be:-
        /// REPEAT_TOGGLE_MODE_NONE =0 REPEAT_TOGGLE_MODE_ONE = 1 TOGGLE_MODE_ALL =2
        /// default is TOGGLE_MODE_ALL
        /// </summary>
        /// <param name="repeatMode"></param>
        public void SetRepeatMode(int repeatMode)
        {
            m_MediaPlayer.Call("setRepeatMode", repeatMode);
        }

        /// <summary>
        /// Get repeat mode
        /// </summary>
        public int GetRepeatMode()
        {
            return m_MediaPlayer.Call<int>("getRepeatMode");
        }

        /// <summary>
        /// check shuffle mode is enable or not
        /// </summary>
        /// <returns></returns>
        public bool ShuffleModeEnabled()
        {
            return m_MediaPlayer.Call<bool>("shuffleModeEnabled");
        }

        /// <summary>
        /// Set shuffle mode
        /// </summary>
        /// <param name="isShuffleEnable"></param>
        public void SetShuffleModeEnabled(bool isShuffleEnable)
        {
            m_MediaPlayer.Call("setShuffleModeEnabled", isShuffleEnable);
        }

        /// <summary>
        /// Toggle between shuffle mode 
        /// </summary>
        /// <returns></returns>
        public bool ToggleShuffleMode()
        {
            return m_MediaPlayer.Call<bool>("toggleShuffleMode");
        }

        /// <summary>
        /// move item to new index of play list
        /// </summary>
        /// <param name="newIndex">new index</param>
        /// <param name="oldIndex">old index</param>
        public void MoveItem(int newIndex, int oldIndex)
        {
            m_MediaPlayer.Call("moveItem", newIndex, oldIndex);
        }

        /// <summary>
        /// Get arraylist of all media items in current playlist
        /// </summary>
        /// <returns> 
        /// Arraylist of MediaItems
        /// </returns>
        public ArrayList GetAllMediaItems()
        {
            var MediaItemArray = m_MediaPlayer.Call<AndroidJavaObject>("getAllItems");
            ArrayList MediaItemList = new ArrayList();
            for (int i = 0; i < MediaItemArray.Call<int>("size"); i++)
            {
                var mediaobject = MediaItemArray.Call<AndroidJavaObject>("get", i);
                MediaItem mediaItem = MediaItem.GetMediaItemObject(mediaobject);
                MediaItemList.Add(mediaItem);
            }
            return MediaItemList;
        }

        /// <summary>
        /// Gives size of current playlist
        /// </summary>
        /// <returns>
        /// number of media items are added in playlist
        /// </returns>
        public int Size()
        {
            return m_MediaPlayer.Call<int>("size");
        }

        /// <summary>
        /// Play next media item in playlist
        /// </summary>
        public void Next()
        {
            m_MediaPlayer.Call("next");
        }

        /// <summary>
        /// play previous media item in playlist
        /// </summary>
        public void Previous()
        {
            m_MediaPlayer.Call("previous");
        }

        /// <summary>
        /// checks if the in list current playing meida has next media in track
        /// </summary>
        /// <returns> 
        /// True: Having next media item in playlist
        /// False: Not having next media item in playlist 
        /// </returns>
        public bool HasNext()
        {
            return m_MediaPlayer.Call<bool>("hasNext");
        }

        /// <summary>
        /// checks if the in list current playing meida has previous media in track
        /// </summary>
        /// <returns>
        /// True: Having previous media item in playlist
        /// False: Not having previous media item in playlist
        /// </returns>
        public bool HasPrevious()
        {
            return m_MediaPlayer.Call<bool>("hasPrevious");
        }

        public long GetCurrentContentDuration()
        {
            return m_MediaPlayer.Call<long>("getCurrentContentDuration");
        }

        public string GetFormattedCurrentContentDuration()
        {
            return m_MediaPlayer.Call<string>("getFormattedCurrentContentDuration");
        }

        #endregion

        #region Listner handler

        /// <summary>
        /// Default media player event listner adding
        /// Will give callback through actions only
        /// </summary>
        public void AddMediaPlayerListener()
        {
            if(mediaPlayerEventListener == null)
                mediaPlayerEventListener = new MediaPlayerEventListener(mediaPlayer);
            m_MediaPlayer.Call("addMediaEventListener", mediaPlayerEventListener);
        }

        /// <summary>
        /// Mediaplayer event listner with IMediaPlayerListner interface as callback listner
        /// </summary>
        /// <param name="iMediaPlayerListner">interface object</param>
        public void AddMediaPlayerListner(IMediaEventListner iMediaPlayerListner)
        {
            if (mediaPlayerEventListener == null)
                mediaPlayerEventListener = new MediaPlayerEventListener(mediaPlayer,iMediaPlayerListner);
            m_MediaPlayer.Call("addMediaEventListener", mediaPlayerEventListener);
        }

        /// <summary>
        /// remove media player listner to deallocate mediaplayer listner services
        /// </summary>
        public void RemoveMediaPlayerListener()
        {
            m_MediaPlayer.Call("removeMediaEventListener");
        }

        /// <summary>
        /// media player event listner object
        /// </summary>
        /// <returns>MediaPlayerEventListner for action events</returns>
        public MediaPlayerEventListener GetDefaultMediaPlayerListner()
        {
            return mediaPlayerEventListener;
        }

        public void Retry()
        {
            m_MediaPlayer.Call("retry");
        }

        public bool GetPlaybackState()
        {
            return m_MediaPlayer.Call<bool>("getPlaybackState");
        }

        public void SetIsPlaying(bool playWhenReady)
        {
            m_MediaPlayer.Call("setIsPlaying", playWhenReady);
        }

        public bool PlayWhenReady()
        {
            return m_MediaPlayer.Call<bool>("playWhenReady");
        }

        public void SetStreamVolume(int silentOrGain)
        {
            m_MediaPlayer.Call("setStreamVolume", silentOrGain);
        }

        public int GetStreamVolume()
        {
            return m_MediaPlayer.Call<int>("getStreamVolume");
        }

        public int GetMediaType()
        {
            return m_MediaPlayer.Call<int>("getMediaType");
        }
        #endregion

    }
}
