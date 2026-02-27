#nullable enable

using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimationController : MonoBehaviour, IAnimatable
{
    int frameIndex = 0;
    public AnimationClip defaultClip = null!;
    public AnimationClip? temporalClip;
    public Image image = null!;

    private Action? onComplete;
    private bool? isPersistent;

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
                    if (isPersistent != true)
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

    public void SetTemporalClip(
        AnimationClip clip,
        Action? onComplete = null,
        bool? isPersistent = false
    )
    {
        temporalClip = clip;
        frameIndex = 0;
        this.onComplete = onComplete;
        this.isPersistent = isPersistent;
    }

    public void ClearTemporalClip()
    {
        temporalClip = null;
        frameIndex = 0;
        isPersistent = null;
    }
}
