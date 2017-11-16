using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostData : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/postData";
    private List<Sprite> sprites;
    private List<string> fileList;
    private string text = "";
    private RectTransform content;
    private int index = 0;
    private const string FILE_HEADER = "file://";
    private string filepath = "";
    private Texture2D postTexture;

    void Start() {
        sprites = new List<Sprite>();
           postTexture = new Texture2D(0,0);
        // 縦固定
        Screen.orientation = ScreenOrientation.Portrait;

        var btn = GameObject.Find("Canvas/SubmitBtn").GetComponent<Button>();
        var inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();

        var returnBtn = GameObject.Find("Canvas/ReturnBtn").GetComponent<Button>();
        var path = GetDirPath();
        fileList = GetFilePathList(path);

        btn.onClick.AddListener(() => { StartCoroutine(SendData()); });

        inputField.onEndEdit.AddListener(s => { text = s; });
        inputField.onValueChanged.AddListener(s => { inputField.text = s; });
        returnBtn.onClick.AddListener(() => {
            // メイン画面に戻る
            SceneManager.LoadScene("MainView");
        });

        content = GameObject.Find("Canvas/Scroll View/Viewport/Content").GetComponent<RectTransform>();
        content.sizeDelta = new Vector2(200 * fileList.Count, 0);
        var srect = GameObject.Find("Canvas/Scroll View").GetComponent<ScrollRect>();
        srect.normalizedPosition = new Vector2(0, 0);
        foreach (var s in fileList) {
            var item = Instantiate(Resources.Load("Prefab/AA")) as GameObject;
            item.GetComponent<ImageObject>().Filepath = s;
            item.transform.SetParent(content, false);
        }

        StartCoroutine(LoadTexture2D());
        foreach (var str in fileList) {
            Debug.Log(str);
        }
    }

    // Update is called once per frame
    void Update() {
    }

    private IEnumerator LoadTexture2D() {
        foreach (var path in fileList) {
            var tex = new byte[0];

            if (!File.Exists(path)) {
                yield break;
            }
#if UNITY_ANDROID
            using (var p = new AndroidJavaClass("jp.ac.hal.unityandroidplugin.FileAccessKt")) {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                        using (var context = activity.Call<AndroidJavaObject>("getApplicationContext")) {
                            tex = p.CallStatic<byte[]>("getThumbnails", context, path);
                        }
                    }
                }
            }
#else
            using (var www = new WWW(FILE_HEADER + path)) {
                while (!www.isDone) {
                    yield return new WaitForEndOfFrame();
                }
                 tex = www.bytes;
            }
#endif
            var t2D = new Texture2D(0, 0);
            t2D.LoadImage(tex);
            t2D.Apply(true, true);
            var sprite = Utilities.GetSpriteFromTexture2D(t2D);
            sprites.Add(sprite);
        }
        SetImage();
    }

    private IEnumerator SendData() {
        // todo: 暫定 1番目取得

        var bytes = Utilities.LoadbinaryBytes(filepath);
        var formdata = new WWWForm();

        formdata.AddField("name", "gest");
        if (text != "") {
            formdata.AddField("message", text);
        }
        if (bytes.Length != 0) {
            formdata.AddBinaryData("uploadfile", bytes);
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

    private void SetImage() {
        foreach (var btn in content.GetComponentsInChildren<Button>()) {
            btn.GetComponent<Image>().sprite = sprites[0];
            btn.onClick.AddListener(() => {
                filepath = btn.GetComponent<ImageObject>().Filepath;

                postTexture.LoadImage(Utilities.LoadbinaryBytes(filepath));
                postTexture.Apply(true, true);
                var img = GameObject.Find("Canvas/Image").GetComponent<Image>();
                Debug.unityLogger.Log("img", img);
                img.sprite = Utilities.GetSpriteFromTexture2D(postTexture);
                img.color = Color.white;
                
            });
            sprites.RemoveAt(0);
            if (sprites.Count == 0) {
                break;
            }
        }
    }

    private string GetDirPath() {
#if UNITY_ANDROID
        using (var p = new AndroidJavaClass("jp.ac.hal.unityandroidplugin.FileAccessKt")) {
            return p.CallStatic<string>("getFilePath");
        }
#else
        return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
#endif
    }

    private List<string> GetFilePathList(string dirPath) {
        var patterns = new[] {".jpg", ".png"};
        var list = Directory.EnumerateFiles(dirPath, "*.*", SearchOption.AllDirectories)
            .Where(file => patterns.Any(pattern => file.ToLower().EndsWith(pattern)) &&
                           !(file.IndexOf("/.thumbnails/", StringComparison.Ordinal) > 0))
            .OrderByDescending(File.GetLastWriteTime)
            .Skip(index).Take(20).ToList();
        return list;
    }
}
