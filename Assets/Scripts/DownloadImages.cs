﻿using System.Collections.Generic;
using System.IO;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DownloadImages : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/textures/";
    private const string DB_SERVER_URL = "http://superkuma.net/DB";
    private List<Love> loves;
    private int count;
    private SpriteRenderer spriteRenderer;
    private TextMesh messageTextMesh;
    private List<string> cacheImages;
    private string tempDir = "";
    private ReactiveProperty<Love> viewLove;

    void Start() {
        count = 0;
        loves = new List<Love>();
        tempDir = Application.temporaryCachePath;
        cacheImages = new List<string>();
        spriteRenderer = GameObject.FindGameObjectWithTag("target").GetComponent<SpriteRenderer>();
        messageTextMesh = GameObject.FindGameObjectWithTag("message").GetComponent<TextMesh>();
        
        ObservableWWW.Get(DB_SERVER_URL)
            .Select(result => {
                var json = new JSONObject(result);
                for (var i = 0; i < json.Count; i++) {
                    var jsoncur = json[i];
                    var jsonmsg = jsoncur.GetField("message");
                    var jsonimg = jsoncur.GetField("file");
                    loves.Add(new Love(jsonmsg.str, jsonimg.str));
                }
                return loves;
            })
            .Subscribe(
                result => {
                    foreach (var love in loves) {
                        var url = SERVER_URL + love.ImageName;
                        var path = tempDir + "/" + love.ImageName;
                        cacheImages.Add(path);
                        if (!File.Exists(path)) {
                            ObservableWWW.GetWWW(url)
                                .Subscribe(
                                    success => {
                                        Debug.unityLogger.Log("pathpath", "pathpath");
                                        File.WriteAllBytes(path, success.bytes);
                                    },
                                    error => { Debug.Log("error1"); });
                        }
                    }
                },
                error => { Debug.Log("errrrrrrrrrrrrrr"); }
            );
    }

// Update is called once per frame
    void Update() {
    }

    public void Texturechange() {
        Debug.unityLogger.Log("count", count);
        Debug.unityLogger.Log("cacheimages", cacheImages.Count);
        Debug.unityLogger.Log("loves", loves.Count);
        if (count > cacheImages.Count - 1) {
            count = 0;
        }
        var textures = new Texture2D(0, 0);
        textures.LoadImage(Utilities.LoadbinaryBytes(cacheImages[count]));
        var x = -spriteRenderer.bounds.center.x / spriteRenderer.bounds.size.x + 0.5f;
        var y = -spriteRenderer.bounds.center.x / spriteRenderer.bounds.size.x + 0.5f;
        spriteRenderer.sprite =
            Sprite.Create(textures, new Rect(0, 0, 200, 200),
                new Vector2(x, y));
        messageTextMesh.text = loves[count].Message;
        count++;
    }
}