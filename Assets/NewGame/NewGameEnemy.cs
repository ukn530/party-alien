using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameEnemy : MonoBehaviour {

	public float torque;
	Rigidbody rb;
	Vector3 firstPos;
	GameObject mainCamera;

	// Use this for initialization
	void Start () {
		firstPos = transform.position;
		rb = GetComponent<Rigidbody>();
		rb.maxAngularVelocity = 2;

		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {

		if (NewGameGameScript.newGameState == NewGameGameScript.NewGameState.Play) {
			rb.AddTorque(transform.forward * torque * -1);
		}
		if (transform.position.y < 0) {
			reset ();
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject == GameObject.FindGameObjectWithTag("Player")) {
			reset();
		}
	}

	public void reset () {
		mainCamera.GetComponent<NewGameGameScript> ().resetSong ();
		transform.position = firstPos;
	}
}
