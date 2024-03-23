using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileLoader
{
    private const string FileName = "nouns.txt";

    public FileLoader()
    {
        string path = Path.Combine(Application.streamingAssetsPath, FileName);

#if UNITY_WEBGL

        var loadingRequest = UnityWebRequest.Get(path);
        loadingRequest.SendWebRequest();

        while (loadingRequest.isDone == false && loadingRequest.result == UnityWebRequest.Result.ConnectionError && loadingRequest.result == UnityWebRequest.Result.ProtocolError) ;

        Dictionary = loadingRequest.downloadHandler.text;

#else

        Dictionary = File.ReadAllText(path);

#endif
    }

    public string Dictionary { get; }
}
