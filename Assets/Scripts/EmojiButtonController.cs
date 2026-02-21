using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmojiButtonController : MonoBehaviour, IPointerClickHandler
{
    Image image;
    public EmotionEnum emotion;
    public event Action<EmotionEnum> OnClick;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
        {
            OnClick(emotion);
        }
    }

    public void SetOnClick(Action<EmotionEnum> emotion)
    {
        OnClick = emotion;
    }

    public void SetSelected(bool selected)
    {
        image.color = selected ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
    }
}
