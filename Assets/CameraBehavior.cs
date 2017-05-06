using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playStartAnimation () {
		Debug.Log ("camera behabior");
		GetComponent<Animator> ().SetTrigger ("start");
	}
}
