using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

public class DownloadImages : MonoBehaviour {

	//private string path = "Assets/Resources/textures/ai.jpg";
	// Use this for initialization
	// index
	private string cae ="";
	const string serve = "http://superkuma.net/textures/";
	const string p_url = "http://superkuma.net/DB";
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
		var msg = new List<string>();
		var img = new List<string>();
		WWW result = new WWW(p_url);
		yield return result;
		if (result.error == null) {
			JSONObject json = new JSONObject (result.text);
			int rowCount = json.Count;
			for (int i = 0;i<json.Count; i++) {
				JSONObject jsoncur = json[i];
				JSONObject jsonmsg = jsoncur.GetField ("message");
				JSONObject jsonimg = jsoncur.GetField ("file");
				msg.Add(jsonmsg.str);
				img.Add(jsonimg.str);
				Debug.Log (jsonmsg.str);
			}
		}
		Debug.Log (msg.Count);
		for(int i = 0;i<msg.Count;i++){
			string path = serve + img [i];
			using (UnityWebRequest www = UnityWebRequest.Get (path)) {
				yield return www.Send();
				if (www.isNetworkError) {
				} else {
					cae = Application.temporaryCachePath;
				    File.WriteAllBytes (cae + "/" + i + ".png", www.downloadHandler.data);
				}
			}
		}
//		for (int i = 1; i < 6; i++) {
//			string path = serve + i +".png";
//			using (UnityWebRequest www = UnityWebRequest.Get (path)) {
//				yield return www.Send ();
//				if (www.isNetworkError) {
//				} else {
//					print (Application.temporaryCachePath + "/" + i + ".png");
//					cae = Application.temporaryCachePath;
//					File.WriteAllBytes (cae + "/" + i + ".png", www.downloadHandler.data);
//				}
//			}
//		}

	}	
}
