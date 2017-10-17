using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PostData : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/postData";

    private List<string> fileList;
    private string text = "";
    
    // Use this for initialization
    void Start() {

        Screen.orientation = ScreenOrientation.Portrait;
        
        var btn = GameObject.Find("Canvas/SubmitBtn").GetComponent<Button>();
        var inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();

        var returnBtn = GameObject.Find("Canvas/ReturnBtn").GetComponent<Button>();
        var image = GameObject.Find("Canvas/Image").GetComponent<Image>();
        var path = getDirPath();
        fileList = GetFilePathList(path);
        
        btn.onClick.AddListener(() => {
            StartCoroutine(SendData());
        });
        
        inputField.onEndEdit.AddListener(s => {
            text = s;
        });
        inputField.onValueChanged.AddListener(s => {
            inputField.text = s;
        });
        returnBtn.onClick.AddListener(() => {
            // メイン画面に戻る
        });
        
        
    }


    // Update is called once per frame
    void Update() {
    }

    private byte[] GetBytesFromMedia(string path) {
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
            using (var bin = new BinaryReader(fs)) {
                return bin.ReadBytes((int) bin.BaseStream.Length);
            }
        }
    }

    IEnumerator SendData() {
        // todo: 暫定 1番目取得
        var filepath = fileList[0];

        var bytes = GetBytesFromMedia(filepath);

        Debug.Log(text);
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

    private List<string> GetFilePathList(string dirPath) {
        var patterns = new[] {".jpg", ".png"};
        var list = Directory.EnumerateFiles(dirPath, "*.*", SearchOption.AllDirectories)
            .Where(file => patterns.Any(pattern => file.ToLower().EndsWith(pattern))).ToList();
        return list;
    }
}