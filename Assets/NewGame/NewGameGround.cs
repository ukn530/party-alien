using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		Debug.Log(GetComponent<MeshRenderer>().material.color);

//		GetComponent<MeshRenderer> ().material.color = new Color(1f, 1f, 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void ChangeColorWhite () {
		GetComponent<Animator> ().SetTrigger ("ChangeToWhite");
	}

	public void ChangeColorRed () {
		GetComponent<Animator> ().SetTrigger ("ChangeToRed");
	}
}
