#nullable enable

using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; } = null!;

    public string playerName = "";
    public string password = "";

    private List<Scenario> scenarios = new();
    private int currentScenarioIndex = 0;
    private bool toNextScenario = false;
    private int currentDialogueIndex = 0;
    private bool toNextDialogue = false;
    private Scenario? CurrentScenario =>
        currentScenarioIndex < scenarios.Count ? scenarios[currentScenarioIndex] : null;

    private readonly List<BookRecord> friendsToShow = new();
    private int friendsToShowIndex = 0;
    private bool toNextFriend = false;

    public EmotionEnum? lastEmotion = null;

    // Scenario List
    public ScenarioList initialScenarios = null!;
    public ScenarioList tutorialScenarios = null!;
    public ScenarioList readTestScenarios = null!;
    public ScenarioList mainScenarios = null!;

    // Animation Controllers
    public ImageAnimationController bunnyFrontImage = null!;
    public ImageAnimationController bunnyBackImage = null!;
    public TextAnimationController dialogueText = null!;
    public ImageAnimationController dialogueImage = null!;
    public TextAnimationController bubbleText = null!;
    public ImageAnimationController bubbleImage = null!;

    public TextAnimationController readingText = null!;

    // Animation Clips
    public AnimationClip bunnyChat = null!;
    public AnimationClip dialogueTurn = null!;

    // Click Controllers
    public ClickController dialogueClick = null!;
    public ClickController backgroundClick = null!;

    // UI Controllers
    public RegisterController registerController = null!;
    public BackgroundController backgroundController = null!;
    public BookRecordController recordBookController = null!;

    // Sprites
    public AnimationClip defaultClip = null!;
    public AnimationClip happyClip = null!;
    public AnimationClip sadClip = null!;
    public AnimationClip angryClip = null!;
    public AnimationClip tastyClip = null!;
    public AnimationClip embarrassedClip = null!;
    public AnimationClip spoilerClip = null!;

    public readonly List<string> randomReadTexts = new()
    {
        "재밌는 얘기 없어? 빨리 뭐라도 좀 읽고 와 봐.",
        "지구인들은 카페에서 책을 읽는 것도 좋아한다며? 너도 그래?",
        "내 아지트 어때? 독서하기엔 딱이지?",
        "아… 어디 재밌는 얘기 하러 온 지구인 어디 없나…",
    };

    public readonly List<string> randomReadingTexts = new()
    {
        "달에 이야기를 가져가는 중...",
        "신작 집필에 열중하는 중...",
        "달토끼 친구들과 수다 떠는 중...",
        "달나라행 고가도로 정체 중...",
        "달나라에서 북 트레이 정리 중...",
    };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        scenarios = new List<Scenario>();
        scenarios.AddRange(readTestScenarios.scenarios);
        scenarios.AddRange(initialScenarios.scenarios);
        scenarios.AddRange(tutorialScenarios.scenarios);
        scenarios.AddRange(mainScenarios.scenarios);

        ClearScreen();
        ProceedScenario();
    }

    void Update()
    {
        if (toNextScenario)
        {
            toNextScenario = false;
            currentScenarioIndex++;
            ProceedScenario();
        }

        if (toNextDialogue)
        {
            toNextDialogue = false;
            currentDialogueIndex++;
            ProceedDialogue();
        }

        if (toNextFriend)
        {
            toNextFriend = false;
            friendsToShowIndex++;
            ProceedFriend();
        }
    }

    private void ProceedScenario()
    {
        if (currentScenarioIndex < scenarios.Count)
        {
            StartScenario(scenarios[currentScenarioIndex]);
        }
    }

    private void StartScenario(Scenario scenario)
    {
        switch (scenario)
        {
            case DialogueScenario dialogueScenario:
                StartDialogues();
                break;
            case RegisterScenario registerScenario:
                StartRegister();
                break;
            case LoginScenario loginScenario:
                StartLogin();
                break;
            case SelectScenario selectScenario:
                // StartSelect(selectScenario.options);
                break;
            case ReadScenario readScenario:
                StartRead();
                break;
            case ReadingScenario readingScenario:
                StartReading();
                break;
            case RecordBookScenario recordBookScenario:
                StartRecordBook();
                break;
            case FriendsScenario friendsScenario:
                StartFriends();
                break;
            default:
                break;
        }
    }

    private void ClearScreen()
    {
        bunnyFrontImage.ClearTemporalClip();
        bunnyFrontImage.gameObject.SetActive(false);
        bunnyBackImage.ClearTemporalClip();
        bunnyBackImage.gameObject.SetActive(false);
        dialogueImage.ClearTemporalClip();
        dialogueImage.gameObject.SetActive(false);
        dialogueText.ClearTargetText();
        dialogueText.gameObject.SetActive(false);
        bubbleText.ClearTargetText();
        bubbleText.gameObject.SetActive(false);
        bubbleImage.ClearTemporalClip();
        bubbleImage.gameObject.SetActive(false);
        readingText.ClearTargetText();
        readingText.gameObject.SetActive(false);

        backgroundController.SetDefault();
    }

    private void StartDialogues()
    {
        if (CurrentScenario is not DialogueScenario)
            return;

        ClearScreen();
        bunnyFrontImage.gameObject.SetActive(true);
        dialogueImage.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        currentDialogueIndex = 0;
        ProceedDialogue();
    }

    private void ProceedDialogue()
    {
        if (CurrentScenario is not DialogueScenario dialogueScenario)
            return;

        dialogueText.ClearTargetText();

        if (currentDialogueIndex >= dialogueScenario.dialogues.Count)
        {
            currentDialogueIndex = 0;
            toNextScenario = true;
            return;
        }

        if (currentDialogueIndex > 0)
        {
            dialogueText.ClearTargetText();
            AudioManager.Instance.Play("ui_page_sound");
            dialogueImage.SetTemporalClip(
                dialogueTurn,
                () =>
                {
                    bunnyFrontImage.SetTemporalClip(bunnyChat);
                    dialogueText.SetTargetText(
                        dialogueScenario.dialogues[currentDialogueIndex].text,
                        () =>
                        {
                            bunnyFrontImage.ClearTemporalClip();
                            dialogueClick.SetOnClick(() =>
                            {
                                toNextDialogue = true;
                            });
                        }
                    );
                }
            );
        }
        else
        {
            bunnyFrontImage.SetTemporalClip(bunnyChat);
            dialogueText.SetTargetText(
                dialogueScenario.dialogues[currentDialogueIndex].text,
                () =>
                {
                    bunnyFrontImage.ClearTemporalClip();
                    dialogueClick.SetOnClick(() =>
                    {
                        toNextDialogue = true;
                    });
                }
            );
        }
    }

    private void StartRegister()
    {
        ClearScreen();
        bunnyFrontImage.gameObject.SetActive(true);
        registerController.Open(() =>
        {
            EndRegister();
        });
    }

    public void EndRegister()
    {
        toNextScenario = true;
        registerController.Close();
    }

    private void StartLogin()
    {
        ClearScreen();
        bunnyFrontImage.gameObject.SetActive(true);
    }

    public void EndLogin(string playerName)
    {
        this.playerName = playerName;
        toNextScenario = true;
    }

    private void StartRead()
    {
        if (CurrentScenario is not ReadScenario readScenario)
            return;

        ClearScreen();
        bunnyBackImage.gameObject.SetActive(true);
        bubbleImage.gameObject.SetActive(true);
        bubbleText.gameObject.SetActive(true);
        dialogueImage.gameObject.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        var randomText = "";
        if (lastEmotion.HasValue)
        {
            bunnyBackImage.SetTemporalClip(GetEmotionClip(lastEmotion.Value));
            randomText = GetEmotionText(lastEmotion.Value);
        }
        else
        {
            bunnyBackImage.SetTemporalClip(defaultClip);
            randomText = GetRandomReadText();
        }

        bubbleText.SetTargetText(randomText, () => { });
        dialogueText.SetTargetText(
            "책을 읽는 동안은 와이파이와 데이터를 전부 꺼 줘야 해. 너는 너의 세계에, 나는 나의 세계에 집중하는 거야.\n그럼 잘 부탁해, 지구인!",
            () => { }
        );
    }

    private string GetRandomReadText()
    {
        return randomReadTexts[Random.Range(0, randomReadTexts.Count)];
    }

    private string GetEmotionText(EmotionEnum emotion)
    {
        switch (emotion)
        {
            case EmotionEnum.Happy:
                return "재밌는 이야기였어, 그렇지?";
            case EmotionEnum.Sad:
                return "이럴 수는 없는 거야… 어떻게 이런 이야기가 존재하는 거지?";
            case EmotionEnum.Angry:
                return "열받음을 참을 수가 없네!! 토끼펀치!!";
            case EmotionEnum.Tasty:
                return "이 책을 달나라로 꼭 가져갈거야…";
            case EmotionEnum.Embarrassed:
                return "내가 뭘 들은 거지… 넌 뭘 읽은 거냐…?";
            case EmotionEnum.Spoiler:
                return "아, 이건 무조건 직접 읽어 봐야 하는 이야기니까… 쉿!";
            default:
                return GetRandomReadText();
        }
    }

    private AnimationClip GetEmotionClip(EmotionEnum emotion)
    {
        switch (emotion)
        {
            case EmotionEnum.Happy:
                return happyClip;
            case EmotionEnum.Sad:
                return sadClip;
            case EmotionEnum.Angry:
                return angryClip;
            case EmotionEnum.Tasty:
                return tastyClip;
            case EmotionEnum.Embarrassed:
                return embarrassedClip;
            case EmotionEnum.Spoiler:
                return spoilerClip;
            default:
                return defaultClip;
        }
    }

    private void StartReading()
    {
        if (CurrentScenario is not ReadingScenario readingScenario)
            return;

        ClearScreen();
        backgroundController.SetReading();
        readingText.gameObject.SetActive(true);
        readingText.SetTargetText(GetRandomReadingText(), () => { });
    }

    private string GetRandomReadingText()
    {
        return randomReadingTexts[Random.Range(0, randomReadingTexts.Count)];
    }

    private void StartRecordBook()
    {
        if (CurrentScenario is not RecordBookScenario recordBookScenario)
            return;

        ClearScreen();
        bunnyBackImage.gameObject.SetActive(true);
        recordBookController.Open(() =>
        {
            EndRecordBook();
        });
    }

    public void EndRecordBook()
    {
        if (CurrentScenario is not RecordBookScenario recordBookScenario)
            return;

        toNextScenario = true;
        recordBookController.Close();
    }

    private void StartFriends()
    {
        if (CurrentScenario is not FriendsScenario friendsScenario)
            return;

        friendsToShow.Clear();
        friendsToShowIndex = 0;

        ClearScreen();
        backgroundController.SetFriend();
        bubbleImage.gameObject.SetActive(true);
        bubbleText.gameObject.SetActive(true);

        GetFriends().Forget();
    }

    private async UniTaskVoid GetFriends()
    {
        var bookRecords = await ApiClient.GetAsync<BookRecordResponse>("/books");
        var filteredRecords = bookRecords
            .books.Where(record => record.password != password && record.name != playerName)
            .ToList();

        int count = Mathf.Min(filteredRecords.Count, 3);
        // Get random count records from filteredRecords
        var randomRecords = filteredRecords.OrderBy(record => Random.value).Take(count).ToList();

        friendsToShow.AddRange(randomRecords);
        ProceedFriend();
    }

    private void ProceedFriend()
    {
        if (CurrentScenario is not FriendsScenario friendsScenario)
            return;

        if (friendsToShowIndex >= friendsToShow.Count)
        {
            scenarios.Clear();
            scenarios.AddRange(mainScenarios.scenarios);
            currentScenarioIndex = 0;
            ProceedScenario();
            return;
        }

        string text =
            $"{friendsToShow[friendsToShowIndex].name}, {friendsToShow[friendsToShowIndex].bookName} 읽고 ‘{friendsToShow[friendsToShowIndex].review}’(이)라고 했대!";
        backgroundController.SetFriendEmotion(friendsToShow[friendsToShowIndex].emotion);
        bubbleText.SetTargetText(
            text,
            () =>
            {
                backgroundClick.SetOnClick(() =>
                {
                    toNextFriend = true;
                });
            }
        );
    }

    // Offline / Online
    public void OnOffline()
    {
        if (CurrentScenario is ReadScenario readScenario)
        {
            toNextScenario = true;
        }
    }

    public void OnOnline()
    {
        if (CurrentScenario is ReadingScenario readingScenario)
        {
            toNextScenario = true;
        }
    }
}
