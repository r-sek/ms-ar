using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SwipeGesture))]
public class SwipeScript : MonoBehaviour {
    private SwipeGesture swipeGesture;

    // Use this for initialization
    void Start() {
    }

    void OnEnable() {
        swipeGesture = GetComponent<SwipeGesture>();
        swipeGesture.OnSwipeRight
            .Subscribe(_ => {
                Debug.unityLogger.Log("swipe", "右");
                #if UNITY_IOS
					SceneManager.LoadScene("iOSUploadScene");
				#else
                    SceneManager.LoadScene("UploadScene");
                #endif
            });
        swipeGesture.OnSwipeLeft
            .Subscribe(_ => { Debug.unityLogger.Log("swipe", "左"); });
        swipeGesture.OnSwipeDown
            .Subscribe(_ => { Debug.unityLogger.Log("swipe", "下"); });
        swipeGesture.OnSwipeUp
            .Subscribe(_ => { Debug.unityLogger.Log("swipe", "上"); });
    }
}