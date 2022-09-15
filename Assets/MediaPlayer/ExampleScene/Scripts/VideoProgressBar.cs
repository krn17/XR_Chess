using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using MediaServices;

public class VideoProgressBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static MediaPlayer mpManager;
    public Image progressBar;
    public Image bufferBar;
    private long _totalDuration;
    private long _currentPosition;
    private long _buffredPosition;
    private string _currentPostionText;
    public Text subtitle;
    public Text Progress;

    private void Awake()
    {
        if (progressBar == null)
            progressBar = GetComponent<Image>();
    }
    
    public void Init(MediaPlayer mediaplayer)
    {

        mpManager = mediaplayer;
        mpManager.onUpdateContent += onContentUpdate;
        mpManager.onStartContent += onStartContent;
        mpManager.onUpdateSubtitle += onSubtitleUpdate;
    }

    private void onSubtitleUpdate(string obj)
    {
        subtitle.text = obj;
    }

    private void onStartContent(long totalDuration, string arg2)
    {
        _totalDuration = totalDuration;
    }

    private void onContentUpdate(string currentPostionText, long currentPosition, string bufferPostionText, long bufferedPosition)
    {
        _currentPostionText = currentPostionText;
        _currentPosition = currentPosition;
        _buffredPosition = bufferedPosition;
        bufferBar.fillAmount = (float)((float)bufferedPosition / (float)_totalDuration);
        progressBar.fillAmount = (float)((float)currentPosition / (float)_totalDuration);
    }

    public void Forward(int value)
    {
        if (mpManager.Size() > 0)
        {
            _currentPosition = _currentPosition + value;
            if (_currentPosition > _totalDuration)
            {
                _currentPosition = _totalDuration;
            }
            progressBar.fillAmount = (float)((float)_currentPosition / (float)_totalDuration);
            Progress.text = Utility.GetFormattedDuration(_currentPosition);
        }
    }

    public void Rewind(int value)
    {
        if (mpManager.Size() > 0)
        {
            _currentPosition = _currentPosition - value;
            if (_currentPosition < 0)
            {
                _currentPosition = 0;
            }
            progressBar.fillAmount = (float)((float)_currentPosition / (float)_totalDuration);
            Progress.text = Utility.GetFormattedDuration(_currentPosition);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        TrySkip(eventData);

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        TrySkip(eventData);
    }
    private void TrySkip(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            progressBar.rectTransform, eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(progressBar.rectTransform.rect.xMin, progressBar.rectTransform.rect.xMax, localPoint.x);
            progressBar.fillAmount = pct;
            var frame = _totalDuration * pct;
            TimeSpan Seekt = TimeSpan.FromMilliseconds(frame);
            Progress.text = Seekt.Hours + ":" +Seekt.Minutes + ":"+ Seekt.Seconds + ":" + Seekt.Milliseconds;
            try
            {
                mpManager.SeekBarPosition((long)frame);
            }
            catch {
                Debug.Log("seekbar error");
            }
        }
    }

    private void StartScrub(float pct)
    {
        var frame = _totalDuration * pct;
        mpManager.SeekBarPosition((long)frame);
    }
    private void MoveScrub(float pct)
    {
        var frame = _totalDuration * pct;
        mpManager.SeekBarPosition((long)frame);
    }
    private void StopScrub(float pct)
    {
        var frame = _totalDuration * pct;
        mpManager.SeekBarPosition((long)frame);
    }


}