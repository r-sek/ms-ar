using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UniRx;

public class start : MonoBehaviour {
	private const string SERVER_URL = "http://superkuma.net/postData";

	[DllImport ("__Internal")]
	private static extern void OpenCameraRoll (string path);
	private byte[] imageBinary;
	private string text = "";
	private Texture2D post;
	public Image image;
	// Use this for initialization
	void Start () {
		#if UNITY_IOS
			OpenCameraRoll(Application.temporaryCachePath + "/tempImage");
		#else
			SceneManager.LoadScene("MainView");
		#endif

		var submitBtn = GameObject.Find("Canvas/SubmitBtn").GetComponent<Button>();
		var inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
		var returnBtn = GameObject.Find ("Canvas/ReturnBtn").GetComponent<Button> ();

		post = new Texture2D (0, 0);

		submitBtn.OnClickAsObservable().Subscribe(_ => {
			var formdata = new WWWForm();

			formdata.AddField("name", "gest");
			if (text != "") {
				formdata.AddField("message", text);
			}
			if (imageBinary.Length != 0) {
				formdata.AddBinaryData("uploadfile", imageBinary);
			}
			ObservableWWW.PostWWW(SERVER_URL, formdata)
				.Subscribe(
					result => { Debug.Log("success");}
					,error => { Debug.Log("ng");}
				);
		});
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
	}
}
