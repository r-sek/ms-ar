using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UniRx;

public class start : MonoBehaviour {
	private const string SERVER_URL = "http://superkuma.net/api/json";
	private const string FILE_HEAD = "file://";
	[DllImport ("__Internal")]
	private static extern void OpenCameraRoll (string path);
	private byte[] imageBinary;
	private string text = "";
	private Texture2D post;
	private string filename = "";
	public Image image;
	// Use this for initialization
	void Start () {

		ChangeImage ();
		var inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
		var returnBtn = GameObject.Find ("Canvas/ReturnBtn").GetComponent<Button> ();


		inputField.OnEndEditAsObservable()
			.Subscribe(
				s => { text = s; }
			);
		
		returnBtn.OnClickAsObservable()
			.Subscribe(
				_ => SceneManager.LoadScene("MainView")
			);
	}
	// Update is called once per frame
	void Update () {

	}
	public void SetImage (string path)
	{
		Debug.unityLogger.Log("img", path);
		var texture2D = new Texture2D (2, 2);
		texture2D.LoadImage (System.IO.File.ReadAllBytes (path));
		var sprite = Sprite.Create (texture2D, new Rect (0, 0, texture2D.width, texture2D.height), 0.5f * Vector2.one);
		image.sprite = sprite;
		imageBinary = Utilities.LoadbinaryBytes (path);
		var f = new FileInfo (path);
		filename = f.Name;
	}
	public void ChangeImage(){
		#if UNITY_IOS
		OpenCameraRoll(Application.temporaryCachePath + "/tempImage");
		#else
		SceneManager.LoadScene("MainView");
		#endif
		post = new Texture2D (0, 0);
	}
	public void submitImage(){
		var formdata = new WWWForm();

		formdata.AddField("name", "gest");
		if (text != "") {
			formdata.AddField("message", text);
		}
		if (imageBinary.Length != 0) {
			formdata.AddBinaryData("upload_file", imageBinary , filename);
		}
		ObservableWWW.PostWWW(SERVER_URL, formdata)
			.Subscribe(
				result => { Debug.Log("success");}
				,error => { Debug.Log("ng");}
			);
		ChangeImage ();
	}
	public void returnView(){
		SceneManager.LoadScene ("MainView");
	}
}
