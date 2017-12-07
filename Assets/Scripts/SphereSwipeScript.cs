
using UniRx;
using UnityEngine;

[RequireComponent(typeof(SwipeGesture))]
public class SphereSwipeScript : MonoBehaviour {
	private SwipeGesture swipeGesture;
	private Camera mainCamera;
	private Vector3 newAngle = new Vector3(0,0,0);

	void OnEnable() {
		mainCamera = Camera.main;
		swipeGesture = GetComponent<SwipeGesture>();

		swipeGesture.OnTap.Subscribe(_ => {
			Debug.Log("tap");
		});

		swipeGesture.OnDoubletap.Subscribe(_ => {
			Debug.Log("double tap");
		});
		
		swipeGesture.OnSwipeRight
			.Subscribe(_ => {
				newAngle = mainCamera.transform.localEulerAngles;
				newAngle.y += 15f;
				mainCamera.transform.localEulerAngles = newAngle;
				Debug.unityLogger.Log("swipe", "右");
			});
		swipeGesture.OnSwipeLeft
			.Subscribe(_ => { 
				newAngle = mainCamera.transform.localEulerAngles;
				newAngle.y -= 15f;
				mainCamera.transform.localEulerAngles = newAngle;
				Debug.unityLogger.Log("swipe", "左"); 
			});
		swipeGesture.OnSwipeDown
			.Subscribe(_ => { 
				newAngle = mainCamera.transform.localEulerAngles;
				newAngle.x += 15f;
				mainCamera.transform.localEulerAngles = newAngle;
				Debug.unityLogger.Log("swipe", "下"); 
			});
		swipeGesture.OnSwipeUp
			.Subscribe(_ => {
				newAngle = mainCamera.transform.localEulerAngles;
				newAngle.x -= 15f;
				mainCamera.transform.localEulerAngles = newAngle;
				Debug.unityLogger.Log("swipe", "上");
			});
	}
}
