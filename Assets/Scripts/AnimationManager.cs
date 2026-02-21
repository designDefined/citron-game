using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    readonly List<IAnimatable> animatables = new();
    readonly List<IAnimatable> animatableTexts = new();
    float timer = 0;
    public float frameRate = 8f;

    float textTimer = 0;
    public float textFrameRate = 16f;

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

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            foreach (var animatable in animatables)
                animatable.OnNextFrame();
        }

        textTimer += Time.deltaTime;
        if (textTimer >= 1f / textFrameRate)
        {
            textTimer = 0f;
            foreach (var animatableText in animatableTexts)
                animatableText.OnNextFrame();
        }
    }

    public void Register(IAnimatable animatable)
    {
        animatables.Add(animatable);
    }

    public void Unregister(IAnimatable animatable)
    {
        animatables.Remove(animatable);
    }

    public void RegisterText(IAnimatable animatable)
    {
        animatableTexts.Add(animatable);
    }

    public void UnregisterText(IAnimatable animatable)
    {
        animatableTexts.Remove(animatable);
    }
}
