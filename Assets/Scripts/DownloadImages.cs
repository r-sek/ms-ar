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
            .Select(text => new JSONObject(text))
            .SelectMany(jsonList => jsonList.list)
            .Select(json => {
                var id = json.GetField("id").str;
                var username = json.GetField("name").str;
                var message = json.GetField("message").str;
                var mediaName = json.GetField("media_name").str;
                var mediaType = json.GetField("media_type").str;
                var love = new Love(id,username,message,mediaName,mediaType);
                loves.Add(love);
                Debug.unityLogger.Log("love", mediaName + "::" + loves[loves.Count - 1].MediaName);
                Debug.unityLogger.Log("love", mediaType + "::" + loves[loves.Count - 1].MediaType);
                Debug.unityLogger.Log("love", message + "::" + loves[loves.Count - 1].Message);
                return love;
            })
            .Where(love=>love.MediaType !=Love.MediaTypeEnum.NONE)
            .Subscribe(love => {
                Debug.unityLogger.Log("url",love.MediaName);
                var url = SERVER_URL + love.MediaName;
                var path = tempDir + "/" + love.MediaName;
                cacheImages.Add(path);
                if (!File.Exists(path)) {
                    Debug.unityLogger.Log("url", url);
                    ObservableWWW.GetWWW(url)
                        .Subscribe(
                            success => {
                                Debug.unityLogger.Log("pathpath", "pathpath");
                                File.WriteAllBytes(path, success.bytes);
                            },
                            error => { Debug.Log("error1"); });
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

        GameObject.FindGameObjectWithTag("target").transform.localScale = new Vector3(scale, scale, 1.0f);

        messageTextMesh.text = loves[count].Message;
        count++;
    }
}