//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MediaServices;
using System.Collections;

/// <summary>
/// Media player UI extended with media player 
/// add Interface IMediaEventListner for media events, IAudioEventListner for audio manager events
/// </summary>
public class MediaPlayerLiteUI : MediaPlayer, IAudioEventListner
{
    #region Media Player object 
    private AudioManager audioManagerObject;
    private MediaDetails currentMediaDetails;
    #endregion

    #region Public Properties
    public Material videoPlayerObject;
    public VideoProgressBar progressBar;
    #endregion

    #region UI
    public Slider volumeSlider;
    public Button muteButton;
    public Toggle Shuffle;
    public Text totalDurationText;
    public Text ProgressDurationText;
    public Button NextButton;
    public Button PrevButton;
    public Text LogTextfield;
    public Text SubtitleField;
    public Text BufferDurationText;
    private ArrayList currentPlayList = new ArrayList();

    #endregion

    #region Mono Behaviour Methods

    private void Start()
    {
        Init();
        if (videoPlayerObject != null)
            PrepareVideoPlayerSurface(videoPlayerObject);
        audioManagerObject = new AudioManager(this);

        setLogEnabled(true);
        RegisterListner();
        progressBar.Init(this);
        onUpdateContent += OnUpdateContent;
        onStartContent += OnStartContent;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            RegisterListner();
        }
        else
        {
            UnRegisterListner();
        }
    }
    #endregion

    /// <summary>
    /// Prepare example list of media items
    /// </summary>
   public void PrepareList()
    {
        RemoveAllItems();
        SetRepeatMode(0);
        currentPlayList.Add(new MediaItem("https://storage.googleapis.com/wvmedia/clear/h264/tears/tears.mpd", false, null, "Non DRM Mpd"));
        currentPlayList.Add(new MediaItem("https://html5demos.com/assets/dizzy.mp4", false, null, "sample mp4"));
        currentPlayList.Add(new MediaItem("https://tessrndmedia-inct.streaming.media.azure.net/3589acae-d913-4b95-9b60-b9bd474c2440/MultiAudioSample1.ism/manifest(format=m3u8-cmaf)", false, null, "Clear HLS: MultiAudioSample3", MediaPlayerConstants.Extentions.HLS_EXTENTION));
        currentPlayList.Add(new MediaItem("https://tessrndmedia-inct.streaming.media.azure.net/bc23b732-7dfd-40b5-a512-1e6697402e01/MultiAudioSample3.ism/manifest", false, null, "Smooth Streaming", MediaPlayerConstants.Extentions.SMOOTH_STREAMING_EXTENTION));
        currentPlayList.Add(new MediaItem("https://storage.googleapis.com/wvmedia/cenc/h264/tears/tears_sd.mpd", false, new DRMConfiguration(MediaPlayerConstants.DRMEnum.WIDE_WINE_UUID, "https://proxy.uat.widevine.com/proxy?provider=widevine_test"), "DRM mpd", MediaPlayerConstants.Extentions.DASH_EXTENTION));
        currentPlayList.Add(new MediaItem("https://tess-blob-cdn.azureedge.net/videos/songs/DHARIA-AugustDiaries.wav", false, null, "wav test"));
        currentPlayList.Add(new MediaItem("https://html5demos.com/assets/dizzy.mp4", false, null, "Clear MP4: Dizzy with external subtitle", null, new Subtitle("https://storage.googleapis.com/exoplayer-test-media-1/ttml/netflix_ttml_sample.xml", MediaPlayerConstants.MimeTypes.APPLICATION_TTML, "1", 1, "en")));
        currentPlayList.Add(new MediaItem("https://thepaciellogroup.github.io/AT-browser-tests/video/ElephantsDream.mp4", false, null, "Clear MP4: Elephant dream subtitle", null, new Subtitle("https://thepaciellogroup.github.io/AT-browser-tests/video/subtitles-en.vtt", MediaPlayerConstants.MimeTypes.TEXT_VTT, "2", 1, "en")));
        currentPlayList.Add(new MediaItem("https://cph-p2p-msl.akamaized.net/hls/live/2000341/test/master.m3u8", true, null, "HLS: Live 2", MediaPlayerConstants.Extentions.HLS_EXTENTION));
        AddItems(currentPlayList);
    }

    public void PrepareSingle()
    {
        RemoveAllItems();
        AddItem(new MediaItem("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", false, null, "sample mp4"));
    }

    public void PlayMedia()
    {
        Play(0);
    }

    public void setHasNextPrev()
    {
        NextButton.interactable = HasNext();
        PrevButton.interactable = HasPrevious();
    }


    #region Media Player Audio Controls

    /// <summary>
    /// change volume level 
    /// for max and min value case
    /// </summary>
    /// <param name="newVolume"></param>
    public void changeVolume(float newVolume)
    {
        DebugLog("New volume" + (int)newVolume);
        audioManagerObject.changeVolume((int)newVolume);
    }

    /// <summary>
    /// set mute with ui
    /// </summary>
    /// <param name="val"> toggle value / button</param>
    public void setMute()
    {
        audioManagerObject.ToggleMuteUnMute();
    }

    public void Forward()
    {
        Forward(5000);
    }


    /// <summary>
    /// Rewind by 5000 milliseconds
    /// </summary>
    public void Rewind()
    {
        Rewind(5000);
    }


    #endregion

    #region MediaPlayerEventListner


    /// <summary>
    /// on content start
    /// </summary>
    /// <param name="ContentDuration"></param>
    /// <param name="formattedContentDuration"></param>
    public void OnStartContent(long ContentDuration, string formattedContentDuration)
    {
        setHasNextPrev();
        totalDurationText.text = formattedContentDuration;
    }

    /// <summary>
    /// update timebar
    /// </summary>
    /// <param name="formattedCurrentPosition"></param>
    /// <param name="currentPosition"></param>
    /// <param name="formattedBufferedPosition"></param>
    /// <param name="bufferedPosition"></param>
    public void OnUpdateContent(string formattedCurrentPosition, long currentPosition, string formattedBufferedPosition, long bufferedPosition)
    {
        ProgressDurationText.text = formattedCurrentPosition;
        BufferDurationText.text = formattedBufferedPosition;
    }


    /// <summary>
    /// on update of subtitlet text 
    /// </summary>
    /// <param name="subtitleText"></param>
    public new void onUpdateSubtitle(string subtitleText)
    {
        SubtitleField.text = subtitleText;
    }

    #endregion

    #region
    /// <summary>
    /// on audio manager initialized
    /// </summary>
    /// <param name="minMusicStreamVolume"></param>
    /// <param name="maxMusicStreamVolume"></param>
    public void onInitialize(int minMusicStreamVolume, int maxMusicStreamVolume)
    {
        DebugLog("on audio onInitialize " + minMusicStreamVolume + " " + maxMusicStreamVolume);
        volumeSlider.minValue = minMusicStreamVolume;
        volumeSlider.maxValue = maxMusicStreamVolume;
        volumeSlider.value = audioManagerObject.GetAudioCurrentVolume();
    }


    /// <summary>
    /// on volume changes
    /// </summary>
    /// <param name="currentVolume"></param>
    public void onVolumeChange(int currentVolume)
    {
        DebugLog("on audio current volume " + currentVolume);
        volumeSlider.value = currentVolume;
    }

    /// <summary>
    /// toggle event for mute unmute state
    /// </summary>
    /// <param name="isMute"></param>

    public void onVolumeMuteUnMute(bool isMute)
    {
        if (isMute)
            muteButton.transform.GetChild(0).GetComponent<Text>().text = "Unmute";
        else
            muteButton.transform.GetChild(0).GetComponent<Text>().text = "Mute";
    }

    #endregion

    void RegisterListner()
    {
        AddMediaPlayerListner();
        audioManagerObject?.AddAudioEventListener(this);
    }
    void UnRegisterListner()
    {
        RemoveMediaPlayerListener();
        audioManagerObject?.RemoveAudioEventListener();
    }

    public void DebugLog(string logText)
    {
        if (LogTextfield != null)
            LogTextfield.text = logText;
        Debug.Log(logText);
    }
}
