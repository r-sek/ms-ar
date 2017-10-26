using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ChangeImage : MonoBehaviour {
	public string BASE_TEXTURE;
	public int i = 0;
	public DownloadImages downloadimages;
	public Sprite image2;
	public TextMesh messagetm;
	public Renderer cuberenderer;

	// Use this for initialization
	void Start () {
		GameObject arcamera = GameObject.FindGameObjectWithTag ("arcamera");
		downloadimages =  arcamera.GetComponent<DownloadImages>();
		GameObject message = GameObject.FindGameObjectWithTag ("message");
		messagetm = message.GetComponent<TextMesh> ();
		GameObject cube = GameObject.FindGameObjectWithTag ("target");
		cuberenderer = cube.GetComponent<Renderer> ();
		BASE_TEXTURE = Application.temporaryCachePath;

	}

	// Update is called once per frame
	void Update () {
	}
	public void texturechange(){
		if (i > downloadimages.getMaxrange()-1) {
			i = 0;
		}
		//Texture[] textures = Resources.LoadAll<Texture>(BASE_TEXTURE);
		Texture2D textures = new Texture2D (0, 0);
		textures.LoadImage (LoadBin (BASE_TEXTURE + "/" + i + ".png"));
		cuberenderer.material.mainTexture = textures;
		messagetm.text = downloadimages.getMessage(i);
		i++;
	}
	public void textureback(){
		i--;
		if (i <= 0) {
			i = downloadimages.getMaxrange()-1;
		}
		//Texture[] textures = Resources.LoadAll<Texture>(BASE_TEXTURE);
		Texture2D textures = new Texture2D(0,0);
		textures.LoadImage (LoadBin (BASE_TEXTURE + "/" + i + ".png"));
		cuberenderer.material.mainTexture = textures;
		messagetm.text = downloadimages.getMessage (i);
		i++;
	}
	byte[] LoadBin(string path){
		FileStream fs = new FileStream(path, FileMode.Open);
		BinaryReader br = new BinaryReader(fs);
		byte[] buf = br.ReadBytes( (int)br.BaseStream.Length);
		br.Close();
		return buf;
	}
}

