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
    [Serializable]
    public class MediaItem
    {
        public AndroidJavaObject m_MediaItem;
        public string _mediaUrl { get; set; }
        public bool _isLive { get; set; }
        public string _title { get; set; }
        public string _Extention { get; set; }
        public string _thumbnailUrl { get; set; }
        public DRMConfiguration _drmConfiguration { get; set; }

        public Subtitle _subTitle { get; set; }

        public MediaItem(string url, bool isLiveTv = false, DRMConfiguration drmConfiguration = null, string title = null, string Extention = null, Subtitle subTitle = null, string tumbnailURL = null, int algo=0)
        {
            _mediaUrl = url;
            _isLive = isLiveTv;
            _drmConfiguration = drmConfiguration;
            _thumbnailUrl = tumbnailURL;
            _subTitle = subTitle;
            _title = title;
            _Extention = Extention;
            if(algo == 1) //DEFAULT //SONYLIVE
            {
                var mediaItemBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_BUILDER, url);
                m_MediaItem = mediaItemBuilder.
                    Call<AndroidJavaObject>("setTitle", _title).
                    Call<AndroidJavaObject>("build");
            }
            else if(algo == 2) //HOT STAR
            {
                Debug.Log("BUILDER PATH ==>" + MediaPlayerConstants.MEDIA_ITEM_BUILDER);
                var mediaItemBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_BUILDER, url);
                m_MediaItem = mediaItemBuilder.
                    Call<AndroidJavaObject>("setDRMConfiguration", _drmConfiguration?.m_DrmConfiguration).
                    Call<AndroidJavaObject>("setTitle", _title).
                    Call<AndroidJavaObject>("build");
            }
            else if (algo == 3) //LIONSGATE
            {
                var mediaItemBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_BUILDER, url);
                m_MediaItem = mediaItemBuilder.
                    Call<AndroidJavaObject>("setDRMConfiguration", _drmConfiguration?.m_DrmConfiguration).
                    Call<AndroidJavaObject>("setTitle", _title).
                    Call<AndroidJavaObject>("build");
            }
            else if(algo == 4)
            {
                var mediaItemBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_BUILDER, url);
                m_MediaItem = mediaItemBuilder.
                    Call<AndroidJavaObject>("setDRMConfiguration", _drmConfiguration?.m_DrmConfiguration).
                    Call<AndroidJavaObject>("setTitle", _title).
                    Call<AndroidJavaObject>("build");
            }
            else if(algo == 5)
            {
                var mediaItemBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_BUILDER, url);
                m_MediaItem = mediaItemBuilder.
                    Call<AndroidJavaObject>("setDRMConfiguration", _drmConfiguration?.m_DrmConfiguration).
                    Call<AndroidJavaObject>("setTitle", _title).
                    Call<AndroidJavaObject>("build");
            }
            else if (algo == 6)//ZEE5
            {
                var mediaItemBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_BUILDER, url);
                m_MediaItem = mediaItemBuilder.
                    Call<AndroidJavaObject>("setDRMConfiguration", _drmConfiguration?.m_DrmConfiguration).
                    Call<AndroidJavaObject>("setExtension", Extention).
                    Call<AndroidJavaObject>("setTitle", _title).
                    Call<AndroidJavaObject>("setSubtitle", subTitle?.m_SubTitle).
                    Call<AndroidJavaObject>("setThumbnailUrl", _thumbnailUrl).
                    Call<AndroidJavaObject>("build");
            }
            else
            {
                var mediaItemBuilder = new AndroidJavaObject(MediaPlayerConstants.MEDIA_ITEM_BUILDER, url);
                m_MediaItem = mediaItemBuilder.Call<AndroidJavaObject>("setIsLiveTv", _isLive).
                    Call<AndroidJavaObject>("setDRMConfiguration", _drmConfiguration?.m_DrmConfiguration).
                    Call<AndroidJavaObject>("setExtension", Extention).
                    Call<AndroidJavaObject>("setTitle", _title).
                    Call<AndroidJavaObject>("setSubtitle", subTitle?.m_SubTitle).
                    Call<AndroidJavaObject>("setThumbnailUrl", _thumbnailUrl).
                    Call<AndroidJavaObject>("build");
            }

        }
        public static MediaItem GetMediaItemObject(AndroidJavaObject m_MediaObject)
        {
            return new MediaItem(
                m_MediaObject.Get<AndroidJavaObject>("mediaUri").ToString(),
                m_MediaObject.Get<bool>("isLiveTv"),
                DRMConfiguration.GetDRMObject(m_MediaObject.Get<AndroidJavaObject>("drmConfiguration")),
                m_MediaObject.Get<string>("title"),
                m_MediaObject.Get<string>("extension"),
                Subtitle.GetSubTitleItem(m_MediaObject.Get<AndroidJavaObject>("subTitle")),
                m_MediaObject.Get<string>("thumbnailUrl")
                );
        }
        public static bool IsExistInList(ArrayList ListOfObject, MediaItem mediaItem)
        {
            bool isContain = false;
            foreach (MediaItem mediaI in ListOfObject)
            {
                if (mediaI.m_MediaItem == mediaItem.m_MediaItem)
                {
                    isContain = true;
                    break;
                }
                else {
                    isContain = false;
                }
            }
            return isContain;
        }
    }
}