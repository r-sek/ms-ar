using UnityEngine;
using Vuforia;
using System.Collections;

public class MyTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {
    private TrackableBehaviour mTrackableBehaviour;
	public ParticleSystem ps;
	private SpriteRenderer target;
	private MeshRenderer text;
	private Vector3 pos;
    // Use this for initialization
    void Start() {
		target = GameObject.FindGameObjectWithTag ("target").GetComponent<SpriteRenderer> ();
		text = GameObject.FindGameObjectWithTag ("message").GetComponent<MeshRenderer> ();
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
		StopCoroutine (Fadeout());
		StartCoroutine (Fadein());
		StartCoroutine (movie());
        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    }

    private void OnTrackingLost() {
		StopCoroutine (Fadein());
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
	private IEnumerator Fadein(){
		GetComponent<DownloadImages>().Texturechange();
		var color = target.color;
		color.a = 0.01f;
		target.material.color = color;
		target.enabled = true;
		Color alpha = new Color (0f,0f,0f,0.01f);
		for (; target.material.color.a < 1; target.material.color += alpha) {
			yield return new WaitForSeconds (0.015f);
		}
		text.enabled = true;
	}
	private IEnumerator Fadeout(){
		var color = target.color;
		target.material.color = color;
		Color alpha = new Color (0f,0f,0f,0.01f);
		for(;target.material.color.a > 0;target.material.color -= alpha){
			yield return new WaitForSeconds (0.015f);
		}
		target.enabled = false;
		text.enabled = false;
	}
}