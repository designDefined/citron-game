using System;
using TMPro;
using UnityEngine;

public class TextAnimationController : MonoBehaviour, IAnimatable
{
    int textIndex = 0;
    public string targetText = "";
    public TMP_Text text;

    private Action onComplete;

    void OnEnable()
    {
        AnimationManager.Instance.RegisterText(this);
    }

    void OnDisable()
    {
        AnimationManager.Instance.UnregisterText(this);
    }

    public void OnNextFrame()
    {
        if (textIndex <= targetText.Length)
        {
            text.text = targetText[..textIndex];
            textIndex++;
        }
        else
        {
            if (onComplete != null)
            {
                onComplete();
                onComplete = null;
            }
        }
    }

    public void SetTargetText(string text, Action onComplete)
    {
        targetText = text;
        textIndex = 0;
        this.onComplete = onComplete;
    }

    public void ClearTargetText()
    {
        targetText = "";
        textIndex = 0;
        onComplete = null;
    }
}
