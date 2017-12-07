using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class setImage : MonoBehaviour {

	private int count;
	private string[] mes = {"とても好きです","うちの猫です","ハート","犬","可愛い"};
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
		spriteRenderer.sprite = Resources.Load ("ImageTarget3/"+count,typeof(Sprite)) as Sprite;
		messageTextMesh.text = mes[count];
		count++;
	}
}
