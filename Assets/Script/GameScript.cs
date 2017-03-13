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
	public GameObject iconPlayer;
	public GameObject score;

	AudioSource audioSource;

	float posIconMoveX;
	float posIconMoveToX;

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

		iconEnemy.transform.position = new Vector3(posIconMoveX, iconEnemy.transform.position.y);
		iconPlayer.transform.position = new Vector3(posIconMoveX, iconPlayer.transform.position.y);
		calcPositionForIcon ();

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
				iconEnemy.transform.position = new Vector3(posIconMoveX, iconEnemy.transform.position.y);
			} else {
				state = State.Player;
				iconPlayer.transform.position = new Vector3(posIconMoveX, iconPlayer.transform.position.y);
			}
		}

		if (state == State.Enemy) {
			// 1フレームでどれだけ進めばいいか。
			// かかるフレーム数にdeltaTimeをかける
			// (100 - 10)をbeatTime*scores[scoreNum].Length秒で進めばいい
			iconPlayer.SetActive(false);
			iconEnemy.SetActive (true);
			iconEnemy.transform.Translate (Vector3.right * Time.deltaTime * (posIconMoveToX - posIconMoveX) / 2);
		} else if (state == State.Player) {
			iconEnemy.SetActive (false);
			iconPlayer.SetActive (true);
			iconPlayer.transform.Translate (Vector3.right * Time.deltaTime * (posIconMoveToX - posIconMoveX) / 2);
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

	void initIcon () {
		
	}

	void calcPositionForIcon () {
		posIconMoveX = noteWrapperEnemy.transform.GetChild (0).transform.position.x;
		float intervalIconMoveX = noteWrapperEnemy.transform.GetChild (1).transform.position.x - posIconMoveX;
		posIconMoveToX = noteWrapperEnemy.transform.GetChild (scores [scoreNum].Length - 1).transform.position.x + intervalIconMoveX;
	}
}
