using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour
{
    public GameObject root;
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;

    public Button registerButton;

    public event Action OnRegisterSuccess;

    void Awake()
    {
        root.SetActive(false);
    }

    void OnEnable()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClick);
    }

    void OnDisable()
    {
        registerButton.onClick.RemoveListener(OnRegisterButtonClick);
    }

    public void Open(Action onRegisterSuccess)
    {
        nameInput.text = "";
        passwordInput.text = "";
        root.SetActive(true);
        OnRegisterSuccess = onRegisterSuccess;
    }

    public void Close()
    {
        nameInput.text = "";
        passwordInput.text = "";
        root.SetActive(false);
    }

    public void OnRegisterButtonClick()
    {
        Register().Forget();
    }

    public async UniTaskVoid Register()
    {
        string name = nameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
        {
            return;
        }

        await ApiClient.PostAsync<object>("/users", new { name, password });
        ScenarioManager.Instance.playerName = name;
        ScenarioManager.Instance.password = password;
        OnRegisterSuccess?.Invoke();
    }
}
