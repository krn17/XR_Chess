//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================

using JMRSDK.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace MediaServices
{
    public class MediaPlayerConstants
    {
        #region Unity Activity
        /// <summary>
        /// UnityPlayer Activity for context passing
        /// </summary>
        public static AndroidJavaObject UnityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        #endregion

        #region Packages 
        /// <summary>
        /// Media player pacakge name
        /// </summary>
        public const string MEDIA_PLAYER_PACKAGE = "in.tesseract.mediaplayer.";

        /// <summary>
        /// Media player base pacakge name
        /// </summary>
        public const string MEDIA_PLAYER_BASE_PACKAGE = MEDIA_PLAYER_PACKAGE + "base.";

        /// <summary>
        /// Media player Core Package
        /// </summary>
        public const string MEDIA_PLAYER_CORE_PACKAGE = MEDIA_PLAYER_PACKAGE + "core.";

        /// <summary>
        /// Media player Listner Pacakge
        /// </summary>
        public const string MEDIA_PLAYER_LISTNER_PACKAGE = MEDIA_PLAYER_PACKAGE + "listener.";

        /// <summary>
        /// Media player util pacakge name
        /// </summary>
        public const string MEDIA_PLAYER_UTIL_PACKAGE = MEDIA_PLAYER_PACKAGE + "util.";

        /// <summary>
        /// Media player media pacakge name
        /// </summary>
        public const string MEDIA_PLAYER_MEDIA_PACKAGE = MEDIA_PLAYER_PACKAGE + "model.media.";
        public const string MEDIA_PLAYER_MEDIA_DETAILS_PACKAGE = MEDIA_PLAYER_PACKAGE + "model.mediaDetails.";
        #endregion


        #region MediaPlayer Classes

        public const string MEDIA_PLAYER = MEDIA_PLAYER_PACKAGE + "MediaPlayer";
        public const string MEDIA_PLAYER_BUILDER = MEDIA_PLAYER + "$Builder";

        public const string DEFAULT_MEDIA_RENDERER = MEDIA_PLAYER_CORE_PACKAGE + "DefaultMediaRenderer";
        public const string DEFAULT_MEDIA_LOAD_CONTROLLER = MEDIA_PLAYER_CORE_PACKAGE + "DefaultMediaLoadController";
        public const string DEFAULT_MEDIA_TRACK_SELECTOR = MEDIA_PLAYER_CORE_PACKAGE + "DefaultMediaTrackSelector";

        public const string MEDIA_AUDIO_MANAGER = MEDIA_PLAYER_CORE_PACKAGE + "AudioManager";
        #endregion
        public const string MEDIA_DETAILS = MEDIA_PLAYER_MEDIA_DETAILS_PACKAGE + "MediaDetails";
        public const string MEDIA_ITEM_QUALTIY = MEDIA_PLAYER_MEDIA_DETAILS_PACKAGE + "MediaItemQuality";
        public const string TRACK_SELECTOR_MODEL = MEDIA_PLAYER_MEDIA_DETAILS_PACKAGE + "TrackSelectorModel";


        #region Listner Classes
        public const string MEDIA_AUDIO_EVENT_LISTNER = MEDIA_PLAYER_LISTNER_PACKAGE + "AudioEventListener";
        public const string MEDIA_PLAYER_EVENT_LISTNER = MEDIA_PLAYER_LISTNER_PACKAGE + "MediaPlayerEventListener";
        #endregion

        #region MediaItem Classes

        public const string MEDIA_ITEM = MEDIA_PLAYER_MEDIA_PACKAGE + "MediaItem";
        public const string MEDIA_ITEM_BUILDER = MEDIA_ITEM + "$Builder";

        public const string DRM_CONFIGURATION = MEDIA_PLAYER_MEDIA_PACKAGE + "DRMConfiguration";
        public const string DRM_CONFIGURATION_BUILDER = DRM_CONFIGURATION + "$Builder";

        public const string SUBTITLE = MEDIA_PLAYER_MEDIA_PACKAGE + "SubTitle";
        public const string SUBTITLE_BUILDER = SUBTITLE + "$Builder";

        public const string EXTENTION = MEDIA_PLAYER_UTIL_PACKAGE + "Constants$Extension";
        public const string MIME_TYPES = MEDIA_PLAYER_UTIL_PACKAGE + "MimeTypes";

        public const string DRM_ENUM = MEDIA_PLAYER_UTIL_PACKAGE + "Constants$DRMEnum";
        public const string UTILITY = MEDIA_PLAYER_UTIL_PACKAGE + "Utility";
        public const string VideoShader = "TMR/VideoUnlitShader";
        public class DRMEnum
        {
            public static AndroidJavaObject UUID_NIL = new AndroidJavaClass(DRM_ENUM).GetStatic<AndroidJavaObject>("NIL_UUID");
            public static AndroidJavaObject COMMON_PUSH_UUID = new AndroidJavaClass(DRM_ENUM).GetStatic<AndroidJavaObject>("COMMON_PUSH_UUID");
            public static AndroidJavaObject WIDE_WINE_UUID = new AndroidJavaClass(DRM_ENUM).GetStatic<AndroidJavaObject>("WIDE_WINE_UUID");
            public static AndroidJavaObject CLEARKEY_UUID = new AndroidJavaClass(DRM_ENUM).GetStatic<AndroidJavaObject>("CLEAR_KEY_UUID");
            public static AndroidJavaObject PLAYER_UUID = new AndroidJavaClass(DRM_ENUM).GetStatic<AndroidJavaObject>("PLAYER_UUID");
        }


        public class Extentions
        {
            public static string HLS_EXTENTION = new AndroidJavaClass(EXTENTION).GetStatic<string>("HLS");
            public static string DASH_EXTENTION = new AndroidJavaClass(EXTENTION).GetStatic<string>("DASH");
            public static string SMOOTH_STREAMING_EXTENTION = new AndroidJavaClass(EXTENTION).GetStatic<string>("SMOOTH_STREAMING");
        }

        public class MimeTypes
        {
            public static string BASE_TYPE_TEXT = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("BASE_TYPE_TEXT");
            public static string BASE_TYPE_APPLICATION = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("BASE_TYPE_APPLICATION");
            public static string TEXT_VTT = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("TEXT_VTT");
            public static string TEXT_SSA = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("TEXT_SSA");
            public static string APPLICATION_MP4 = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_MP4");
            public static string APPLICATION_WEBM = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_WEBM");
            public static string APPLICATION_MPD = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_MPD");
            public static string APPLICATION_M3U8 = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_M3U8");
            public static string APPLICATION_SS = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_SS");
            public static string APPLICATION_ID3 = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_ID3");
            public static string APPLICATION_CEA608 = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_CEA608");
            public static string APPLICATION_CEA708 = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_CEA708");
            public static string APPLICATION_SUBRIP = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_SUBRIP");
            public static string APPLICATION_TTML = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_TTML");
            public static string APPLICATION_TX3G = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_TX3G");
            public static string APPLICATION_MP4VTT = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_MP4VTT");
            public static string APPLICATION_MP4CEA608 = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_MP4CEA608");
            public static string APPLICATION_RAWCC = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_RAWCC");
            public static string APPLICATION_VOBSUB = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_VOBSUB");
            public static string APPLICATION_PGS = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_PGS");
            public static string APPLICATION_SCTE35 = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_SCTE35");
            public static string APPLICATION_CAMERA_MOTION = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_CAMERA_MOTION");
            public static string APPLICATION_EMSG = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_EMSG");
            public static string APPLICATION_DVBSUBS = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_DVBSUBS");
            public static string APPLICATION_EXIF = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_EXIF");
            public static string APPLICATION_ICY = new AndroidJavaClass(MIME_TYPES).GetStatic<string>("APPLICATION_ICY");
        }
        #endregion

        #region Utils
        /// <summary>
        /// Get Arraylist 
        /// </summary>
        /// <param name="nativeItemArray"></param>
        /// <returns></returns>
        public static ArrayList GetArrayListFromObject(AndroidJavaObject nativeItemArray)
        {
            if (nativeItemArray.Call<int>("size") == 0)
                return null;
            ArrayList jo_ArrayList = new ArrayList();
            for (int i = 0; i < nativeItemArray.Call<int>("size"); i++)
            {
                jo_ArrayList.Add(nativeItemArray.Call<AndroidJavaObject>("get", i));
            }

            return jo_ArrayList;
        }

        /// <summary>
        /// Enqueue your actions for multithreading
        /// </summary>
        /// <param name="T"></param>
        public static void MainThreadEnqueue(Action T)
        {
            MainThreadDispatcher.Execute(T);
        }
        #endregion
    }
}
