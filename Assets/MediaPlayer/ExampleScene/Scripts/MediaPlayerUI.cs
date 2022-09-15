//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MediaServices;
using System;
using System.IO;

/// <summary>
/// Media player UI extended with media player 
/// add Interface IMediaEventListner for media events, IAudioEventListner for audio manager events
/// </summary>
public class MediaPlayerUI : MediaPlayer, IMediaEventListner, IAudioEventListner
{
    #region Media Player object 
    private MediaPlayer mediaPlayerObject;
    private AudioManager audioManagerObject;
    private MediaRenderer mediaRendrer;
    private MediaTrackSelector mediaTrackSelector;
    private MediaLoadController mediaLoadController;
    private MediaDetails currentMediaDetails;
    private MediaItem currentMediaItem;
    #endregion

    #region Public Properties
    public Material videoPlayerObject;
    public GameObject cubeVideoPlayerObject;
    public GameObject imageVideoPlayerObject;
    public VideoProgressBar progressBar;
    public GameObject mediaItemPrefab;
    public GameObject mediaTitlePrefab;
    public List<MediaItem> currentPlayList = new List<MediaItem>();
    public List<MediaItem> AddedMediaItems = new List<MediaItem>();
    #endregion

    #region UI
    public Text LogTextfield;
    public Slider volumeSlider;
    public Button muteButton;
    public Dropdown QualityDropDown;
    public Dropdown audioDropDown;
    public Dropdown subtitleDropDown;
    public Dropdown repeatDropDown;
    public Transform playlistParent;
    public Transform currentPlaylistParent;
    public Toggle Shuffle;
    public Text totalDurationText;
    public Text ProgressDurationText;
    public Button NextButton;
    public Button PrevButton;
    public Button PlayPauseButton;
    public Button retryButton;
    //public RawImage songThumbnail;
    //public GameObject songThumbnail2;
    public Text mediaTitle;
    public Text mediaCount;
    public GameObject alertPopup;

    #endregion

    #region private variables
    private int currentQualityIndex;
    private int currentAudioIndex;
    private int currentSubtitleIndex;
    private bool isFirstTime;
    #endregion

    #region Mono Behaviour Methods

    private void Start()
    {
        currentQualityIndex = 0;
        currentAudioIndex = 0;
        currentSubtitleIndex = 0;
        retryButton.gameObject.SetActive(false);

        mediaPlayerObject = this;
        mediaRendrer = new MediaRenderer();
        mediaLoadController = new MediaLoadController();
        mediaTrackSelector = new MediaTrackSelector();
        mediaPlayerObject.Init(mediaRendrer, mediaTrackSelector, mediaLoadController);
        if (videoPlayerObject != null)
            mediaPlayerObject.PrepareVideoPlayerSurface(videoPlayerObject);
        if (cubeVideoPlayerObject != null)
            mediaPlayerObject.PrepareVideoPlayerSurface(cubeVideoPlayerObject);
        audioManagerObject = new AudioManager(mediaPlayerObject);

        mediaPlayerObject.setLogEnabled(true);
        if (audioManagerObject != null)
        {
            audioManagerObject.AppResume();
        }
        RegisterListner();
        progressBar.Init(mediaPlayerObject);
        //PrepareList();
        CreateLocalList();
        alertPopup.SetActive(false);
    }


    private void OnApplicationPause(bool focus)
    {
        if (!focus)
        {
            AppResume();
            if (audioManagerObject != null)
            {
                audioManagerObject.AppResume();
            }
            RegisterListner();
        }
        else
        {
            AppPause();
            if (audioManagerObject != null)
            {
                audioManagerObject.AppPause();
            }
            UnRegisterListner();
        }
    }
    #endregion

