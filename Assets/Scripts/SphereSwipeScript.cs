using UniRx;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(SwipeGesture))]
public class SphereSwipeScript : MonoBehaviour {
    private SwipeGesture swipeGesture;
    private Camera mainCamera;
    private Vector3 newAngle = new Vector3(0, 0, 0);
    private VideoPlayer videoPlayer;
    private int index = 0;
    private Object[] videoClips;

    void OnEnable() {
        mainCamera = Camera.main;
        swipeGesture = GetComponent<SwipeGesture>();
        videoPlayer = GameObject.Find("/Sphere").GetComponent<VideoPlayer>();

        videoClips = Resources.LoadAll("Movies", typeof(VideoClip));

        swipeGesture.OnDoubletap.Subscribe(_ => {
            Debug.Log("Double tap");
            ChangeVideo();
        });
        swipeGesture.OnTap.Subscribe(count => {
            Debug.Log("single Tap");
            Play();
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

    public void Play() {
        if (videoPlayer.isPlaying) {
            videoPlayer.Pause();
        }
        else {
            videoPlayer.Play();
        }
    }

    public void ChangeVideo() {
        videoPlayer.Stop();
        // resourceの切り替え
        videoPlayer.clip = videoClips[index % 2] as VideoClip;
        index++;
    }
}