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
    public class Subtitle
    {
        public AndroidJavaObject m_SubTitle;
        public string _SubtitleUrl { get; private set; }
        public string _MimeTypes { get; private set; }
        public string _Id { get; private set; }
        public int? _SelectionFlag { get; private set; }
        public string _Lang { get; private set; }

        public Subtitle(string subtitleUrl = "", string mimeTypes = "", string id = "", int? selectionFlag = 1, string lang = "")
        {
            _SubtitleUrl = subtitleUrl;
            _MimeTypes = mimeTypes;
            _Id = id;
            _SelectionFlag = selectionFlag;
            _Lang = lang;

            m_SubTitle = new AndroidJavaObject(MediaPlayerConstants.SUBTITLE_BUILDER, subtitleUrl, mimeTypes).
                    //Call<AndroidJavaObject>("setId", id).
                    //  Call<AndroidJavaObject>("setSelectionFlag", (int)selectionFlag).
                    Call<AndroidJavaObject>("setLanguage", lang).
                    Call<AndroidJavaObject>("build");
        }
        public static Subtitle GetSubTitleItem(AndroidJavaObject m_subtitle)
        {
            if (m_subtitle != null)
                return new Subtitle(m_subtitle.Get<string>("id"), m_subtitle.Get<string>("mimeTypes"), m_subtitle.Get<string>("id"),1/* m_subtitle.Get<int>("selectionFlag")*/, m_subtitle.Get<string>("lang"));
            else
                return null;
        }
    }
}
