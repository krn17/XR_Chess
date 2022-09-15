//=====================================================================
// Copyright Tesseract Imaging Limited 2020. All Rights Reserved.
// Node module: Media Player
// Author: Sagar Ahirrao
//=====================================================================


using UnityEngine;
using UnityEngine.UI;
using MediaServices;
using System.Collections;

/// <summary>
/// media item ui for example playlist
/// </summary>
public class MediaItemUI : MonoBehaviour
{
    public MediaItem mediaItem;
    public Text itemTitle;
    public MediaPlayerUI mediaPlayer;
    public Toggle Addremove;
    public Transform List;

    /// <summary>
    /// set player and media item
    /// </summary>
    /// <param name="_mediaItem"></param>
    /// <param name="_mediaPlayer"></param>
    public void Init(MediaItem _mediaItem, MediaPlayerUI _mediaPlayer)
    {
        mediaItem = _mediaItem;
        mediaPlayer = _mediaPlayer;
        itemTitle.text = "Title: " + _mediaItem._title + "\n" + "Extention: " + _mediaItem._Extention;
        Addremove.isOn = false;
        Addremove.transform.GetChild(0).gameObject.SetActive(true);
        Addremove.transform.GetChild(1).gameObject.SetActive(false);
    }

    /// <summary>
    /// ui toggle to add remove item from playlist
    /// </summary>
    /// <param name="Add"></param>
    public void PlaylistItemToggle(bool Add)
    {
        Addremove.transform.GetChild(1).gameObject.SetActive(Add);
        Addremove.transform.GetChild(0).gameObject.SetActive(!Add);
        if (Add)
            AddItemToPlaylist();
        else
            RemoveItemFromPlaylist();
    }

    /// <summary>
    /// Add item to play list 
    /// </summary>
    public void AddItemToPlaylist()
    {
        if (!mediaPlayer.IsExistInCurrentList(mediaItem)) {
            mediaPlayer.AddItem(mediaItem);
        }
        if(!mediaPlayer.ExistInlist(mediaItem))
            mediaPlayer.AddItemToList(mediaItem);
        
    }

    /// <summary>
    /// play item to playlist
    /// </summary>
    public void PlayItemToPlaylist()
    {
        StartCoroutine(executePlay());
    }
    
    /// <summary>
    /// execute corotine to add delay in add&play
    /// </summary>
    /// <returns></returns>
    IEnumerator executePlay() {
        if (!mediaPlayer.IsExistInCurrentList(mediaItem))
        {
            Addremove.isOn = true;
            yield return new WaitForSeconds(0.1f);
        }
        mediaPlayer.Play(mediaItem);
    }

    //remove item from playlist
    public void RemoveItemFromPlaylist()
    {
        //if (MediaItem.IsExistInList(mediaPlayer.GetAllItems(), mediaItem))
        //{
            mediaPlayer.RemoveItem(mediaItem);
        //}
        mediaPlayer.RemoveItemFromList(mediaItem);
    }
}
