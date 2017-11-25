using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {
    // Use this for initialization
    private Canvas dialogCanvas;

    private Button button;
    void Start() {
        dialogCanvas = gameObject.GetComponent<Canvas>();
        button = dialogCanvas.GetComponentInChildren<Button>();
        Debug.unityLogger.Log("dialog","start");
        if (dialogCanvas != null) {
            dialogCanvas.enabled = false;
        }

        button.OnClickAsObservable().Subscribe(_=> {
            dialogCanvas.enabled = false;
            Debug.unityLogger.Log("dialog","false");
        });
    }

    public void ViewDialog() {
        if (dialogCanvas == null) return;
        dialogCanvas.enabled = true;
        Debug.unityLogger.Log("dialog","true");
    }
    // Update is called once per frame
    void Update() {
    }
}