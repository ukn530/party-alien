using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	public enum State {
		Enemy,
		Player
	}

	public static State state;
	public float timeCounter;
	public float beatTimeCounter;

	public Text fpsText;

	public static int[][] scores;
	public static float beatTime; // 命名はtimePerBeatとかの方が良い
	public static int scoreNum; // indexOfScoreとか

	public GameObject noteWrapperEnemy;
	public GameObject noteRest;
	public GameObject noteNormal;
	public GameObject noteLoud;
	public GameObject iconEnemy;
	public GameObject score;

	GameObject icon;

	AudioSource audioSource;

	Vector3 posIconMove;
	Vector3 posIconMoveTo;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		scores = new int[][]{
//			new int[] {0,0,1,1,0,0,1,0}, 
//			new int[] {0,0,1,1,0,0,1,0}, 
//			new int[] {0,1,0,1,0,1,1,1}, 
//			new int[] {1,0,1,0,1,1,0,1},
//			new int[] {1,0,1,0,0,1,0,1},
//			new int[] {0,1,0,1,0,1,1,1},
			new int[] {0,1,0,1,1,1,0,1},
			new int[] {0,0,1,0,0,0,1,1,0,0,1,1,0,0,1,0}
		};


		timeCounter = 0;
		beatTimeCounter = 0;

		scoreNum = 0;
		beatTime = 2.0f / GameScript.scores [scoreNum].Length;

		posIconMove = Vector3.zero;
		posIconMoveTo = Vector3.zero;

		icon = Instantiate (iconEnemy);
		calcPositionForIcon ();
		icon.transform.position = posIconMove;
		icon.transform.parent = score.transform;

		DrawScore ();

		audioSource = GetComponent<AudioSource> ();

		Debug.Log(state);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(state);
		timeCounter += Time.deltaTime;
		fpsText.text = 1f / Time.deltaTime + "fps";

		if (timeCounter > 2) {
			timeCounter -= 2f;
			if (state == State.Player) {
				scoreNum = Random.Range(0, scores.Length);
				beatTime = 2.0f / scores[scoreNum].Length;
				state = State.Enemy;
				DrawScore ();
				icon.transform.position = posIconMove;
				Debug.Log ("State Enemy");
			} else {
				state = State.Player;
				Debug.Log ("State Player");
			}
		}

		if (state == State.Enemy) {
			// 1フレームでどれだけ進めばいいか。
			// かかるフレーム数にdeltaTimeをかける
			// (100 - 10)をbeatTime*scores[scoreNum].Length秒で進めばいい
			icon.SetActive (true);
			icon.transform.Translate (Vector3.right * Time.deltaTime * (posIconMoveTo.x - posIconMove.x) / 2);
		} else if (state == State.Player) {
			icon.SetActive (false);
		}
	}

	void FixedUpdate () {
		beatTimeCounter += Time.deltaTime;

		if (beatTimeCounter > 0.5) {
			audioSource.Play ();
			beatTimeCounter -= 0.5f;
		}
	}

	void DrawScore () {
		
		// スコアを削除 
		foreach ( Transform n in noteWrapperEnemy.transform ) {
			GameObject.Destroy(n.gameObject);
		}

		for (int i = 0; i < scores [scoreNum].Length; i++) {
			if (scores [scoreNum] [i] == 0) {

				Instantiate (noteRest, noteWrapperEnemy.transform);

			} else if (scores [scoreNum] [i] == 1) {

				Instantiate (noteNormal, noteWrapperEnemy.transform);
				
			} else if (scores [scoreNum] [i] == 2) {

				Instantiate (noteLoud, noteWrapperEnemy.transform);

			}
		}
	}

	void calcPositionForIcon () {
		posIconMove = noteWrapperEnemy.transform.GetChild (0).transform.position;
		Vector3 intervalIconMove = noteWrapperEnemy.transform.GetChild (1).transform.position - posIconMove;
		posIconMoveTo = noteWrapperEnemy.transform.GetChild (scores [scoreNum].Length - 1).transform.position + intervalIconMove;
	}
}
