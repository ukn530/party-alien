using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour {


	// Use this for initialization
	void Start () {
		WebCamDevice[] devices = WebCamTexture.devices;
		// display all cameras
		for (var i = 0; i < devices.Length; i++) {
			Debug.Log (devices [i].name);
		}

		WebCamTexture webcamTexture = new WebCamTexture(devices[0].name, 640, 360, 30);
		GetComponent<Renderer> ().material.mainTexture = webcamTexture;



		#if UNITY_EDITOR
		#elif UNITY_IOS
		transform.Rotate(new Vector3(0, -90, 0));
		#endif

		webcamTexture.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
