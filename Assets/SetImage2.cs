using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetImage2 : MonoBehaviour {
	private int count;
	private string[] mes = {"幸せ","大好き！","ハート","愛してる","１周年"};
	public SpriteRenderer spriteRenderer;
	public TextMesh messageTextMesh;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	public void Texturechange() {
		if (count == 5) {
			count = 0;
		}
		spriteRenderer.sprite = Resources.Load ("ImageTarget4/"+count,typeof(Sprite)) as Sprite;
		messageTextMesh.text = mes[count];
		count++;
	}
}
