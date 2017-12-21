using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic;

public class MyTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {
    private TrackableBehaviour mTrackableBehaviour;
	public ParticleSystem ps;
	public DownloadImages downloadimage;
	public GameObject cube;
	public AudioSource audio;
	public TextMesh textmesh;
	private string textString;
	private List<string> textArray;
	private Sprite sprite;
	private SpriteRenderer target;
	private MeshRenderer text;
	private MeshRenderer textCanvas;
	private Vector3 pos;
	private Vector3 posMax;
	private Coroutine fadein;
	private Coroutine fadeout;
	private Coroutine movie;
    // Use this for initialization
    void Start() {
		ps.Stop ();
		textCanvas = cube.GetComponent<MeshRenderer> ();
		target = GameObject.Find ("Sprite").GetComponent<SpriteRenderer> ();
		text = GameObject.Find ("message").GetComponent<MeshRenderer> ();
		sprite = target.sprite;
		pos = new Vector3 (sprite.bounds.extents.x,sprite.bounds.extents.y - 0.2f,sprite.bounds.extents.z);
		ps.transform.position = pos;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour) {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
            OnTrackingFound();
        }
        else {
            OnTrackingLost();
        }
    }

    private void OnTrackingFound() {
		audio.Play ();
		if (fadeout != null) {
			StopCoroutine (fadeout);
		}
		fadein = StartCoroutine (Fadein());
		movie = StartCoroutine (Movie());
        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    }

    private void OnTrackingLost() {
		if (fadein != null) {
			StopCoroutine (fadein);
			StopCoroutine (movie);
		}
        fadeout = StartCoroutine (Fadeout());
        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
    }

	private IEnumerator Movie(){
		ps.Play ();
		yield return new WaitForSeconds (1.0f);
		ps.Stop ();
		yield return new WaitForSeconds (2.0f);
	}
	private IEnumerator Fadein(){
		downloadimage.Texturechange();
		textArray = new List<string>();
		textString = textmesh.text;
		string[] array = textString.Split('\n');
		int maxstrLength = 0;
		foreach(string str in array){
			textCanvas.transform.localScale = new Vector3 (1f,0.01f,0.6f);
			text.transform.localPosition = new Vector3 (-0.4f,0.7f,-0.15f);
			if (str.Length > 8 && maxstrLength < str.Length) {
				maxstrLength = str.Length;
				float size = str.Length * 0.13f;
				Vector3 posi = text.transform.position;
				textCanvas.transform.localScale = new Vector3 (size, 0.01f, 0.6f);
				posi.x -= str.Length * 9.5f;
				Debug.Log (posi);
				text.transform.position = posi;
			} 
		}
		textmesh.text = textString;
		textCanvas.enabled = true;
		text.enabled = true;
		var color = target.color;
		color.a = 0.01f;
		target.material.color = color;
		target.enabled = true;
		Color alpha = new Color (0f,0f,0f,0.05f);
		for (; target.material.color.a < 1; target.material.color += alpha) {
			yield return new WaitForSeconds (0.001f);
		}
	}
	private IEnumerator Fadeout(){
		textCanvas.enabled = false;
		text.enabled = false;
		var color = target.color;
		target.material.color = color;
		Color alpha = new Color (0f,0f,0f,0.05f);
		for(;target.material.color.a > 0;target.material.color -= alpha){
			yield return new WaitForSeconds (0.001f);
		}
		target.enabled = false;
	}
}