using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

public static class ApiClient
{
    private static readonly string baseUrl = "http://localhost:8080";
    private static readonly string authToken = "";

    public static async UniTask<string> GetRawAsync(
        string path,
        Dictionary<string, string> headers = null,
        CancellationToken ct = default
    )
    {
        using var request = UnityWebRequest.Get($"{baseUrl}{path}");
        AddAuthHeader(request);
        AddCustomHeaders(request, headers);

        await request.SendWebRequest().ToUniTask(cancellationToken: ct);

        if (request.result != UnityWebRequest.Result.Success)
            throw new Exception(request.error);

        return request.downloadHandler.text;
    }

    public static async UniTask<T> GetAsync<T>(
        string path,
        Dictionary<string, string> headers = null,
        CancellationToken ct = default
    )
    {
        var jsonString = await GetRawAsync(path, headers, ct);
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static async UniTask<T> PostAsync<T>(
        string path,
        object body,
        Dictionary<string, string> headers = null,
        CancellationToken ct = default
    )
    {
        string json = JsonConvert.SerializeObject(body);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using var request = new UnityWebRequest($"{baseUrl}{path}", "POST");
        AddAuthHeader(request);
        AddCustomHeaders(request, headers);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest().ToUniTask(cancellationToken: ct);

        if (request.result != UnityWebRequest.Result.Success)
            throw new Exception(request.error);

        return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
    }

    private static void AddAuthHeader(UnityWebRequest request)
    {
        if (!string.IsNullOrEmpty(authToken))
            request.SetRequestHeader("Authorization", $"Bearer {authToken}");
    }

    private static void AddCustomHeaders(
        UnityWebRequest request,
        Dictionary<string, string> headers
    )
    {
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
        }
    }
}
