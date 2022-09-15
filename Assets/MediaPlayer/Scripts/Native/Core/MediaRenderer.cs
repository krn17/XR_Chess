//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaServices
{
    public class MediaRenderer
    {
        public AndroidJavaObject m_MediaRendrer = null;
        public MediaRenderer()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            m_MediaRendrer = new AndroidJavaObject(MediaPlayerConstants.DEFAULT_MEDIA_RENDERER, MediaPlayerConstants.UnityActivity);
        }
    }
}