    /// <summary>
    /// Prepare example list of media items
    /// </summary>
    void PrepareList()
    {
        SetRepeatMode(0);
                    //currentPlayList.Add(new MediaItem("https://storage.googleapis.com/wvmedia/clear/h264/tears/tears.mpd", false, null, "Non DRM Mpd"));
                    //currentPlayList.Add(new MediaItem("https://html5demos.com/assets/dizzy.mp4", false, null, "sample mp4"));
                    //currentPlayList.Add(new MediaItem("https://tessrndmedia-inct.streaming.media.azure.net/3589acae-d913-4b95-9b60-b9bd474c2440/MultiAudioSample1.ism/manifest(format=m3u8-cmaf)", false, null, "Clear HLS: MultiAudioSample1", MediaPlayerConstants.Extentions.HLS_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://tessrndmedia-inct.streaming.media.azure.net/33421bed-4103-4827-8bd3-d72670b0b59c/MultiAudioSample2.ism/manifest(format=m3u8-cmaf)", false, null, "Clear HLS: MultiAudioSample2", MediaPlayerConstants.Extentions.HLS_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://tessrndmedia-inct.streaming.media.azure.net/f7dd2ec6-ac35-4924-91ed-6398b5c44119/MultiAudioSample4.ism/manifest", false, null, "smooth stream: MultiAudioSample3", MediaPlayerConstants.Extentions.SMOOTH_STREAMING_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://tessrndmedia-inct.streaming.media.azure.net/22408731-8df3-45f0-87b8-d60f0e04212e/MultiAudioSample5.ism/manifest(format=mpd-time-cmaf)", false, null, "Dash stream: MultiAudioSample4", MediaPlayerConstants.Extentions.DASH_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://tessrndmedia-inct.streaming.media.azure.net/bc23b732-7dfd-40b5-a512-1e6697402e01/MultiAudioSample3.ism/manifest", false, null, "Smooth Streaming", MediaPlayerConstants.Extentions.SMOOTH_STREAMING_EXTENTION));
        //currentPlayList.Add(new MediaItem("https://tesseractblobstorage.blob.core.windows.net/videos/MultiCaptionsample1/playlist.m3u8", false, null, "multi cap Streaming", MediaPlayerConstants.Extentions.HLS_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://tesseractblobstorage.blob.core.windows.net/videos/MultiCaptionSample2/playlist.m3u8", false, null, "multi cap Streaming 2", MediaPlayerConstants.Extentions.HLS_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://tesseractblobstorage.blob.core.windows.net/videos/MultiCaptionSample3/playlist.m3u8", false, null, "multi cap Streaming 3", MediaPlayerConstants.Extentions.HLS_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://bitmovin-a.akamaihd.net/content/sintel/hls/playlist.m3u8", false, null, "hls streaming 6 ", MediaPlayerConstants.Extentions.HLS_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://storage.googleapis.com/wvmedia/cenc/h264/tears/tears_sd.mpd", false, new DRMConfiguration(MediaPlayerConstants.DRMEnum.WIDE_WINE_UUID, "https://proxy.uat.widevine.com/proxy?provider=widevine_test"), "DRM mpd", MediaPlayerConstants.Extentions.DASH_EXTENTION));
                    //currentPlayList.Add(new MediaItem("https://tess-blob-cdn.azureedge.net/videos/songs/DHARIA-AugustDiaries.wav", false, null, "wav test"));
                    //currentPlayList.Add(new MediaItem("https://html5demos.com/assets/dizzy.mp4", false, null, "Clear MP4: Dizzy with external subtitle", null, new Subtitle("https://storage.googleapis.com/exoplayer-test-media-1/ttml/netflix_ttml_sample.xml", MediaPlayerConstants.MimeTypes.APPLICATION_TTML, "1", 1, "en")));
                    //currentPlayList.Add(new MediaItem("https://thepaciellogroup.github.io/AT-browser-tests/video/ElephantsDream.mp4", false, null, "Clear MP4: Elephant dream subtitle", null, new Subtitle("https://thepaciellogroup.github.io/AT-browser-tests/video/subtitles-en.vtt", MediaPlayerConstants.MimeTypes.TEXT_VTT, "2", 1, "en")));
                    //currentPlayList.Add(new MediaItem("https://cph-p2p-msl.akamaized.net/hls/live/2000341/test/master.m3u8", true, null, "HLS: Live 2", MediaPlayerConstants.Extentions.HLS_EXTENTION));
        // Audios
        //currentPlayList.Add(new MediaItem("https://tess-blob-cdn.azureedge.net/videos/songs/DHARIA-AugustDiaries.mp3", true, null, "Songs: DHARIA-AugustDiaries", ""));
        //currentPlayList.Add(new MediaItem("https://tess-blob-cdn.azureedge.net/videos/songs/Senorita.mp3", true, null, "Songs: Senorita-1", ""));

        // New samples
        //currentPlayList.Add(new MediaItem("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", false, null, "Google Sample"));
        //currentPlayList.Add(new MediaItem("https://devstreaming-cdn.apple.com/videos/streaming/examples/img_bipbop_adv_example_ts/master.m3u8", false, null, "Clear HLS-V4: BigBuckBunny with Audio Quality"));
        //currentPlayList.Add(new MediaItem("https://amssamples.streaming.mediaservices.windows.net/8b661219-cef3-4413-9471-a0b02794cc4c/BigBuckBunny.ism/manifest(format=m3u8-aapl-v3)", false, null, "Clear HLS-V3: BigBuckBunny with Audio Quality", MediaPlayerConstants.Extentions.HLS_EXTENTION));
        //currentPlayList.Add(new MediaItem("https://devstreaming-cdn.apple.com/videos/streaming/examples/bipbop_16x9/bipbop_16x9_variant.m3u8", false, null, "HLS: Apple 16x9 basic stream"));
        //currentPlayList.Add(new MediaItem("https://storage.googleapis.com/wvmedia/clear/h264/tears/tears.mpd", false, null, "Clear DASH: Tears"));
        //currentPlayList.Add(new MediaItem("https://html5demos.com/assets/dizzy.mp4", false, null, "Clear MP4: Dizzy with external subtitle", null, new Subtitle("https://storage.googleapis.com/exoplayer-test-media-1/ttml/netflix_ttml_sample.xml", MediaPlayerConstants.MimeTypes.APPLICATION_TTML, "1", 1, "en")));
                    //currentPlayList.Add(new MediaItem("https://storage.googleapis.com/shaka-demo-assets/angel-one-hls/hls.m3u8", false, null, "Clear HLS: Angel one"));
        //currentPlayList.Add(new MediaItem("", false, null, ""));
        //currentPlayList.Add(new MediaItem("", false, null, ""));
        //currentPlayList.Add(new MediaItem("", false, null, ""));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/DHARIA-AugustDiaries.mp3",
            false,
            null,
            "Mp3 Song: DHARIA-AugustDiaries",
            null,
            null,
            "https://www.c-sharpcorner.com/article/generate-thumbnail-from-live-url-of-microsoft-office-and-pdf-file/Images/image001.jpg"
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/Senorita.mp3",
            false,
            null,
            "Mp3 Song: Senorita",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/DHARIA-AugustDiaries.aac",
            false,
            null,
            "AAC Song: DHARIA-AugustDiaries",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/Senorita.aac",
            false,
            null,
            "AAC Song: Senorita",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/DHARIA-AugustDiaries.flac",
            false,
            null,
            "Flac Song: DHARIA-AugustDiaries",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/Senorita.flac",
            false,
            null,
            "Flac Song: Senorita",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/DHARIA-AugustDiaries.m4a",
            false,
            null,
            "Mp4 Song: DHARIA-AugustDiaries",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/Senorita.m4a",
            false,
            null,
            "Mp4 Song: Senorita",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tess-blob-cdn.azureedge.net/videos/songs/Senorita.m4a",
            false,
            null,
            "Mp4 Song: Senorita",
            null,
            null,
            null
            ));

        // videos
        currentPlayList.Add(new MediaItem(
            "https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
            false,
            null,
            "Google Sample",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://devstreaming-cdn.apple.com/videos/streaming/examples/img_bipbop_adv_example_ts/master.m3u8",
            false,
            null,
            "Clear HLS-V4: BigBuckBunny with Audio Quality",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://devstreaming-cdn.apple.com/videos/streaming/examples/bipbop_16x9/bipbop_16x9_variant.m3u8",
            false,
            null,
            "HLS: Apple 16x9 basic stream",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://storage.googleapis.com/wvmedia/clear/h264/tears/tears.mpd",
            false,
            null,
            "Clear DASH: Tears",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://storage.googleapis.com/shaka-demo-assets/angel-one-hls/hls.m3u8",
            false,
            null,
            "Clear HLS: Angel one",
            null,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/3589acae-d913-4b95-9b60-b9bd474c2440/MultiAudioSample1.ism/manifest(format=m3u8-cmaf)",
            false,
            null,
            "Clear HLS: MultiAudioSample1",
            MediaPlayerConstants.Extentions.HLS_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/3589acae-d913-4b95-9b60-b9bd474c2440/MultiAudioSample1.ism/manifest(format=mpd-time-cmaf)",
            false,
            null,
            "Clear DASH: MultiAudioSample1",
            MediaPlayerConstants.Extentions.DASH_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/3589acae-d913-4b95-9b60-b9bd474c2440/MultiAudioSample1.ism/manifest",
            false,
            null,
            "Clear SS: MultiAudioSample1",
            MediaPlayerConstants.Extentions.SMOOTH_STREAMING_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/33421bed-4103-4827-8bd3-d72670b0b59c/MultiAudioSample2.ism/manifest(format=m3u8-cmaf)",
            false,
            null,
            "Clear HLS: MultiAudioSample2",
            MediaPlayerConstants.Extentions.HLS_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/33421bed-4103-4827-8bd3-d72670b0b59c/MultiAudioSample2.ism/manifest(format=mpd-time-cmaf)",
            false,
            null,
            "Clear DASH: MultiAudioSample2",
            MediaPlayerConstants.Extentions.DASH_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/33421bed-4103-4827-8bd3-d72670b0b59c/MultiAudioSample2.ism/manifest",
            false,
            null,
            "Clear SS: MultiAudioSample2",
            MediaPlayerConstants.Extentions.SMOOTH_STREAMING_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/bc23b732-7dfd-40b5-a512-1e6697402e01/MultiAudioSample3.ism/manifest(format=m3u8-aapl)",
            false,
            null,
            "Clear HLS: MultiAudioSample3",
            MediaPlayerConstants.Extentions.HLS_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/f7dd2ec6-ac35-4924-91ed-6398b5c44119/MultiAudioSample4.ism/manifest(format=m3u8-aapl)",
            false,
            null,
            "Clear HLS: MultiAudioSample4",
            MediaPlayerConstants.Extentions.HLS_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/22408731-8df3-45f0-87b8-d60f0e04212e/MultiAudioSample5.ism/manifest(format=m3u8-cmaf)",
            false,
            null,
            "Clear HLS: MultiAudioSample5",
            MediaPlayerConstants.Extentions.HLS_EXTENTION,
            null,
            null
            ));

        currentPlayList.Add(new MediaItem(
            "https://tessrndmedia-inct.streaming.media.azure.net/22408731-8df3-45f0-87b8-d60f0e04212e/MultiAudioSample5.ism/manifest(format=m3u8-cmaf)",
            false,
            null,
            "Clear HLS: MultiAudioSample5",
            MediaPlayerConstants.Extentions.HLS_EXTENTION,
            null,
            null
            ));

        foreach (var item in currentPlayList)
        {
            GameObject mediaObject = Instantiate(mediaItemPrefab, playlistParent);
            mediaObject.GetComponent<MediaItemUI>().Init(item, this); //init mediaitem ui to set mediaitem and mediaplayer
        }
    }
    void CreateLocalList()
    {
        Debug.Log("===>ADDING LOCAL FILES "+Application.persistentDataPath);
		string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android", StringComparison.Ordinal));
        string myPath = path +"/SamplesVideos/";
        //string myPath = "/storage/emulated/0/SamplesVideos/";
        DirectoryInfo dir = new DirectoryInfo(myPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            Debug.Log("===> Files  " + f.FullName);
            currentPlayList.Add(new MediaItem(
            f.FullName,
            false,
            null,
            f.Name,
            null,
            null,
            ""
            ));
        }
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_1.aac",
        //    false,
        //    null,
        //    "Audio_1.aac",
        //    null,
        //    null,
        //    ""
        //    ));

        //currentPlayList.Add(new MediaItem(
        //    "storage/emulated/0/SamplesVideos/Audio_2.AIFF",
        //    false,
        //    null,
        //    "Audio_2.AIFF",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_3.flac",
        //    false,
        //    null,
        //    "Audio_3.flac",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_4.m4a",
        //    false,
        //    null,
        //    "Audio_4.m4a",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_5.MMF",
        //    false,
        //    null,
        //    "Audio_5.mmf",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_6.ogg",
        //    false,
        //    null,
        //    "Audio_6.ogg",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_7.opus",
        //    false,
        //    null,
        //    "Audio_7.opus",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_8.wav",
        //    false,
        //    null,
        //    "Audio_8.wav",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Audio_9.m4r",
        //    false,
        //    null,
        //    "Audio_9.m4r",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/SampleVideo_38Seconds.mp4",
        //    false,
        //    null,
        //    "SampleVideo.mp4",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Tere Naam- [MyMp3Bhojpuri.In].mp3",
        //    false,
        //    null,
        //    "Tere naam.mp3",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_1.3G2",
        //    false,
        //    null,
        //    "Video_1.3g2",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_2.3gp",
        //    false,
        //    null,
        //    "Video_2.3gp",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_3.avi",
        //    false,
        //    null,
        //    "Video_3.avi",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_4.flv",
        //    false,
        //    null,
        //    "Video_4.flv",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_5.mkv",
        //    false,
        //    null,
        //    "Video_5.mkv",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_6.mov",
        //    false,
        //    null,
        //    "Video_6.mov",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_7.mpg",
        //    false,
        //    null,
        //    "Video_7.mpg",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_8.webm",
        //    false,
        //    null,
        //    "Video_8.webm",
        //    null,
        //    null,
        //    ""
        //    ));
        //currentPlayList.Add(new MediaItem(
        //    "/storage/emulated/0/SamplesVideos/Video_9.wmv",
        //    false,
        //    null,
        //    "Video_9.wmv",
        //    null,
        //    null,
        //    ""
        //    ));
        foreach (var item in currentPlayList)
        {
            GameObject mediaObject = Instantiate(mediaItemPrefab, playlistParent);
            mediaObject.GetComponent<MediaItemUI>().Init(item, this); //init mediaitem ui to set mediaitem and mediaplayer
        }
        Debug.Log("===>ADDING LOCAL FILES : DONE");
    }
    public void RemoveList()
    {
        if (currentPlaylistParent.childCount > 0)
        {
            foreach (Transform i in currentPlaylistParent)
            {
                Destroy(i.gameObject);
            }
        }
    }
    public bool ExistInlist(MediaItem mediaItem) {
        return AddedMediaItems.Contains(mediaItem);
    }
    public void AddItemToList(MediaItem mediaItem)
    {
        if (!AddedMediaItems.Contains(mediaItem)) { 
        AddedMediaItems.Add(mediaItem);
        RemoveList();
        foreach (var item in AddedMediaItems)
        {
            GameObject mediaObject = Instantiate(mediaTitlePrefab, currentPlaylistParent);
            mediaObject.GetComponent<Text>().text = item._title;
        }
        }

    }
    public void RemoveItemFromList(MediaItem mediaItem)
    {
        AddedMediaItems.Remove(mediaItem);
        RemoveList();
        foreach (var item in AddedMediaItems)
        {
            GameObject mediaObject = Instantiate(mediaTitlePrefab, currentPlaylistParent);
            mediaObject.GetComponent<Text>().text = item._title;
        }
    }

    public void PlayPause()
    {
        if (mediaPlayerObject.Size() <= 0)
        {
            StartCoroutine(ShowAlertPopup("No media available. Add media and play again."));
            return;
        }

        //if (!mediaPlayerObject.IsPlaying())
        if (!isFirstTime)
        {
            // Play first media
            mediaPlayerObject.Play(0);
            isFirstTime = true;
        }
        else
        {
            if (TogglePlayPause())
            {
                PlayPauseButton.transform.GetChild(0).GetComponent<Text>().text = "Pause";
            }
            else
            {
                PlayPauseButton.transform.GetChild(0).GetComponent<Text>().text = "Play";
            }
        }
    }
    public void OnRetry()
    {
        Retry();
    }
    public void OnNext()
    {
        currentAudioIndex = 0;
        currentQualityIndex = 0;
        currentSubtitleIndex = 0;

        Next();
    }
    public void OnPrevious()
    {
        currentAudioIndex = 0;
        currentQualityIndex = 0;
        currentSubtitleIndex = 0;

        Previous();
    }
    public void setHasNextPrev()
    {
        NextButton.interactable = HasNext();
        PrevButton.interactable = HasPrevious();
    }

    /// <summary>
    /// Toggler surface to switch UI and 3d Mode
    /// </summary>
    /// <param name="val"></param>
    public void ToggleSurface(bool val)
    {
        cubeVideoPlayerObject.SetActive(val);
        imageVideoPlayerObject.SetActive(!val);
    }

    #region Media Player Controls

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
    /// <summary>
    /// On is playing State changed
    /// </summary>
    /// <param name="isPlaying"></param>
    public new void onIsPlayingChanged(bool isPlaying)
    {
        Debug.Log("--> onIsPlayingChanged: isPlaying: " + isPlaying);

        if (isPlaying)
        {
            PlayPauseButton.transform.GetChild(0).GetComponent<Text>().text = "Pause";
        }
        else
        {
            PlayPauseButton.transform.GetChild(0).GetComponent<Text>().text = "Play";
        }
        DebugLog("onIsPlayingChanged" + isPlaying);
    }

    /// <summary>
    /// forward by 5000 milliseconds
    /// </summary>
    public void Forward()
    {
        mediaPlayerObject.Forward(5000);
        progressBar.Forward(5000);
    }


    /// <summary>
    /// Rewind by 5000 milliseconds
    /// </summary>
    public void Rewind()
    {
        mediaPlayerObject.Rewind(5000);
        progressBar.Rewind(5000);
    }


    #endregion

    #region MediaPlayerEventListner
    /// <summary>
    /// On player got error
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="errorType"></param>
    /// <param name="httpErrorCode"></param>
    /// <param name="extraInfo"></param>
    public new void onPlayerError(string errorMessage, int errorType, int httpErrorCode, string extraInfo)
    {
        Debug.Log("--> onPlayerError: errorMessage: " + errorMessage);

        DebugLog("onPlayerError  " + errorMessage + " " + errorType + " " + httpErrorCode + " " + extraInfo);

        retryButton.gameObject.SetActive(true);
        StartCoroutine(ShowAlertPopup(errorMessage));
    }

    /// <summary>
    /// On player state changed while playing media
    /// </summary>
    /// <param name="playWhenReady"></param>
    /// <param name="playbackState"></param>
    public new void onPlayerStateChanged(bool playWhenReady, int playbackState)
    {
        Debug.Log("--> onPlayerStateChanged: playWhenReady: " + playWhenReady + "  playbackState: " + playbackState);
        DebugLog("onPlayerStateChanged " + playWhenReady + " " + playbackState);

        if (playbackState == 2)
            retryButton.gameObject.SetActive(false);
        else if (playbackState == 3)
            retryButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// on postion discontinuity changes
    /// </summary>
    /// <param name="reason"></param>
    public new void onPositionDiscontinuity(int reason)
    {
        DebugLog("onPositionDiscontinuity " + reason);

        Debug.Log("--> onPositionDiscontinuity: reason: " + reason);
    }

    /// <summary>
    /// On Queue position changes
    /// </summary>
    /// <param name="previousIndex"></param>
    /// <param name="newIndex"></param>
    public new void onQueuePositionChanged(int previousIndex, int newIndex)
    {
        Debug.Log("--> onQueuePositionChanged: previousIndex: " + previousIndex + "  newIndex: " + newIndex);

        setHasNextPrev();
        DebugLog("onQueuePositionChanged " + previousIndex + " " + newIndex);
    }

    /// <summary>
    /// On change of Repeate Mode
    /// </summary>
    /// <param name="repeatMode"></param>
    public new void onRepeatModeChanged(int repeatMode)
    {
        Debug.Log("--> onRepeatModeChanged: repeatMode: " + repeatMode);

        DebugLog("onRepeatModeChanged  " + repeatMode);
        setHasNextPrev();
        repeatDropDown.value = repeatMode;
    }

    /// <summary>
    /// On Shuffle mode change
    /// </summary>
    /// <param name="isShuffleEnable"></param>
    public new void onShuffleModeChanged(bool isShuffleEnable)
    {
        Debug.Log("--> onShuffleModeChanged: isShuffleEnable: " + isShuffleEnable);

        DebugLog("onShuffleModeChanged " + isShuffleEnable);
    }

    /// <summary>
    /// on content start
    /// </summary>
    /// <param name="ContentDuration"></param>
    /// <param name="formattedContentDuration"></param>
    public new void onStartContent(long ContentDuration, string formattedContentDuration)
    {
        Debug.Log("--> onStartContent: ContentDuration: " + ContentDuration + "  formattedContentDuration: " + formattedContentDuration);

        currentMediaItem = (MediaItem)mediaPlayerObject.GetAllItems()[mediaPlayerObject.GetCurrentIndex()];
        DebugLog("current media Item: ====>  " + currentMediaItem?._title);
        setHasNextPrev();
        totalDurationText.text = formattedContentDuration;

        mediaTitle.text = "Title: " + currentMediaItem?._title;
    }

    /// <summary>
    /// update timebar
    /// </summary>
    /// <param name="formattedCurrentPosition"></param>
    /// <param name="currentPosition"></param>
    /// <param name="formattedBufferedPosition"></param>
    /// <param name="bufferedPosition"></param>
    public new void onUpdateContent(string formattedCurrentPosition, long currentPosition, string formattedBufferedPosition, long bufferedPosition)
    {
        //Debug.Log("--> onUpdateContent: formattedCurrentPosition: " + formattedCurrentPosition + "  currentPosition: " + currentPosition + "  formattedBufferedPosition: " + formattedBufferedPosition + "  bufferedPosition: " + bufferedPosition);

        ProgressDurationText.text = formattedCurrentPosition;
    }


    /// <summary>
    /// on update of subtitlet text 
    /// </summary>
    /// <param name="subtitleText"></param>
    public new void onUpdateSubtitle(string subtitleText)
    {
        Debug.Log("--> onUpdateSubtitle: subtitleText: " + subtitleText);

        DebugLog("onUpdateSubtitle " + subtitleText);
    }

    /// <summary>
    /// on video details added 
    /// </summary>
    /// <param name="mediaDetails"></param>
    public new void onVideoDetails(MediaDetails mediaDetails)
    {
        Debug.Log("--> onVideoDetails");

        currentMediaDetails = mediaDetails;
        QualityDropDown?.ClearOptions();
        audioDropDown?.ClearOptions();
        subtitleDropDown?.ClearOptions();
        if (currentMediaDetails.mediaItemQuality?.Count > 0)
        {
            List<Dropdown.OptionData> qualityOptionDatas = new List<Dropdown.OptionData>();
            foreach (MediaItemQuality quality in currentMediaDetails?.mediaItemQuality)
            {
                qualityOptionDatas.Add(new Dropdown.OptionData(quality?.width + "X" + quality?.height));
            }
            QualityDropDown?.AddOptions(qualityOptionDatas);
            QualityDropDown.value = currentQualityIndex;
            QualityDropDown?.RefreshShownValue();

        }
        if (currentMediaDetails.audioTrackList?.Count > 0)
        {
            List<Dropdown.OptionData> audioOptionData = new List<Dropdown.OptionData>();
            foreach (TrackSelectorModel audio in currentMediaDetails?.audioTrackList)
            {
                audioOptionData.Add(new Dropdown.OptionData(audio?.lang));
            }
            audioDropDown?.AddOptions(audioOptionData);
            audioDropDown.value = currentAudioIndex;
            audioDropDown?.RefreshShownValue();
            mediaTrackSelector.ChangeAudioLanguage((TrackSelectorModel)currentMediaDetails.audioTrackList[currentAudioIndex]);
        }
        if (currentMediaDetails.subtitleLanguageList?.Count > 0)
        {
            List<Dropdown.OptionData> subOptionData = new List<Dropdown.OptionData>();
            foreach (TrackSelectorModel sub in currentMediaDetails?.subtitleLanguageList)
            {
                subOptionData.Add(new Dropdown.OptionData(sub?.lang));
            }
            subtitleDropDown?.AddOptions(subOptionData);
            subtitleDropDown.value = currentSubtitleIndex;
            subtitleDropDown?.RefreshShownValue();
        }
    }

    //public new void onUpdateThumbnail(byte[] byteArray)
    //{
    //    Debug.Log("--> onUpdateThumbnail: " + byteArray + "  size: " + byteArray.Length);

    //    ShowThumbnail(byteArray);
    //}

    //public new void onMediaTypeChanged(int flag)
    //{
    //    Debug.Log("--> onMediaTypeChanged: " + flag);
    //}

    public new void onCurrentSelectedMediaItem(MediaItem mediaItem)
    {
        Debug.Log("--> onCurrentSelectedMediaItem : " + mediaItem);
    }

    public new void onMediaItemQualityChange(MediaItemQuality mediaItemQuality)
    {
        Debug.Log("--> onMediaItemQualityChange: " + mediaItemQuality);
    }

    #endregion

    /// <summary>
    /// On media quality change
    /// </summary>
    /// <param name="index"></param>
    public void onMediaQualityChange(int index)
    {

        foreach (MediaItemQuality quality in currentMediaDetails.mediaItemQuality)
        {
            if ((quality.width + "X" + quality.height) == QualityDropDown.options[index].text)
            {
                mediaTrackSelector.ChangeVideoQuality((int)quality.width, (int)quality.height, (int)quality.bitRate_bps);
                currentQualityIndex = index;
            }
        }
    }


    /// <summary>
    /// audio track change by dropdown
    /// </summary>
    /// <param name="index"></param>
    public void onAudioLangChange(int index)
    {

        Debug.Log("--> onAudioLangChange: index :" + index );
        foreach (TrackSelectorModel audio in currentMediaDetails.audioTrackList)
        {
            if (audio.lang == audioDropDown.options[index].text)
            {
                mediaTrackSelector.ChangeAudioLanguage(audio);
                currentAudioIndex = index;
            }
        }
    }

    /// <summary>
    /// audio track change by dropdown
    /// </summary>
    /// <param name="index"></param>
    public void onSubtitleChange(int index)
    {
        Debug.Log("--> onSubtitleChange index: " + index);
        foreach (TrackSelectorModel sub in currentMediaDetails.subtitleLanguageList)
        {
            if ((sub.lang) == subtitleDropDown.options[index].text)
            {
                Debug.Log("--> call ChangeAudioLanguage sub: " + sub.lang);
                mediaTrackSelector.ChangeSubtitleLanguage(sub);
                currentSubtitleIndex = index;
            }
        }
    }


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
        DebugLog("check mute done " + isMute);
        if (isMute)
        {
            muteButton.transform.GetChild(0).GetComponent<Text>().text = "Unmute";
        }
        else
        {
            muteButton.transform.GetChild(0).GetComponent<Text>().text = "Mute";
        }
    }

    #endregion

    void RegisterListner()
    {
        mediaPlayerObject?.AddMediaPlayerListner(this);
        audioManagerObject?.AddAudioEventListener(this);
    }
    void UnRegisterListner()
    {
        mediaPlayerObject?.RemoveMediaPlayerListener();
        audioManagerObject?.RemoveAudioEventListener();
    }

    //private void ShowThumbnail(byte[] byteArray)
    //{
    //    Texture2D tex = new Texture2D(256, 256, TextureFormat.ETC2_RGBA8 /*PVRTC_RGBA4*/, false);
    //    tex.LoadRawTextureData(byteArray);
    //    tex.Apply();
    //    songThumbnail.texture = tex;
    //    //songThumbnail2.GetComponent<Renderer>().material.mainTexture = tex;
    //}

    public void GetMediaCount()
    {
        mediaCount.text = GetAllItems().Count.ToString();
    }

    public IEnumerator ShowAlertPopup(string msg)
    {
        alertPopup.SetActive(true);
        alertPopup.transform.Find("Message").GetComponent<Text>().text = msg;
        yield return new WaitForSeconds(2f);
        alertPopup.SetActive(false);
    }

    public void OnExitApplication()
    {
        Application.Quit();
    }

    public void DebugLog(string logText)
    {
        if (LogTextfield != null)
            LogTextfield.text = logText;
        Debug.Log(logText);
    }
}
