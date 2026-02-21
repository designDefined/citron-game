using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public Image backgroundImage;
    public Image emotionImage;
    public GameObject rope;

    public Sprite defaultSprite;
    public Sprite defaultLibrarySprite;
    public Sprite readingSprite;
    public Sprite friendSprite;
    public Sprite happySprite;
    public Sprite sadSprite;
    public Sprite angrySprite;
    public Sprite tastySprite;
    public Sprite embarrassedSprite;
    public Sprite spoilerSprite;

    public void SetDefault()
    {
        Debug.Log("SetDefault");
        backgroundImage.sprite = defaultSprite;
        rope.SetActive(false);
    }

    public void SetDefaultLibrary()
    {
        backgroundImage.sprite = defaultLibrarySprite;
        rope.SetActive(false);
    }

    public void SetReading()
    {
        backgroundImage.sprite = readingSprite;
        rope.SetActive(true);
    }

    public void SetFriend()
    {
        backgroundImage.sprite = friendSprite;
        rope.SetActive(false);
    }

    public void SetFriendEmotion(EmotionEnum emotion)
    {
        switch (emotion)
        {
            case EmotionEnum.Happy:
                emotionImage.sprite = happySprite;
                break;
            case EmotionEnum.Sad:
                emotionImage.sprite = sadSprite;
                break;
            case EmotionEnum.Angry:
                emotionImage.sprite = angrySprite;
                break;
            case EmotionEnum.Tasty:
                emotionImage.sprite = tastySprite;
                break;
            case EmotionEnum.Embarrassed:
                emotionImage.sprite = embarrassedSprite;
                break;
            case EmotionEnum.Spoiler:
                emotionImage.sprite = spoilerSprite;
                break;
            default:
                emotionImage.sprite = happySprite;
                break;
        }
    }
}
