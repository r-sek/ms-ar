using UniRx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class init : MonoBehaviour {
	const string SERVER_URL = "http://superkuma.net/api/users";
	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey ("Init")) {
			string str = "";
			for (int i = 0; i < 10; i++) {
				int result = Random.Range (0, 9);
				str = str + result.ToString ();
			}
			var formdata = new WWWForm ();

			formdata.AddField ("id", str);
			ObservableWWW.Post (SERVER_URL, formdata)
				.Subscribe (
				result => {
					Debug.unityLogger.Log ("result", result);

					Debug.Log ("success");
				}, 
				error => {
					GameObject.Find ("Dialog").GetComponent<Dialog> ().ViewDialog ();
					Debug.Log ("ng");
				}
			);
			PlayerPrefs.SetString ("Init", str);
			Debug.Log (str);
		} 
	}

		
		

	
	// Update is called once per frame
	void Update () {
		
	}
}
