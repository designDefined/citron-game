using UnityEngine;

[CreateAssetMenu(fileName = "AnimationClip", menuName = "AnimationClip")]
public class AnimationClip : ScriptableObject
{
    public Sprite[] frames;
    public bool loop = true;
}
