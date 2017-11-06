using UnityEngine;
using Vuforia;

public class MyTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {
    private TrackableBehaviour mTrackableBehaviour;

    // Use this for initialization
    void Start() {
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
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);

        GetComponent<DownloadImages>().Texturechange();

        // Enable rendering:
        foreach (var component in rendererComponents) {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (var component in colliderComponents) {
            component.enabled = true;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    }

    private void OnTrackingLost() {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        // Disable rendering:
        foreach (var component in rendererComponents) {
            component.enabled = false;
        }

        // Disable colliders:
        foreach (var component in colliderComponents) {
            component.enabled = false;
        }

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
    }
}