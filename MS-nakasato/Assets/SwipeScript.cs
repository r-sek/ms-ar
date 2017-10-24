using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeScript : MonoBehaviour {

	public Vector3 start;
	public Vector3 end;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			start = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(0)){
			end = Input.mousePosition;
			if (start.x > end.x) {
				start = new Vector3(0,0,0);
				end = new Vector3(0,0,0);
				SceneManager.LoadScene ("UploadScene");
			}
		}
	}
}
