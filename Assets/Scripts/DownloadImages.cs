using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class DownloadImages : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/storage/textures/";
    private const string API_SERVER_URL = "http://superkuma.net/api/json/";
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
        
          ObservableWWW.Get(API_SERVER_URL)
            .SelectMany(result => {
                var json = new JSONObject(result);
                for (var i = 0; i < json.Count; i++) {
                    var jsoncur = json[i];
                    var id = jsoncur.GetField("id").ToString();
                    var username = jsoncur.GetField("name").ToString();
                    var message = jsoncur.GetField("message").ToString();
                    var mediaName = jsoncur.GetField("media_name").ToString();
                    var mediaType = jsoncur.GetField("media_type").ToString();
                    loves.Add(new Love(id,username,message, mediaName,mediaType));
                }
                return loves;
            }).Subscribe(love => {
                var url = SERVER_URL + love.MediaName;
                var path = tempDir + "/" + love.MediaName;
                cacheImages.Add(path);
                if (!File.Exists(path)) {
                    ObservableWWW.GetWWW(url)
                        .Subscribe(
                            success => {
                                Debug.unityLogger.Log("pathpath", "pathpath");
                                File.WriteAllBytes(path, success.bytes);
                            },
                            error => {Debug.Log("error1");  });
                }
            });
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
        var y = -spriteRenderer.bounds.center.y / spriteRenderer.bounds.size.y + 0.5f;
        var sprite =
            Sprite.Create(textures, new Rect(0, 0, textures.width, textures.height),
                new Vector2(x, y));
        spriteRenderer.sprite = sprite;
        var sizeX = sprite.bounds.size.x;
        var sizeY = sprite.bounds.size.y;

        
        var scaleX = 1.0f / sizeX;
        var scaleY = 1.0f / sizeY;

        var scale = scaleX > scaleY ? scaleX : scaleY;
        
        GameObject.FindGameObjectWithTag("target").transform.localScale = new Vector3(scale,scale,1.0f);
        
        messageTextMesh.text = loves[count].Message;
        count++;
    }
}