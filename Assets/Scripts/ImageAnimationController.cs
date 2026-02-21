using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimationController : MonoBehaviour, IAnimatable
{
    int frameIndex = 0;
    public AnimationClip defaultClip;
    public AnimationClip temporalClip;
    public Image image;

    private Action onComplete;

    void OnEnable()
    {
        AnimationManager.Instance.Register(this);
    }

    void OnDisable()
    {
        AnimationManager.Instance.Unregister(this);
    }

    public void OnNextFrame()
    {
        if (temporalClip != null)
        {
            if (frameIndex >= temporalClip.frames.Length)
            {
                if (temporalClip.loop)
                {
                    frameIndex = 0;
                }
                else
                {
                    ClearTemporalClip();
                    onComplete?.Invoke();
                    onComplete = null;
                }
            }
        }
        else if (defaultClip != null)
        {
            if (frameIndex >= defaultClip.frames.Length)
            {
                if (defaultClip.loop)
                    frameIndex = 0;
                else
                    frameIndex = defaultClip.frames.Length - 1;
            }
        }

        if (temporalClip != null)
            image.sprite = temporalClip.frames[frameIndex];
        else if (defaultClip != null)
            image.sprite = defaultClip.frames[frameIndex];
        frameIndex++;
    }

    public void SetTemporalClip(AnimationClip clip, Action onComplete = null)
    {
        temporalClip = clip;
        frameIndex = 0;
        this.onComplete = onComplete;
    }

    public void ClearTemporalClip()
    {
        temporalClip = null;
        frameIndex = 0;
    }
}
