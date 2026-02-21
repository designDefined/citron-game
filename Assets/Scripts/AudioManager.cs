using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    private AudioSource source;

    public float Volume;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volume;
    }

    public void Play()
    {
        if (source == null)
        {
            Debug.LogError($"[Sound.Play] AudioSource is null for sound: {name}.");
            return;
        }

        if (source.clip == null)
        {
            Debug.LogError($"[Sound.Play] AudioClip is null for sound: {name}.");
            return;
        }

        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetLoop()
    {
        source.loop = true;
    }

    public void SetLoopCancle()
    {
        source.loop = false;
    }

    public void SetVolume()
    {
        source.volume = Volume;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    public Sound[] sounds;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return; // 중복 인스턴스 파괴 후 즉시 종료
        }
        DontDestroyOnLoad(this.gameObject);
        Instance = this;

        for (int i = 0; i < sounds.Length; i++)
        {
            // 1. 사운드 오브젝트 생성
            GameObject soundObject = new GameObject(
                "사운드 파일 이름 :" + i + "=" + sounds[i].name
            );

            // 2. AudioSource 컴포넌트 추가 및 'newSource' 변수에 할당
            AudioSource newSource = soundObject.AddComponent<AudioSource>();

            // 🌟 3. AudioSource 속성 설정 (이 변수가 이 시점에서 유효해야 합니다.)
            newSource.spatialBlend = 0f; // 2D 사운드로 설정
            newSource.playOnAwake = false; // Play On Awake 끄기
            newSource.priority = 0; // 우선순위 최대로 설정 (선택 사항)

            // 4. Sound 객체에 설정된 AudioSource 전달 (newSource 사용)
            sounds[i].SetSource(newSource);

            // 5. AudioManager의 자식으로 설정
            soundObject.transform.SetParent(this.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Play("bgm_1");
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancle(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancle();
                return;
            }
        }
    }

    public void SetVolume(string _name, float _Volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Volume = _Volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }

    public float GetSoundDuration(string soundName)
    {
        // 1. sounds 배열을 순회하며 이름이 일치하는 Sound 객체를 찾습니다.
        for (int i = 0; i < sounds.Length; i++)
        {
            if (soundName == sounds[i].name)
            {
                // 2. 일치하는 Sound 객체를 찾았다면, 해당 객체의 clip 길이를 반환합니다.
                // Sound 객체는 이미 AudioClip clip 변수를 가지고 있습니다.
                return sounds[i].clip.length;
            }
        }

        // 3. 찾는 사운드가 없을 경우 경고 로그를 남기고 0f를 반환합니다.
        Debug.LogWarning($"[AudioManager] Sound not found: {soundName}");
        return 0f;
    }
}
