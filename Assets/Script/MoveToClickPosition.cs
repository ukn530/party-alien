using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClickPosition : MonoBehaviour {

	Vector3 clickPosition;
	Vector3 movedVector;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		clickPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 pos = Input.mousePosition;
			pos.z = -Camera.main.transform.position.z;
			clickPosition = Camera.main.ScreenToWorldPoint(pos);
		}

		movedVector = clickPosition - transform.position;
		transform.position += (movedVector * Time.deltaTime * Application.targetFrameRate * 0.3f );
	}
}
