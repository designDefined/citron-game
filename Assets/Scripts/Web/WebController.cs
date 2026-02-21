using System.Runtime.InteropServices;
using UnityEngine;

public static class WebController
{
#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    public static extern void ShowAlert(string message);

#else
    public static void ShowAlert(string message)
    {
        Debug.Log($"ShowAlert Called: {message}");
    }

#endif
}
