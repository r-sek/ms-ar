using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadImages : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/textures/";
    private const string DB_SERVER_URL = "http://superkuma.net/DB";
    private List<Love> loves;
    private int count;
    private SpriteRenderer spriteRenderer;
    private TextMesh messageTextMesh;
    private List<string> cacheImages;

    void Start() {
        count = 0;
        loves = new List<Love>();
        cacheImages = new List<string>();
        spriteRenderer = GameObject.FindGameObjectWithTag("target").GetComponent<SpriteRenderer>();
        messageTextMesh = GameObject.FindGameObjectWithTag("message").GetComponent<TextMesh>();
        Observable
            .FromCoroutine(GetMessage)
            .Subscribe(m => {
                Observable
                    .FromCoroutine(GetFile)
                    .Subscribe(f => { Debug.Log("okok"); });
            });
    }

    // Update is called once per frame
    void Update() {
    }


    IEnumerator GetMessage() {
        var result = new WWW(DB_SERVER_URL);
        yield return result;
        Debug.Log(result.text);
        if (result.error != null) yield break;
        var json = new JSONObject(result.text);
        for (var i = 0; i < json.Count; i++) {
            var jsoncur = json[i];
            var jsonmsg = jsoncur.GetField("message");
            var jsonimg = jsoncur.GetField("file");
            loves.Add(new Love(jsonmsg.str, jsonimg.str));
            Debug.Log(jsonmsg.str);
        }
    }

    IEnumerator GetFile() {
        foreach (var love in loves) {
            var url = SERVER_URL + love.ImageName;
            var path = Application.temporaryCachePath + "/" + love.ImageName;
            cacheImages.Add(path);
            if (File.Exists(path)) continue;
            using (var www = UnityWebRequest.Get(url)) {
                yield return www.Send();
                if (www.isNetworkError) {
                }
                else {
                    File.WriteAllBytes(path, www.downloadHandler.data);
                }
            }
        }
    }

    public void Texturechange() {
        if (count > cacheImages.Count - 1) {
            count = 0;
        }
        var textures = new Texture2D(0, 0);
        textures.LoadImage(Utilities.LoadbinaryBytes(cacheImages[count]));
        var x = -spriteRenderer.bounds.center.x / spriteRenderer.bounds.size.x + 0.5f;
        var y = -spriteRenderer.bounds.center.x / spriteRenderer.bounds.size.x + 0.5f;
        spriteRenderer.sprite =
            Sprite.Create(textures, new Rect(0, 0, textures.width, textures.height),
                new Vector2(x, y));
        messageTextMesh.text = loves[count].Message;
        count++;
    }
}