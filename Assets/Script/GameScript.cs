using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	public enum State {
		Enemy,
		Player,
		Idle
	}

	enum Rate {
		Fail,
		Good,
		Great,
		Perfect
	}

	public static State state;
	Rate rate;


	int[][] scores = new int[][]{
		new int[] {0,1,0,1,0,1,0,1},
		new int[] {0,0,1,1,0,0,1,0}, 
		new int[] {0,1,1,0,1,1,1,0,1,0,1,1},
		new int[] {0,1,0,1,0,1,1,1}, 
		new int[] {1,0,1,0,1,1,0,1,1,0,1,0,1,1,0,1},
		new int[] {1,0,1,0,0,1,0,1},
		new int[] {0,1,0,1,0,1,1,1,0,1,0,1,0,1,1,1},
		new int[] {0,1,0,1,1,1,0,1},
		new int[] {0,0,1,0,0,0,1,1,0,0,1,1,0,0,1,0}
	};


	float BPM;

	float timeCounterInASong;

	float timePerQuarterBeat;
	float timeStompForBar;
	float timeStompForQuarterBeat;
	float timeStompForBeat;

	int beatIndex;
	int scoreIndex;


	public GameObject noteWrapperEnemy;
	public GameObject noteWrapperPlayer;
	public GameObject noteRest;
	public GameObject noteNormal;
	public GameObject noteLoud;
	public GameObject noteCircle;
	public GameObject iconEnemy;
	public GameObject iconPlayer;
	public GameObject score;


	public Text scoreText;
	public Text timingText;
	public Text fpsText;


	AudioSource audioSource;
	private AudioSource[] audioSources = new AudioSource[2];
	public AudioClip snare;
	public AudioClip kick;


	float posIconMoveX;
	float posIconMoveToX;


	int paddingOfScore;
	int point = 0;


	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		// 音の設定
		for (int i = 0; i < 2; i++) {
			GameObject child = new GameObject("AudioPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
		}

		audioSources [0].clip = kick;
		audioSources [1].clip = snare;

		audioSource = GetComponent<AudioSource> ();


		// リズムの設定
		beatIndex = 0;
		scoreIndex = 0;

		BPM = 120f;
		timePerQuarterBeat = 60f / BPM;
		timeStompForBar = 0;
		timeStompForQuarterBeat = 0;
		timeStompForBeat = 0;
		timeCounterInASong = 0;

		state = State.Idle;


		// スコアの表示
		drawScore ();
		calcPositionForIcon ();

		iconEnemy.transform.position = new Vector3(posIconMoveX, iconEnemy.transform.position.y);
		iconPlayer.transform.position = new Vector3(posIconMoveX, iconPlayer.transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {

		if (state == State.Enemy) {

			iconEnemy.transform.Translate (Vector3.right * Time.deltaTime * (posIconMoveToX - posIconMoveX) / 2);

		} else if (state == State.Player) {

			iconPlayer.transform.Translate (Vector3.right * Time.deltaTime * (posIconMoveToX - posIconMoveX) / 2);
			onTap ();
		}

		eachABar ();
		eachQuarterBeat ();
		eachABeat ();

		timeCounterInASong += Time.deltaTime;
		fpsText.text = 1f / Time.deltaTime + "fps";
	}


	void eachABar () {
		// 1小節ごとに1回呼ぶ
		if (timeCounterInASong >= timeStompForBar) {

			timeStompForBar += timePerQuarterBeat * 4f;
			Debug.Log ("timeStompForBar: " + timeStompForBar);
			
			if (state == State.Player || state == State.Idle) {
				state = State.Enemy;
				
				scoreIndex = Random.Range(0, scores.Length);
				drawScore ();

				iconEnemy.transform.position = new Vector3(posIconMoveX, iconEnemy.transform.position.y);

				iconPlayer.SetActive(false);
				iconEnemy.SetActive (true);

			} else {
				state = State.Player;
				
				iconPlayer.transform.position = new Vector3(posIconMoveX, iconPlayer.transform.position.y);

				iconEnemy.SetActive (false);
				iconPlayer.SetActive (true);
			}
			beatIndex = 0;
		}
	}


	void eachQuarterBeat () {
		// 1拍ごと
		if (timeCounterInASong >= timeStompForQuarterBeat) {

			timeStompForQuarterBeat += timePerQuarterBeat;
			Debug.Log ("timeStompForQuarterBeat: " + timeStompForQuarterBeat);

			audioSources [0].PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);
		}
	}


	void eachABeat () {

		// 1音符ごと
		if (timeCounterInASong > timeStompForBeat) {
			timeStompForBeat += (timePerQuarterBeat * 4) / scores[scoreIndex].Length;

			if (state == State.Enemy) {
				
				if (scores[scoreIndex][beatIndex] > 0) {
					
					audioSources [1].PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);
				}
			}

			beatIndex++;
		}
	}


	void onTap () {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) {

			Instantiate(noteCircle, iconPlayer.transform.position, Quaternion.identity, noteWrapperPlayer.transform);
			//checkTiming ();
			tapBeat ();
		}
	}
	/*
	void checkTiming () {
		if (scores [scoreIndex] [beatIndex] > 0) {

			if (timeStompForBar / 64 < timeCounterInABeat && timeCounterInABeat < 3 * timeStompForBar / 64) {
				Debug.Log ("Perfect");
				point += 100;
				timingText.text = "Perfect " + (timeCounterInABeat - timeStompForBar / 32);
			} else if (timeStompForBar / 128 < timeCounterInABeat && timeCounterInABeat < 7 * timeStompForBar / 128) {
				Debug.Log ("Great");
				point += 50;
				timingText.text = "Great " + (timeCounterInABeat - timeStompForBar / 32);
			} else {
				Debug.Log ("Good");
				point += 20;
				timingText.text = "Good " + (timeCounterInABeat - timeStompForBar / 32);
			}
			scoreText.text = "Score: " + point;
		}
	}
*/
	void tapBeat() {
		audioSource.PlayOneShot (snare);
		//changeGrapics ();
		//playParticle ();
	}


	void drawScore () {
		
		// スコアを削除 
		foreach ( Transform n in noteWrapperEnemy.transform ) {
			GameObject.Destroy(n.gameObject);
		}

		foreach ( Transform n in noteWrapperPlayer.transform ) {
			GameObject.Destroy(n.gameObject);
		}

		for (int i = 0; i < scores [scoreIndex].Length; i++) {
			if (scores [scoreIndex] [i] == 0) {

				Instantiate (noteRest, noteWrapperEnemy.transform);

			} else if (scores [scoreIndex] [i] == 1) {

				Instantiate (noteNormal, noteWrapperEnemy.transform);
				
			} else if (scores [scoreIndex] [i] == 2) {

				Instantiate (noteLoud, noteWrapperEnemy.transform);

			}
		}

		foreach ( Transform n in noteWrapperEnemy.transform ) {
			n.GetComponent<RectTransform> ().localScale = Vector3.one;
		}
	}


	void calcPositionForIcon () {

		posIconMoveX = noteWrapperEnemy.transform.GetChild (0).transform.position.x;
		float intervalIconMoveX = noteWrapperEnemy.transform.GetChild (1).transform.position.x - posIconMoveX;

		posIconMoveX -= intervalIconMoveX / (32 / scores [scoreIndex].Length);
		posIconMoveToX = posIconMoveX + scores [scoreIndex].Length * intervalIconMoveX;
	}
}

