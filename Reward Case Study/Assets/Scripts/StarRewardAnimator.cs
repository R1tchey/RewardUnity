using UnityEngine;
using System.Collections.Generic;
using DG.Tweening; 
using System; 

public class StarRewardAnimator : MonoBehaviour
{
    [Header("星星对象 (UI RectTransform)")]
    public List<RectTransform> starRectTransforms;

    [Header("动画参数")]
    public float popInDuration = 0.5f;
    public float delayBetweenStars = 0.2f;
    public Vector3 finalScale = Vector3.one;
    private Vector3 initialScale = Vector3.zero;
    public Ease starPopEase = Ease.OutBack;

    private Sequence _starAnimationSequence;

    void Start()
    {

        InitializeStars();
        
        if (starRectTransforms.Count > 0)
        {
            StartStarAnimation(() => Debug.Log("DOTween!"));
        }
    }


    public void InitializeStars()
    {

        if (_starAnimationSequence != null && _starAnimationSequence.IsActive())
        {
            _starAnimationSequence.Kill();
        }

        foreach (RectTransform star in starRectTransforms)
        {
            if (star != null)
            {
                star.gameObject.SetActive(true);
                star.localScale = initialScale; 
            }
        }
    }
    
    public void StartStarAnimation(Action onCompleteCallback = null)
    {
        InitializeStars(); 

        _starAnimationSequence = DOTween.Sequence();

        for (int i = 0; i < starRectTransforms.Count; i++)
        {
            RectTransform star = starRectTransforms[i];
            if (star != null)
            {
                if (i > 0)
                {
                    _starAnimationSequence.AppendInterval(delayBetweenStars);
                }
                _starAnimationSequence.Append(
                    star.DOScale(finalScale, popInDuration)
                        .SetEase(starPopEase)
                );
            }
        }


        if (onCompleteCallback != null)
        {
            _starAnimationSequence.OnComplete(() => onCompleteCallback());
        }

        _starAnimationSequence.Play();
    }


    public void PauseStarAnimation()
    {
        if (_starAnimationSequence != null && _starAnimationSequence.IsActive() && _starAnimationSequence.IsPlaying())
        {
            _starAnimationSequence.Pause();
        }
    }

    public void ResumeStarAnimation()
    {
        if (_starAnimationSequence != null && _starAnimationSequence.IsActive() && !_starAnimationSequence.IsPlaying())
        {
            _starAnimationSequence.Play();
        }
    }
    
    public void CompleteStarAnimation()
    {
        if (_starAnimationSequence != null && _starAnimationSequence.IsActive())
        {
            _starAnimationSequence.Complete(); 
        }
    }


    public void StopAndResetStarAnimation()
    {
        InitializeStars();
    }


    void OnDestroy()
    {
        if (_starAnimationSequence != null)
        {
            _starAnimationSequence.Kill();
        }
    }
}