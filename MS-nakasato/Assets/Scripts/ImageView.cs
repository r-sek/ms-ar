using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ImageView : MonoBehaviour {
//    private RectTransform content;

//    private bool ishide = false;

    // Use this for initialization
    void Start() {
//        var filelist = PostData.fileList;
//
//        var path = filelist[0];
////        setImage(path);
//        var sw = System.Diagnostics.Stopwatch.StartNew();
//        sw.Start();
//
//
//        StartCoroutine(LoadTexture2D(path));
//        sw.Stop();
//        Debug.unityLogger.Log("stop", sw.Elapsed);
//
//
//        GetComponent<Button>().onClick.AddListener(() => { });
    }

//    private const string FILE_HEADER = "file://";
//
//    private IEnumerator LoadTexture2D(string path) {
//        if (!File.Exists(path)) {
//            yield break;
//        }
//        using (var www = new WWW(FILE_HEADER + path)) {
//            while (!www.isDone) {
//                yield return new WaitForEndOfFrame();
//            }
//            var tex = www.texture;
//            yield return new WaitForEndOfFrame();
//            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
//            GetComponent<Image>().sprite = sprite;
//        }
//    }


//    private Sprite GetSpriteFromTexture2D(Texture2D t2D) {
//        Sprite sprite = null;
//        if (t2D) {
//            sprite = Sprite.Create(t2D, new Rect(0, 0, t2D.width, t2D.height), Vector2.zero);
//        }
//        return sprite;
//    }
//
//    private Texture2D GetTexture2DFromFile(string path) {
//        Texture2D t2D;
//        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
//            using (var bin = new BinaryReader(fs)) {
//                var readBinary = bin.ReadBytes((int) bin.BaseStream.Length);
//                t2D = new Texture2D(0, 0);
//                t2D.LoadImage(readBinary);
//                t2D.Apply(true, true);
//            }
//        }
//        return t2D;
//    }
}