using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class AutoFocus : MonoBehaviour {

	private bool mVuforiaStarted = false;
	// Use this for initialization
	void Start () {
		var vuforia = VuforiaARController.Instance;
		if (vuforia != null) {
			vuforia.RegisterVuforiaStartedCallback(StartAfterVuforia);
			
		}
	}

	private void StartAfterVuforia() {
		mVuforiaStarted = true;
		SetAutoFocus();
	}

	private void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus) return;
		if (mVuforiaStarted) {
			SetAutoFocus();
		}
	}

	private void SetAutoFocus() {
		if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO)) {
			Debug.Log("autofocus");
		}
		else {
			Debug.Log("doesn't support auto forcus");
		}
	}
}
