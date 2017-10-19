using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine;
using System.IO;

public class DownloadImages : MonoBehaviour {

	//private string path = "Assets/Resources/textures/ai.jpg";
	// Use this for initialization
	// index
	private string cae ="";
	const string serve = "http://superkuma.net/textures/";
	const string p_url = "http://superkuma.net/DB";
	private List<string> messages;
	void Start(){
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
		messages = new List<string>();
		var msg = new List<string>();
		var img = new List<string>();
		WWW result = new WWW(p_url);
		yield return result;
		Debug.Log (result.text);
		if (result.error == null) {
			JSONObject json = new JSONObject (result.text);
			int rowCount = json.Count;
			for (int i = 0;i<json.Count; i++) {
				JSONObject jsoncur = json[i];
				JSONObject jsonmsg = jsoncur.GetField ("message");
				JSONObject jsonimg = jsoncur.GetField ("file");
				messages.Add(jsonmsg.str);
				img.Add(jsonimg.str);
				Debug.Log (jsonmsg.str);
			}
		}
		Debug.Log (messages.Count);
		for(int i = 0;i<messages.Count;i++){
			string path = serve + img [i];
			Debug.Log (serve+img[i]+"To"+cae+"/"+i+".png");
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
	public string getMessage(int i){
		return messages[i];
	}
	public int getMaxrange(){
		return messages.Count ();
	}
}

