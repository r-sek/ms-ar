using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PostData : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/postData";
    private Canvas canvas;

    // Use this for initialization
    void Start() {
        canvas = GetComponent<Canvas>();
        var btn = GameObject.Find("Canvas/SubmitBtn").GetComponent<Button>();

//        var btn = canvas.GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => { StartCoroutine(SendData()); });
    }

    IEnumerator SendData() {
        var text = "";
        var path = getDirPath();
        path = GetFilePath(path);
        var bytes = GetBytesFromMedia(path);
        
        Debug.Log(bytes);
        var formdata = new WWWForm();
        if (text != "") {
            formdata.AddField("text", text);
        }
        if (bytes.Length != 0) {
            formdata.AddBinaryData("media", bytes);
        }

        using (var www = UnityWebRequest.Post(SERVER_URL, formdata)) {
            yield return www.Send();

            if (www.isNetworkError) {
                Debug.Log("send date failed");
                Debug.Log(www.error);
            }
            else {
                Debug.Log("success");
            }
        }
    }

    // Update is called once per frame
    void Update() {
    }

    private byte[] GetBytesFromMedia(string path) {

        using (var fs = new FileStream(path,FileMode.Open,FileAccess.Read)) {
            using (var bin = new BinaryReader(fs)) {
                return bin.ReadBytes((int) bin.BaseStream.Length);
            }
        }
    }

    private string getDirPath() {
#if UNITY_ANDROID
        using (var p = new AndroidJavaClass("jp.ac.hal.unityandroidplugin.FileAccessKt")) {
            return p.CallStatic<string>("getFilePath");
        }
        using (var plugin = new AndroidJavaClass("jp.ac.hal.androidplugin.FileAccessKt")) {
            return plugin.CallStatic<string>("FileAccess");
        }
#else
        return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
#endif
    }

    private string GetFilePath(string dirPath) {
        var patterns = new[] {".jpg", ".png"};
        var list = Directory.EnumerateFiles(dirPath, "*.*", SearchOption.AllDirectories)
            .Where(file => patterns.Any(pattern => file.ToLower().EndsWith(pattern))).ToList();
        return list[0];
    }


}