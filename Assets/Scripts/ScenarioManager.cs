using System;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; }

    // Animation Controllers
    public ImageAnimationController bunnyImage;
    public TextAnimationController dialogueText;
    public ImageAnimationController dialogueImage;

    // Animation Clips
    public AnimationClip bunnyChat;
    public AnimationClip dialogueTurn;

    // Click Controllers
    public ClickController dialogueClick;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        StartDialogue(
            "안녕? 나는 온라인 토끼.\n오프라인이 궁금해서 널 찾아왔어.",
            () =>
            {
                dialogueClick.SetOnClick(() =>
                {
                    ProceedDialogue("너도 나처럼 책을 좋아한다며?\n네 요즘 관심사는 뭐야?");
                });
            }
        );
    }

    private void StartDialogue(string text, Action onComplete = null)
    {
        bunnyImage.SetTemporalClip(bunnyChat);
        dialogueText.SetTargetText(
            text,
            () =>
            {
                bunnyImage.ClearTemporalClip();
                onComplete?.Invoke();
            }
        );
    }

    private void ProceedDialogue(string text, Action onComplete = null)
    {
        dialogueText.ClearTargetText();
        dialogueImage.SetTemporalClip(
            dialogueTurn,
            () =>
            {
                dialogueText.SetTargetText(
                    text,
                    () =>
                    {
                        dialogueImage.ClearTemporalClip();
                        onComplete?.Invoke();
                    }
                );
            }
        );
    }
}
