using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

public class DownloadImages : MonoBehaviour {
    private const string SERVER_URL = "http://superkuma.net/textures/";
    private const string DB_SERVER_URL = "http://superkuma.net/DB";
    private List<Love> loves;
    private int count;
    private Renderer cubeRenderer;
    private TextMesh messageTextMesh;

    void Start() {
        count = 0;
        loves = new List<Love>();
        cubeRenderer = GameObject.FindGameObjectWithTag("target").GetComponent<Renderer>();
        messageTextMesh = GameObject.FindGameObjectWithTag("message").GetComponent<TextMesh>();
        StartCoroutine(GetFile());
    }
    
    // Update is called once per frame
    void Update() {
    }

    IEnumerator GetFile() {
        var result = new WWW(DB_SERVER_URL);
        yield return result;
        Debug.Log(result.text);
        if (result.error == null) {
            var json = new JSONObject(result.text);
            for (var i = 0; i < json.Count; i++) {
                var jsoncur = json[i];
                var jsonmsg = jsoncur.GetField("message");
                var jsonimg = jsoncur.GetField("file");
                loves.Add(new Love(jsonmsg.str,jsonimg.str));
                Debug.Log(jsonmsg.str);
            }
        }
        
        for (var i = 0; i < loves.Count; i++) {
            var path = SERVER_URL + loves[i].ImageName;
            if (!File.Exists(path)) continue;
            using (var www = UnityWebRequest.Get(path)) {
                yield return www.Send();
                if (www.isNetworkError) {
                }
                else {
                    File.WriteAllBytes(Application.temporaryCachePath + "/" + i + ".png", www.downloadHandler.data);
                    Debug.Log(path+"OK");
                }
            }
        }
    }
    
    public void Texturechange() {
        if (count > loves.Count - 1) {
            count = 0;
        }
        var textures = new Texture2D(0, 0);
        textures.LoadImage(Utilities.LoadbinaryBytes(Application.temporaryCachePath + "/" + count + ".png"));
        cubeRenderer.material.mainTexture = textures;
        messageTextMesh.text = loves[count].Message;
        count++;
    }
}