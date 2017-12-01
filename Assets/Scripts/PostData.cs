
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PostData : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/api/json";

    private List<string> fileList;
    private string text = "";
    private RectTransform content;
    private int index = 0;
    private const string FILE_HEADER = "file://";
    private string filepath = "";
    private Texture2D postTexture;
    private byte[] postImageBytes;
    private Image img;
    private string fileName = "";

    void Start() {
        postTexture = new Texture2D(0, 0);
        img = GameObject.Find("Canvas/Image").GetComponent<Image>();

        // 縦固定
        Screen.orientation = ScreenOrientation.Portrait;

        var submitBtn = GameObject.Find("Canvas/SubmitBtn").GetComponent<Button>();
        var inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();

        var returnBtn = GameObject.Find("Canvas/ReturnBtn").GetComponent<Button>();
        var dirPath = GetDirPath();
        fileList = GetFilePathList(dirPath);

        var progress = new ScheduledNotifier<float>();
        //progress.Subscribe(prog => Debug.Log(prog));

        submitBtn.OnClickAsObservable()
            .Subscribe(_ => {
                var formdata = new WWWForm();

                formdata.AddField("name", "gest");
                if (text != "") {
                    formdata.AddField("message", text);
                }
                if (postImageBytes.Length != 0) {
                    formdata.AddBinaryData("upload_file", postImageBytes, fileName);
                }
                ObservableWWW.Post(SERVER_URL, formdata, progress)
                    .Subscribe(
                        result => {
                            img.color = Color.clear;
                            postImageBytes = new byte[0];
                            inputField.text = "";
                            Debug.unityLogger.Log("result",result);

                            Debug.Log("success");
                        }, 
                        error => {
                            GameObject.Find("Dialog").GetComponent<Dialog>().ViewDialog();
                            Debug.Log("ng");
                        }
                    );
            });

        returnBtn.OnClickAsObservable()
            .Subscribe(
                _ => SceneManager.LoadScene("MainView")
            );

        inputField.OnEndEditAsObservable()
            .Subscribe(
                s => { text = s; }
            );

        content = GameObject.Find("Canvas/Scroll View/Viewport/Content").GetComponent<RectTransform>();
        content.sizeDelta = new Vector2(200 * fileList.Count, 0);
        var srect = GameObject.Find("Canvas/Scroll View").GetComponent<ScrollRect>();
        srect.normalizedPosition = new Vector2(0, 0);

        foreach (var s in fileList) {
            var item = Instantiate(Resources.Load("Prefab/AA")) as GameObject;
            if (item == null) continue;
            item.GetComponent<ImageObject>().Filepath = s;
            item.GetComponent<Image>().sprite =
                Utilities.GetSpriteFromTexture2D(Utilities.GetTexture2DFromBytes(Utilities.GetImageByte(s)));
            item.transform.SetParent(content, false);
            var btn = item.GetComponent<Button>();
            btn.OnClickAsObservable()
                .Subscribe(_ => {
                    filepath = btn.GetComponent<ImageObject>().Filepath;
                    var n = new FileInfo(filepath);
                    fileName = n.Name;
                    Debug.unityLogger.Log("fileName", fileName);
#if UNITY_ANDROID
                    postImageBytes = AndroidImageRotate(filepath);
                    postTexture.LoadImage(postImageBytes);
#else
                    postTexture.LoadImage(Utilities.LoadbinaryBytes(filepath));
#endif
                    Debug.unityLogger.Log("img", img);
                    img.sprite = Utilities.GetSpriteFromTexture2D(postTexture);
                    img.color = Color.white;
                });
        }
    }

    // Update is called once per frame
    void Update() {
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

#if UNITY_ANDROID
    private byte[] AndroidImageRotate(string path) {
        using (var plugin = new AndroidJavaClass("jp.ac.hal.unityandroidplugin.FileAccessKt")) {
            return plugin.CallStatic<byte[]>("imageRotate", path);
        }
    }
#endif
}