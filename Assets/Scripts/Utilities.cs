using UnityEngine;
using System.Collections;
using System.IO;

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
}