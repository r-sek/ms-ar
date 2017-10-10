using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

public class downloadScript : MonoBehaviour {

	//private string path = "Assets/Resources/textures/ai.jpg";
	// Use this for initialization
	// index
	private string cae ="";
	const string serve = "http://superkuma.net/textures/";

	void Start () {
		StartCoroutine (GetFile ());
	}

	// Update is called once per frame
	void Update () {
		//Texture tex = readByBinary (readPngFile(path));
		//Renderer renderer = this.GetComponent<Renderer>();
		//renderer.material.mainTexture = tex;
		//print ("a");
		//			if(i < 5){
		//				i++;
		//			}else{
		//				i = 1;
		//			}
		//			string path = serve + i + ".png";
		//			StartCoroutine(GetFile(path));
	}

	public Texture readByBinary(byte[] bytes) {
		Texture2D texture = new Texture2D (1, 1);
		texture.LoadImage (bytes);
		return texture;
	}

	public byte[] readPngFile(string path) {
		using (FileStream fileStream = new FileStream (path, FileMode.Open, FileAccess.Read)) {
			BinaryReader bin = new BinaryReader (fileStream);
			byte[] values = bin.ReadBytes ((int)bin.BaseStream.Length);
			bin.Close ();
			return values;
		}
	}

	IEnumerator GetFile(){
		for (int i = 1; i < 6; i++) {
			string path = serve + i +".png";
			using (UnityWebRequest www = UnityWebRequest.Get (path)) {
				yield return www.Send ();
				if (www.isError) {
				} else {
					//Texture tex = readByBinary (www.downloadHandler.data);
					//Renderer renderer = this.GetComponent<Renderer>();
					//renderer.material.mainTexture = tex;
					print (Application.temporaryCachePath + "/" + i + ".png");

					cae = Application.temporaryCachePath;
					File.WriteAllBytes (cae + "/" + i + ".png", www.downloadHandler.data);
				}
			}
		}
	}
}
