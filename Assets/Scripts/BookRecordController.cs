using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookRecordController : MonoBehaviour
{
    public GameObject root;
    public TMP_InputField booktNameInput;
    public TMP_InputField reviewInput;

    public EmotionEnum? emotion = null;
    public EmojiButtonController happyClick;
    public EmojiButtonController sadClick;
    public EmojiButtonController angryClick;
    public EmojiButtonController tastyClick;
    public EmojiButtonController embarrassedClick;
    public EmojiButtonController spoilerClick;

    public Button ongoingButton;
    public Button doneButton;

    public event Action OnRecordSuccess;

    void Awake()
    {
        root.SetActive(false);
    }

    void OnEnable()
    {
        ongoingButton.onClick.AddListener(OnOngoingButtonClick);
        doneButton.onClick.AddListener(OnDoneButtonClick);
    }

    void OnDisable()
    {
        ongoingButton.onClick.RemoveListener(OnOngoingButtonClick);
        doneButton.onClick.RemoveListener(OnDoneButtonClick);
    }

    public void Open(Action onRecordSuccess)
    {
        booktNameInput.text = "";
        reviewInput.text = "";
        emotion = null;
        root.SetActive(true);
        OnRecordSuccess = onRecordSuccess;

        happyClick.SetOnClick(OnSelectEmoji);
        sadClick.SetOnClick(OnSelectEmoji);
        angryClick.SetOnClick(OnSelectEmoji);
        tastyClick.SetOnClick(OnSelectEmoji);
        embarrassedClick.SetOnClick(OnSelectEmoji);
        spoilerClick.SetOnClick(OnSelectEmoji);
    }

    public void Close()
    {
        booktNameInput.text = "";
        reviewInput.text = "";
        emotion = null;
        root.SetActive(false);

        happyClick.SetOnClick(null);
        sadClick.SetOnClick(null);
        angryClick.SetOnClick(null);
        tastyClick.SetOnClick(null);
        embarrassedClick.SetOnClick(null);
        spoilerClick.SetOnClick(null);
    }

    public void OnSelectEmoji(EmotionEnum emotion)
    {
        happyClick.SetSelected(emotion == EmotionEnum.Happy);
        sadClick.SetSelected(emotion == EmotionEnum.Sad);
        angryClick.SetSelected(emotion == EmotionEnum.Angry);
        tastyClick.SetSelected(emotion == EmotionEnum.Tasty);
        embarrassedClick.SetSelected(emotion == EmotionEnum.Embarrassed);
        spoilerClick.SetSelected(emotion == EmotionEnum.Spoiler);

        this.emotion = emotion;
    }

    public void OnOngoingButtonClick()
    {
        Record(true).Forget();
    }

    public void OnDoneButtonClick()
    {
        Record(false).Forget();
    }

    public async UniTaskVoid Record(bool isOngoing)
    {
        string bookName = booktNameInput.text;
        string review = reviewInput.text;

        if (string.IsNullOrEmpty(bookName) || string.IsNullOrEmpty(review) || emotion == null)
        {
            return;
        }

        await ApiClient.PostAsync<object>(
            "/books",
            new
            {
                name = ScenarioManager.Instance.playerName,
                ScenarioManager.Instance.password,
                bookName,
                review,
                emotion,
                isOngoing,
            }
        );

        ScenarioManager.Instance.lastEmotion = emotion;
        OnRecordSuccess?.Invoke();
    }
}
