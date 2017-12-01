using UnityEngine;
using Vuforia;
using System.Collections;
using UnityEngine.Video;

public class VideoScripts : MonoBehaviour, ITrackableEventHandler {
	private TrackableBehaviour mTrackableBehaviour;
	public ParticleSystem ps;
	public VideoPlayer vp;
	private SpriteRenderer target;
	private Vector3 pos;

	// Use this for initialization
	void Start() {
		target = GameObject.Find ("Sprite").GetComponent<SpriteRenderer> ();
		var sprite = target.sprite;
		pos = new Vector3 (sprite.bounds.extents.x,sprite.bounds.extents.y,sprite.bounds.extents.z);
		ps.transform.position = pos;
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		ps.Stop ();
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
		target.enabled = true;
		vp.Play ();
		StopCoroutine (Fadeout());
		StartCoroutine (movie());
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
	}

	private void OnTrackingLost() {
		StopCoroutine (movie());
		StartCoroutine (Fadeout());
		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
	}

	private IEnumerator movie(){
		ps.Play ();
		yield return new WaitForSeconds (1.0f);
		ps.Stop ();
		yield return new WaitForSeconds (2.0f);
	}

	private IEnumerator Fadeout(){
		vp.Stop();
		var color = target.color;
		target.material.color = color;
		Color alpha = new Color (0f,0f,0f,0.01f);
		for(;target.material.color.a > 0;target.material.color -= alpha){
			yield return new WaitForSeconds (0.015f);
		}
		target.enabled = false;
	}
}