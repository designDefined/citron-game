using UnityEngine;

public class WebBridge : MonoBehaviour
{
    public static WebBridge Instance { get; private set; }

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

    public void OnOffline()
    {
        ScenarioManager.Instance.OnOffline();
    }

    public void OnOnline()
    {
        ScenarioManager.Instance.OnOnline();
    }
}
