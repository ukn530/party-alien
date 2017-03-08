using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

	public enum State {
		Enemy,
		Player
	}

	public static State state;
	public float timeCounter;
	public float beatTimeCounter;

	public static int[][] scores;
	public static float beatTime; // 命名はtimePerBeatとかの方が良い
	public static int scoreNum; // indexOfScoreとか

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		scores = new int[][]{
			new int[] {0,0,1,1,0,0,1,0}, 
			new int[] {0,0,1,1,0,0,1,0}, 
			new int[] {0,1,0,1,0,1,1,1}, 
			new int[] {1,0,1,0,1,1,0,1},
			new int[] {1,0,1,0,0,1,0,1},
			new int[] {0,1,0,1,0,1,1,1},
			new int[] {0,1,0,1,1,1,0,1},
			new int[] {0,0,1,0,0,0,1,1,0,0,1,1,0,0,1,0}
		};


		timeCounter = 0;
		beatTimeCounter = 0;

		scoreNum = 0;
		beatTime = 2.0f / GameScript.scores [scoreNum].Length;

		audioSource = GetComponent<AudioSource> ();

		//audioSource.PlayDelayed (0.03f);
		Debug.Log(state);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(state);
		timeCounter += Time.deltaTime;

		if (timeCounter > 2) {
			timeCounter -= 2f;
			if (state == State.Player) {
				scoreNum = Random.Range(0, scores.Length);
				beatTime = 2.0f / scores[scoreNum].Length;
				state = State.Enemy;
				Debug.Log ("State Enemy");
			} else {
				state = State.Player;
				Debug.Log ("State Player");
			}
		}
	}

	void FixedUpdate() {
		beatTimeCounter += Time.deltaTime;

		if (beatTimeCounter > 0.5) {
			audioSource.Play ();
			beatTimeCounter -= 0.5f;
		}
	}
}
