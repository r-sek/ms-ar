using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ChangeImage : MonoBehaviour {
	public string BASE_TEXTURE;
	public int i = 0;

	public Sprite image2;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
	public void texturechange(){
		i++;
		if (i > 5) {
			i = 1;
		}
		BASE_TEXTURE = Application.temporaryCachePath;
		GameObject cube = GameObject.FindGameObjectWithTag ("target");
		//Texture[] textures = Resources.LoadAll<Texture>(BASE_TEXTURE);
		Texture2D textures = new Texture2D(0,0);
		textures.LoadImage(LoadBin(BASE_TEXTURE + "/" + i + ".png"));
		Renderer renderer = cube.GetComponent<Renderer> ();
		renderer.material.mainTexture = textures;
	}
	public void textureback(){
		i--;
		if (i <= 1) {
			i = 5;
		}
		BASE_TEXTURE = Application.temporaryCachePath;
		GameObject cube = GameObject.FindGameObjectWithTag ("target");
		//Texture[] textures = Resources.LoadAll<Texture>(BASE_TEXTURE);
		Texture2D textures = new Texture2D(0,0);
		textures.LoadImage (LoadBin (BASE_TEXTURE + "/" + i + ".png"));
		Renderer renderer = cube.GetComponent<Renderer> ();
		renderer.material.mainTexture = textures;

	}
	byte[] LoadBin(string path){
		FileStream fs = new FileStream(path, FileMode.Open);
		BinaryReader br = new BinaryReader(fs);
		byte[] buf = br.ReadBytes( (int)br.BaseStream.Length);
		br.Close();
		return buf;
	}
}
