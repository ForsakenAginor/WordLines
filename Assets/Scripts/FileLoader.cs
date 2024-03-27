using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileLoader : MonoBehaviour
{
    private const string FileName = "nouns.txt";

    [SerializeField] private Root _root;

    private readonly string _path = Path.Combine(Application.streamingAssetsPath, FileName);

    private void Start()
    {
        Time.timeScale = 0;
#if UNITY_WEBGL
        StartCoroutine(LoadDictionary());
#else
         _root.Init(File.ReadAllText(_path));
#endif
    }

    private IEnumerator LoadDictionary()
    {
        string result = string.Empty;
        using UnityWebRequest loadingRequest = UnityWebRequest.Get(_path);
        yield return loadingRequest.SendWebRequest();

        if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
            Debug.LogErrorFormat(this, "Unable to load text due to {0} - {1}", loadingRequest.responseCode, loadingRequest.error);
        else
            result = System.Text.Encoding.UTF8.GetString(loadingRequest.downloadHandler.data);

        _root.Init(result);
    }
}
