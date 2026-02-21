using System;
using TMPro;
using UnityEngine;

public class TextAnimationController : MonoBehaviour, IAnimatable
{
    int textIndex = 0;
    public string targetText = "";
    public TMP_Text text;
    public bool hasVoice = false;
    public int voiceInterval = 10;
    private int voiceIntervalIndex = 10;

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
            voiceIntervalIndex++;
            if (hasVoice && voiceIntervalIndex >= voiceInterval)
            {
                AudioManager.Instance.Play("bunny_voice");
                voiceIntervalIndex = 0;
            }
        }
        else
        {
            AudioManager.Instance.Stop("bunny_voice");
            if (onComplete != null)
            {
                onComplete();
                onComplete = null;
            }
        }
    }

    public void SetTargetText(string text, Action onComplete)
    {
        targetText = ReplaceText(text);
        textIndex = 0;
        voiceIntervalIndex = voiceInterval;
        this.onComplete = onComplete;
    }

    public void ClearTargetText()
    {
        targetText = "";
        textIndex = 0;
        voiceIntervalIndex = 0;
        onComplete = null;
    }

    private string ReplaceText(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return text.Replace("{{playerName}}", ScenarioManager.Instance.playerName);
    }
}
