//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaServices
{

    /// <summary>
    /// Media Player Script to be added on unity gameobject
    /// </summary>
    public class MediaPlayer : MonoBehaviour
    {
        /// <summary>
        /// Native videoplayer object
        /// </summary>
        private IMediaPlayer nativePlayerManager;

        #region Public Properties
        /// <summary>
        /// get surface texture id to create video player surface
        /// </summary>
        public IntPtr SurfaceTextureId
        {
            get
            {
                return nativePlayerManager.GetTexture();
            }
        }

        /// <summary>
        /// is video play or pause
        /// </summary>
        public bool isPlaying
        {
            get
            {
                return nativePlayerManager.IsPlaying();
            }
        }

        public Texture2D videoTexture { get; set; }
        public Material videoMaterial { get; set; }
        #endregion


        #region MediaPlayer Events
        public Action<string, int, int, string> onPlayerError;
        public Action<int> onPositionDiscontinuity;
        public Action<bool> onIsPlayingChanged;
        public Action<int, int> onQueuePositionChanged;
        public Action<bool, int> onPlayerStateChanged;
        public Action<long, string> onStartContent;
        public Action<string, long, string, long> onUpdateContent;
        public Action<int> onRepeatModeChanged;
        public Action<bool> onShuffleModeChanged;
        public Action<MediaDetails> onVideoDetails;
        public Action<string> onUpdateSubtitle;
        //public Action<byte[]> onUpdateThumbnail;
        //public Action<int> onMediaTypeChanged;
        public Action<MediaItem> onCurrentSelectedMediaItem;
        public Action<MediaItemQuality> onMediaItemQualityChange;
        #endregion
        private bool isBackgroundPlay;

        #region Player Init

        /// <summary>
        /// initialize player with mediaplayer objects
        /// </summary>
        /// <param name="_mediaRendrer"> media renderer object </param>
        /// <param name="_mediaTrackSelector"> media track selector object </param>
        /// <param name="_mediaLoadController"> media load controller object </param>
        /// <param name="_userAgent"> user agent string </param>
        public void Init(MediaRenderer _mediaRendrer = null, MediaTrackSelector _mediaTrackSelector = null, MediaLoadController _mediaLoadController = null, string _userAgent = null)
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            if (nativePlayerManager != null)
                nativePlayerManager = null;
            nativePlayerManager = new IMediaPlayer(this, _mediaRendrer, _mediaTrackSelector, _mediaLoadController, _userAgent);
        }
        #endregion

        #region Video Texture
        /// <summary>
        /// Get video texture for media
        /// </summary>
        /// <returns>texture2d for material</returns>
        public Texture GetVideoTexture()
        {
            Debug.Log("GetVideoTexture ===> "+SurfaceTextureId);
            videoTexture = Texture2D.CreateExternalTexture(1, 1, TextureFormat.RGBA32, false, false, SurfaceTextureId);
            videoTexture.filterMode = FilterMode.Point;
            return videoTexture;
        }

        /// <summary>
        /// Get video texture for intptr
        /// </summary>
        /// <param name="TexturePtr"> texture int pointer</param>
        /// <returns></returns>
        public Texture GetVideoTexture(IntPtr TexturePtr)
        {
            videoTexture = Texture2D.CreateExternalTexture(1, 1, TextureFormat.RGBA32, false, false, TexturePtr);
            videoTexture.filterMode = FilterMode.Point;
            return videoTexture;
        }

        /// <summary>
        /// use for mesh renderer
        /// </summary>
        /// <param name="videoObject"></param>
        public void PrepareVideoPlayerSurface(GameObject videoObject)
        {
            if (videoObject.GetComponent<Material>() != null)
            {
                videoMaterial = videoObject.GetComponent<Material>();
                videoMaterial.shader = Shader.Find(MediaPlayerConstants.VideoShader);
                videoMaterial.SetTexture("_MainTex", GetVideoTexture());
            }
            else
            {
                videoMaterial = new Material(Shader.Find(MediaPlayerConstants.VideoShader));
                videoMaterial.SetTexture("_MainTex", GetVideoTexture());
                if (videoObject.GetComponent<MeshRenderer>() != null)
                {
                    videoObject.GetComponent<MeshRenderer>().material = videoMaterial;
                }
            }
        }

        /// <summary>
        /// use with mesh/rawimage material
        /// </summary>
        /// <param name="videoObjectMaterial"></param>
        public void PrepareVideoPlayerSurface(Material videoObjectMaterial)
        {
            videoMaterial = videoObjectMaterial;
            videoMaterial.shader = Shader.Find(MediaPlayerConstants.VideoShader);
            videoMaterial.SetTexture("_MainTex", GetVideoTexture());
        }

        #endregion
        
        /// <summary>
        /// Additem and play simultaneously
        /// </summary>
        /// <param name="mediaItem"></param>
        public void AddAndPlay(MediaItem mediaItem) {
            StartCoroutine(ExecuteAddandPlay(mediaItem));
        }

        /// <summary>
        /// courotine to add and play item with delay
        /// </summary>
        /// <param name="mediaItem"></param>
        /// <returns></returns>
        IEnumerator ExecuteAddandPlay(MediaItem mediaItem)
        {
            if (!MediaItem.IsExistInList(GetAllItems(), mediaItem))
            {
               AddItem(mediaItem);
            }
            yield return new WaitForSeconds(0.1f);
            Play(mediaItem);
        }

        public bool IsExistInCurrentList(MediaItem mediaItem) {
            return GetAllItems().Contains(mediaItem);
        }
        /// <summary>
        /// Play media item after adding it to list 
        /// </summary>
        /// <param name="mediaItem"></param>
        public void Play(MediaItem mediaItem)
        {
            nativePlayerManager.Play(mediaItem);
        }


        /// <summary>
        /// Stop media player play
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            nativePlayerManager.Stop();
        }

        /// <summary>
        /// forward/rewind feature
        /// </summary>
        /// <param name="milliseconds">value to seek</param>
        /// <param name="isFastForward">true for forward, false for rewind</param>
        public void SeekTo(long milliseconds, bool isFastForward)
        {
            nativePlayerManager.SeekTo(milliseconds, isFastForward);
        }


        /// <summary>
        /// time bar scrubing seek to position
        /// </summary>
        /// <param name="durationInMs"></param>
        public void SeekBarPosition(long durationInMs)
        {
            nativePlayerManager.SeekBarPosition(durationInMs);
        }

        /// <summary>
        /// forward metod extention
        /// </summary>
        /// <param name="milliseconds"></param>
        public void Forward(long milliseconds = 2000)
        {
            SeekTo(milliseconds, true);
        }

        /// <summary>
        /// rewind metod extention
        /// </summary>
        public void Rewind(long milliseconds = 2000)
        {
            SeekTo(milliseconds, false);
        }

        /// <summary>
        /// toggle play pause
        /// </summary>
        public bool TogglePlayPause()
        {
            return nativePlayerManager.TogglePlayPause();
        }

        public void Retry()
        {
            nativePlayerManager.Retry();
        }

        /// <summary>
        /// is media playing or not
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return nativePlayerManager.IsPlaying();
        }

        public long GetCurrentBufferProgressPosition()
        {
            return nativePlayerManager.GetCurrentBufferProgressPosition();
        }

        public long GetCurrentProgressPosition()
        {
            return nativePlayerManager.GetCurrentProgressPosition();
        }


        #region Playlist Controls
        /// <summary>
        /// play with index of item
        /// </summary>
        /// <param name="index"></param>
        public void Play(int index)
        {
            nativePlayerManager.Play(index);
        }

        /// <summary>
        /// get current media item index 
        /// </summary>
        /// <returns></returns>
        public int GetCurrentIndex()
        {
            return nativePlayerManager.GetCurrentIndex();
        }


        /// <summary>
        /// add item to playlist 
        /// </summary>
        /// <param name="mediaItem"></param>
        /// <returns></returns>
        public bool AddItem(MediaItem mediaItem)
        {
            return nativePlayerManager.AddItem(mediaItem);
        }


        /// <summary>
        /// add multiple items to play list 
        /// </summary>
        /// <param name="mediaItems"></param>
        /// <returns></returns>
        public bool AddItems(ArrayList mediaItems)
        {
            return nativePlayerManager.AddItems(mediaItems);
        }

        /// <summary>
        /// remove media item from play list 
        /// </summary>
        /// <param name="mediaItem"></param>
        /// <returns></returns>
        public bool RemoveItem(MediaItem mediaItem)
        {
            return nativePlayerManager.RemoveItem(mediaItem);
        }

        /// <summary>
        /// remove media item from playlist by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveItem(int index)
        {
            return nativePlayerManager.RemoveItem(index);
        }
        public bool RemoveAllItems()
        {
            return nativePlayerManager.RemoveAllItems();
        }
        /// <summary>
        /// move item to specific index 
        /// </summary>
        /// <param name="newIndex"></param>
        /// <param name="oldIndex"></param>
        public void MoveItem(int newIndex, int oldIndex)
        {
            nativePlayerManager.MoveItem(newIndex, oldIndex);
        }

        /// <summary>
        /// get all item of playlist
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAllItems()
        {
            return nativePlayerManager.GetAllMediaItems();
        }


        /// <summary>
        /// playlist size
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return nativePlayerManager.Size();
        }

        /// <summary>
        /// play next media item
        /// </summary>
        public void Next()
        {
            nativePlayerManager.Next();
        }

        /// <summary>
        /// play previous media item
        /// </summary>
        public void Previous()
        {
            nativePlayerManager.Previous();
        }

        /// <summary>
        /// check if list has next item
        /// </summary>
        /// <returns></returns>
        public bool HasNext()
        {
            return nativePlayerManager.HasNext();
        }


        /// <summary>
        /// check if list has previous item
        /// </summary>
        /// <returns></returns>
        public bool HasPrevious()
        {
            return nativePlayerManager.HasPrevious();
        }

        public long GetCurrentContentDuration()
        {
            return nativePlayerManager.GetCurrentContentDuration();
        }

        public string GetFormattedCurrentContentDuration()
        {
            return nativePlayerManager.GetFormattedCurrentContentDuration();
        }
        public bool ChangeVideoQuality(MediaItem mediaItem, int index)
        {
            return nativePlayerManager.ChangeVideoQuality(mediaItem, index);
        }

        /// <summary>
        /// Repeat mode
        /// Playlist repeat mode. Value can be:-
        /// REPEAT_TOGGLE_MODE_NONE =0 REPEAT_TOGGLE_MODE_ONE = 1 TOGGLE_MODE_ALL =2 
        /// default is TOGGLE_MODE_ALL
        /// </summary>
        /// <param name="repeatMode"></param>
        public void SetRepeatMode(int repeatMode)
        {
            nativePlayerManager.SetRepeatMode(repeatMode);
        }


        /// <summary>
        /// get repeat mode
        /// </summary>
        public int GetRepeatMode()
        {
            return nativePlayerManager.GetRepeatMode();
        }


        /// <summary>
        /// 
        /// </summary>
        public bool ShuffleModeEnabled()
        {
            return nativePlayerManager.ShuffleModeEnabled();
        }


        /// <summary>
        /// shuffle mode
        /// </summary>
        /// <param name="mode"></param>
        public void SetShuffleModeEnabled(bool mode)
        {
            nativePlayerManager.SetShuffleModeEnabled(mode);
        }

        #endregion

        #region MEDIAPLAYER LISTNER
        /// <summary>
        /// addd media player listner
        /// </summary>
        public void AddMediaPlayerListner()
        {
            nativePlayerManager.AddMediaPlayerListener();
        }

        /// <summary>
        /// add media player listner with interface
        /// </summary>
        /// <param name="iMediaEventListner"></param>
        public void AddMediaPlayerListner(IMediaEventListner iMediaEventListner)
        {
            nativePlayerManager.AddMediaPlayerListner(iMediaEventListner);
        }

        /// <summary>
        /// remove listner
        /// </summary>
        public void RemoveMediaPlayerListener()
        {
            nativePlayerManager.RemoveMediaPlayerListener();
        }
        #endregion

        #region Core Components
        public AndroidJavaObject GetNativeObject()
        {
            return nativePlayerManager.GetNativeObject();
        }
        public MediaTrackSelector GetMediaTrackSelector()
        {
            return nativePlayerManager.GetMediaTrackSelector();
        }
        public MediaLoadController GetMediaLoadController()
        {
            return nativePlayerManager.GetMediaLoadController();
        }
        public MediaRenderer GetMediaRendrer()
        {
            return nativePlayerManager.GetMediaRendrer();
        }
        #endregion
        public void setLogEnabled(bool val)
        {
            nativePlayerManager.SetLogEnable(val);
        }
        public void SetBackgroundPlay(bool enabled)
        {
            isBackgroundPlay = enabled;
        }

        public void AppPause()
        {
            if (nativePlayerManager != null)
                nativePlayerManager.OnPause(isBackgroundPlay);
        }
        public void AppResume()
        {
            if (nativePlayerManager != null)
                GetVideoTexture(nativePlayerManager.OnResume());
        }
        #region Mono Methods
        //private void OnApplicationPause(bool pause)
        //{
        //    if (pause && nativePlayerManager != null)
        //    {
        //        nativePlayerManager.OnPause(isBackgroundPlay);
        //    }
        //    if (!pause && nativePlayerManager != null)
        //    {
        //        GetVideoTexture(nativePlayerManager.OnResume());
        //    }
        //}



        private void OnDestroy()
        {
            RemoveMediaPlayerListener();
            nativePlayerManager.OnDestroy();
        }
        #endregion

    }
}
