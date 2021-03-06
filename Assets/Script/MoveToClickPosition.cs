﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClickPosition : MonoBehaviour {

	Vector3 clickPosition;
	Vector3 movedVector;
	Vector3 forceVector;
//	Vector3 forceAngle;
	public GameObject rootBone;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		clickPosition = Vector3.zero;

		forceVector = Vector3.zero;
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

		#if UNITY_EDITOR
		#elif UNITY_IOS
		updateCameraRotation ();
		#endif

		forceVector += Vector3.down * 0.03f;

		if (Input.GetMouseButtonDown (0) || Input.GetKeyDown("space") || transform.position.y < -10f) {
			Vector3 pos = Input.mousePosition;
			pos.z = -Camera.main.transform.position.z;
			clickPosition = Camera.main.ScreenToWorldPoint(pos);


			Debug.Log ("clickPosOfScreenPoint:" + pos);
			Debug.Log ("clickPosOfWorldPoint:" + clickPosition);

			forceVector.y = 0.5f;
//			forceAngle = Vector3.forward * (Random.value - 0.5f) * 10f;


			if (transform.position.x < Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, -Camera.main.transform.position.z)).x) {
				forceVector.x = (Random.value + Random.value + Random.value + Random.value) * 0.2f;
			} else if (transform.position.x > Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, 0, -Camera.main.transform.position.z)).x) {
				forceVector.x = (Random.value + Random.value + Random.value + Random.value) * -0.2f;
			} else {
				forceVector.x = (Random.value + Random.value - 1f) * 0.25f;
			}

		}


//		if (Input.GetKeyDown ("left")) {
//			forceVector.x = -0.2f;
//		} else if (Input.GetKeyDown ("right")) {
//			forceVector.x = 0.2f;
//		}

//		movedVector = clickPosition - transform.position;
//		transform.position += (movedVector * Time.deltaTime * Application.targetFrameRate * 0.3f);

//		rootBone.transform.eulerAngles += forceAngle * Time.deltaTime * Application.targetFrameRate;
		transform.position += forceVector * Time.deltaTime * Application.targetFrameRate;
	}

	void updateCameraRotation() {
		Quaternion gattitude = Input.gyro.attitude;
		gattitude.x *= -1;
		gattitude.y *= -1;
		Camera.main.transform.localRotation = Quaternion.Euler(90, 0, 0) * gattitude;
	}
}
