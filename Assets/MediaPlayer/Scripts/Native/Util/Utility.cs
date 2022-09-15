//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
//=====================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaServices
{
    public class Utility
    {
        public static string GetFormattedDuration (long duration)
        {
            AndroidJavaClass utilityNativeClass = new AndroidJavaClass(MediaPlayerConstants.UTILITY);
            AndroidJavaObject instanceObject = utilityNativeClass.GetStatic<AndroidJavaObject>("INSTANCE");
            return instanceObject.Call<string>("getFormattedDuration", duration);
        }
    }
}