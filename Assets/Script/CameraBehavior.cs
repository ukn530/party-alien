using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

	public GameObject player;
	public GameObject enemy;
	float distance;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameScript.state == GameScript.State.Enemy ||GameScript.state == GameScript.State.Player) {
			
			Vector3 centerPos = (player.transform.position + enemy.transform.position) / 2;

			float distanceX = Mathf.Abs(player.transform.position.x - enemy.transform.position.x) + 10;
			float distanceY = Mathf.Abs(player.transform.position.y - enemy.transform.position.y) + 10;
			float distanceToCamera;

			if (distanceX > distanceY * Camera.main.aspect) {
				distanceToCamera = (distanceX / Camera.main.aspect) * 0.5f / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
			} else {
				distanceToCamera = distanceY * 0.5f / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
			}

			transform.position = new Vector3 (centerPos.x, centerPos.y, centerPos.z - distanceToCamera);
		}
	}

	public void playStartAnimation () {
		
		Hashtable animationGetStartPos = new Hashtable ();
		animationGetStartPos.Add ("position", new Vector3 (13f, 6f, -11f));
		animationGetStartPos.Add ("time", 4);
		animationGetStartPos.Add ("easetype", "easeInOutCubic");
		iTween.MoveTo(gameObject, animationGetStartPos);
	}
}
