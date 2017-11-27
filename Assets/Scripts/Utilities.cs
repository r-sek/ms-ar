using System.IO;
using UnityEngine;

public static class Utilities {
    public static byte[] LoadbinaryBytes(string path) {
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
            using (var bin = new BinaryReader(fs)) {
                return bin.ReadBytes((int) bin.BaseStream.Length);
            }
        }
    }

    public static Sprite GetSpriteFromTexture2D(Texture2D t2D) {
        Sprite sprite = null;
        if (t2D) {
            sprite = Sprite.Create(t2D, new Rect(0, 0, t2D.width, t2D.height), Vector2.zero);
        }
        return sprite;
    }
    
    public static Texture2D GetTexture2DFromBytes(byte[] bytes) {
        var t2D = new Texture2D(0, 0);
        t2D.LoadImage(bytes);
        t2D.Apply(true, true);
        return t2D;
    }

/*public static byte[] GetImageByte(string path) {
#if UNITY_ANDROID
        using (var p = new AndroidJavaClass("jp.ac.hal.unityandroidplugin.FileAccessKt")) {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (var context = activity.Call<AndroidJavaObject>("getApplicationContext")) {
                        return p.CallStatic<byte[]>("getThumbnails", context, path);
                    }
                }
            }
        }
#else
            using (var www = new WWW(FILE_HEADER + path)) {
                while (!www.isDone) {
                    yield return new WaitForEndOfFrame();
                }
                 return www.bytes;
            }
#endif
    }
*/
}