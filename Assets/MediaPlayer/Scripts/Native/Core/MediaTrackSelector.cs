//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Core.MediaTrackSelector
// Author: Sagar Ahirrao
//=====================================================================


using System.Collections;
using UnityEngine;

namespace MediaServices
{
    public class MediaTrackSelector
    {
        public AndroidJavaObject m_MediaTrackSelector = null;
        public MediaDetails currentMediaDetails;


        /// <summary>
        /// Media Track Selector constructor
        /// </summary>
        public MediaTrackSelector()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            m_MediaTrackSelector = new AndroidJavaObject(MediaPlayerConstants.DEFAULT_MEDIA_TRACK_SELECTOR, MediaPlayerConstants.UnityActivity);
        }

        /// <summary>
        /// Change Video Quality
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bitrateIn_bps"></param>
        /// <param name="isAuto"></param>
        public void ChangeVideoQuality(int? width = -1, int? height = -1, int? bitrateIn_bps = -1, bool isAuto = false)
        {
            m_MediaTrackSelector.Call("changeVideoQuality", (int)width, (int)height, (int)bitrateIn_bps, isAuto);
        }

        /// <summary>
        /// Change Audio Language
        /// </summary>
        /// <param name="trackSelectorModel"></param>
        public void ChangeAudioLanguage(TrackSelectorModel trackSelectorModel)
        {
            m_MediaTrackSelector.Call("changeAudioLangOrQuality", trackSelectorModel.m_trackSelectorModel);
        }

        /// <summary>
        /// change subtitle lanaguage with track model selctor
        /// </summary>
        /// <param name="trackSelectorModel"></param>
        public void ChangeSubtitleLanguage(TrackSelectorModel trackSelectorModel)
        {
            m_MediaTrackSelector.Call("changeSubtitleLanguage", trackSelectorModel.m_trackSelectorModel);
        }

        /// <summary>
        /// Get Current playing Media details
        /// </summary>
        /// <returns></returns>
        public MediaDetails GetCurrentMediaDetail()
        {
            var mediaSelector = m_MediaTrackSelector.Call<AndroidJavaObject>("getCurrentMediaDetail");
            currentMediaDetails = MediaDetails.GetMediaDetails(mediaSelector);
            return currentMediaDetails;
        }

    }

    public class MediaDetails
    {
        public AndroidJavaObject mediaDetails;
        public ArrayList mediaItemQuality;
        public ArrayList subtitleLanguageList;
        public ArrayList audioTrackList;
        public MediaDetails()
        {
            mediaDetails = new AndroidJavaObject(MediaPlayerConstants.MEDIA_DETAILS);
        }
        public static MediaDetails GetMediaDetails(AndroidJavaObject m_mediaDetails)
        {
            MediaDetails mediaDetailsObject = new MediaDetails();
            mediaDetailsObject.mediaDetails = m_mediaDetails;
            mediaDetailsObject.mediaItemQuality = new ArrayList();
            mediaDetailsObject.subtitleLanguageList = new ArrayList();
            mediaDetailsObject.audioTrackList = new ArrayList();
            ArrayList mediaitemQualityArraylist = MediaPlayerConstants.GetArrayListFromObject(m_mediaDetails.Get<AndroidJavaObject>("mediaItemQuality"));
            ArrayList trackArrayList = MediaPlayerConstants.GetArrayListFromObject(m_mediaDetails.Get<AndroidJavaObject>("audioTrackList"));
            ArrayList subtitleArrayList = MediaPlayerConstants.GetArrayListFromObject(m_mediaDetails.Get<AndroidJavaObject>("subtitleLanguageList"));
            if (mediaitemQualityArraylist?.Count > 0)
            {
                foreach (AndroidJavaObject mediaitemQuality in mediaitemQualityArraylist)
                {
                    if (mediaitemQuality != null)
                        mediaDetailsObject.mediaItemQuality.Add(MediaItemQuality.GetMediaItemQualityObject(mediaitemQuality));
                }
            }
            if (trackArrayList?.Count > 0)
            {
                foreach (AndroidJavaObject trackList in trackArrayList)
                {
                    if (trackList != null)
                        mediaDetailsObject.audioTrackList.Add(TrackSelectorModel.GetTrackSelectorModel(trackList));
                }
            }

            if (subtitleArrayList?.Count > 0)
            {
                foreach (AndroidJavaObject subtitleList in subtitleArrayList)
                {
                    if (subtitleList != null)
                        mediaDetailsObject.subtitleLanguageList.Add(TrackSelectorModel.GetTrackSelectorModel(subtitleList));
                }
            }
            return mediaDetailsObject;
        }
    }

    public class MediaItemQuality
    {
        public AndroidJavaObject m_mediaItemQuality;
        public int? width = -1;
        public int? height = -1;
        public float frameRate = -1;
        public int? bitRate_bps = 0;
        public string bitRate_mbps = null;
        public MediaItemQuality()
        {
            m_mediaItemQuality = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_QUALTIY);

        }
        public static MediaItemQuality GetMediaItemQualityObject(AndroidJavaObject trackSelectorObject)
        {
            MediaItemQuality mediaItemQuality = new MediaItemQuality();
            mediaItemQuality.m_mediaItemQuality = trackSelectorObject;
            mediaItemQuality.width = trackSelectorObject.Get<int>("width");
            mediaItemQuality.height = trackSelectorObject.Get<int>("height");
            mediaItemQuality.frameRate = trackSelectorObject.Get<float>("frameRate");
            mediaItemQuality.bitRate_bps = trackSelectorObject.Get<int>("bitRate_bps");
            mediaItemQuality.bitRate_mbps = trackSelectorObject.Get<string>("bitRate_mbps");
            return mediaItemQuality;
        }
    }

    public class TrackSelectorModel
    {
        public AndroidJavaObject m_trackSelectorModel;
        public string mimeType = null;
        public string lang = null;
        public string id = null;
        public int? bitRate = null;
        public TrackSelectorModel()
        {
            m_trackSelectorModel = new AndroidJavaObject(MediaPlayerConstants.TRACK_SELECTOR_MODEL);

        }
        public static TrackSelectorModel GetTrackSelectorModel(AndroidJavaObject trackSelectorObject)
        {
            TrackSelectorModel trackSelector = new TrackSelectorModel();
            trackSelector.m_trackSelectorModel = trackSelectorObject;
            trackSelector.mimeType = trackSelectorObject.Get<string>("mimeType");
            trackSelector.lang = trackSelectorObject.Get<string>("lang");
            trackSelector.id = trackSelectorObject.Get<string>("id");
            try
            {
                trackSelector.bitRate = trackSelectorObject.Get<int>("bitRate");
            }
            catch (System.Exception e)
            {
                Debug.Log("empty bitrate object" + e.Message);
            }
            return trackSelector;
        }
    }
}