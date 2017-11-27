using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour {
	private ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = this.GetComponent<ParticleSystem> ();
		ps.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
