using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameCamera : MonoBehaviour {
	GameObject player;
	Vector3 diffPos;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		diffPos = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = new Vector3 (player.transform.position.x + diffPos.x , transform.position.y, transform.position.z);
	}
}
