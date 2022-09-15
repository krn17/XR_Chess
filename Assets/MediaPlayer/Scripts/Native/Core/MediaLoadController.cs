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
    public class MediaLoadController
    {
        public AndroidJavaObject m_MediaLoadController = null;
        public MediaLoadController()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;
            m_MediaLoadController = new AndroidJavaObject(MediaPlayerConstants.DEFAULT_MEDIA_LOAD_CONTROLLER);
        }
    }
}