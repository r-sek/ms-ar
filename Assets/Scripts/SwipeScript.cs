using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(SwipeGesture))]
public class SwipeScript : MonoBehaviour {
    private SwipeGesture swipeGesture;
	public Camera maincamera;
	public CanvasRenderer renderer;
	private AsyncOperation async;
    // Use this for initialization
    void Start() {
		renderer.SetAlpha (0);
	}

    void OnEnable() {
        swipeGesture = GetComponent<SwipeGesture>();
        swipeGesture.OnSwipeRight
            .Subscribe(_ => {
                Debug.unityLogger.Log("swipe", "右");
				StartCoroutine(ChangeScene());
            });
        swipeGesture.OnSwipeLeft
            .Subscribe(_ => {
		        SceneManager.LoadScene("SphereScene");
		        Debug.unityLogger.Log("swipe", "左");
	        });
        swipeGesture.OnSwipeDown
            .Subscribe(_ => { Debug.unityLogger.Log("swipe", "下"); });
        swipeGesture.OnSwipeUp
            .Subscribe(_ => { Debug.unityLogger.Log("swipe", "上"); });
    }

	private IEnumerator ChangeScene(){
		int i = 0;
		#if UNITY_IOS
		async = SceneManager.LoadSceneAsync("iOSUploadScene");
		while(!async.isDone){
			yield return new WaitForSeconds(0.5f);
			if(i%2 == 0){
				renderer.SetAlpha(0);	
			}else{
				renderer.SetAlpha(1);
			}
			i++;
		}
		#else
		async = SceneManager.LoadSceneAsync("UploadScene");
		while(!async.isDone){
			yield return new WaitForSeconds(0.5f);
			if(i%2 == 0){
				renderer.SetAlpha(0);
			}else{
				renderer.SetAlpha(1);
			}
			i++;
		}
		#endif
	}
}