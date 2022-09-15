//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================

using System;
using UnityEngine;

namespace MediaServices
{
    public class MediaPlayerEventListener : AndroidJavaProxy
    {

        private IMediaEventListner iMediaPlayerListner;
        private MediaPlayer mediaPlayer;

        public MediaPlayerEventListener(MediaPlayer _mediaPlayer) : base(MediaPlayerConstants.MEDIA_PLAYER_EVENT_LISTNER)
        {
            mediaPlayer = _mediaPlayer;
        }

        public MediaPlayerEventListener(MediaPlayer _mediaPlayer,IMediaEventListner mediaPlayerListner) : base(MediaPlayerConstants.MEDIA_PLAYER_EVENT_LISTNER)
        {
            mediaPlayer = _mediaPlayer;
            iMediaPlayerListner = mediaPlayerListner;
        }

        void onPlayerError(string errorMessage, int errorType, int httpErrorCode, string extraInfo)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onPlayerError(errorMessage, errorType, httpErrorCode, extraInfo));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onPlayerError?.Invoke(errorMessage, errorType, httpErrorCode, extraInfo));
        }

        void onPositionDiscontinuity(int reason)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onPositionDiscontinuity(reason));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onPositionDiscontinuity?.Invoke(reason));
        }
        void onIsPlayingChanged(bool isPlaying)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onIsPlayingChanged(isPlaying));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onIsPlayingChanged?.Invoke(isPlaying));

        }
        void onQueuePositionChanged(int previousIndex, int newIndex)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onQueuePositionChanged(previousIndex, newIndex));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onQueuePositionChanged?.Invoke(previousIndex, newIndex));
        }

        void onPlayerStateChanged(bool playWhenReady, int playbackState)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onPlayerStateChanged(playWhenReady, playbackState));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onPlayerStateChanged?.Invoke(playWhenReady, playbackState));
        }

        void onStartContent(long ContentDuration, string formattedContentDuration )
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onStartContent(ContentDuration, formattedContentDuration));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onStartContent?.Invoke(ContentDuration, formattedContentDuration));
        }

        void onUpdateContent(string formattedCurrentPosition, long currentPosition, string formattedBufferedPosition, long bufferedPosition)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onUpdateContent(formattedCurrentPosition, currentPosition, formattedBufferedPosition, bufferedPosition));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onUpdateContent?.Invoke(formattedCurrentPosition, currentPosition, formattedBufferedPosition, bufferedPosition));
        }

        void onRepeatModeChanged(int repeatMode)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onRepeatModeChanged(repeatMode));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onRepeatModeChanged?.Invoke(repeatMode));
        }
        void onShuffleModeChanged(bool isShuffleEnable)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onShuffleModeChanged(isShuffleEnable));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onShuffleModeChanged?.Invoke(isShuffleEnable));
        }
        void onVideoDetails(AndroidJavaObject mediaDetails)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onVideoDetails(MediaDetails.GetMediaDetails(mediaDetails)));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onVideoDetails?.Invoke(MediaDetails.GetMediaDetails(mediaDetails)));
        }
        void onUpdateSubtitle(string subtitleText)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onUpdateSubtitle(subtitleText));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onUpdateSubtitle?.Invoke(subtitleText));
        }
        //void onUpdateThumbnail(byte[] byteArray)
        //{
        //    MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onUpdateThumbnail(byteArray));
        //    MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onUpdateThumbnail?.Invoke(byteArray));
        //}
        //void onMediaTypeChanged(int flag)
        //{
        //    MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onMediaTypeChanged(flag));
        //    MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onMediaTypeChanged?.Invoke(flag));
        //}
        void onCurrentSelectedMediaItem(AndroidJavaObject mediaItem)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onCurrentSelectedMediaItem(MediaItem.GetMediaItemObject(mediaItem)));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onCurrentSelectedMediaItem?.Invoke(MediaItem.GetMediaItemObject(mediaItem)));
        }
        void onMediaItemQualityChange(AndroidJavaObject mediaItemQuality)
        {
            MediaPlayerConstants.MainThreadEnqueue(() => iMediaPlayerListner?.onMediaItemQualityChange(MediaItemQuality.GetMediaItemQualityObject(mediaItemQuality)));
            MediaPlayerConstants.MainThreadEnqueue(() => mediaPlayer.onMediaItemQualityChange?.Invoke(MediaItemQuality.GetMediaItemQualityObject(mediaItemQuality)));
        }
    }

    public interface IMediaEventListner
    {
        void onPlayerError(string errorMessage, int errorType, int httpErrorCode, string extraInfo);
        void onPositionDiscontinuity(int reason);
        void onIsPlayingChanged(bool isPlaying);
        void onQueuePositionChanged(int previousIndex, int newIndex);
        void onPlayerStateChanged(bool playWhenReady, int playbackState);
        void onStartContent(long ContentDuration, string formattedContentDuration);
        void onUpdateContent(string formattedCurrentPosition, long currentPosition, string formattedBufferedPosition, long bufferedPosition);
        void onRepeatModeChanged(int repeatMode);
        void onShuffleModeChanged(bool isShuffleEnable);
        void onVideoDetails(MediaDetails mediaDetails);
        void onUpdateSubtitle(string subtitleText);
        //void onUpdateThumbnail(byte[] byteArray);
        //void onMediaTypeChanged(int flag);
        void onCurrentSelectedMediaItem(MediaItem mediaItem);
        void onMediaItemQualityChange(MediaItemQuality mediaItemQuality);

    }

}
