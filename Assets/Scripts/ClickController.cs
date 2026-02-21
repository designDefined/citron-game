using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickController : MonoBehaviour, IPointerClickHandler
{
    public event Action OnClick;
    bool persist = false;
    public bool playSound = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playSound)
        {
            AudioManager.Instance.Play("ui_tap_sound");
        }
        if (OnClick != null)
        {
            OnClick();
            if (!persist)
            {
                OnClick = null;
            }
        }
    }

    public void SetOnClick(Action onClick, bool persist = false)
    {
        OnClick = onClick;
        this.persist = persist;
    }
}
