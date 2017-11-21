using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour {

	[DllImport ("__Internal")]
	private static extern void OpenCameraRoll (string path);

	// Use this for initialization
	void Start () {
		#if UNITY_IOS
			OpenCameraRoll(Application.temporaryCachePath + "/tempImage");
		#else
			SceneManager.LoadScene("MainView");
		#endif
	}

	// Update is called once per frame
	void Update () {

	}
}
