using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class GameScript : MonoBehaviour {

	public enum State {
		Enemy,
		Player,
		Idle
	}

	public enum Rate {
		Fail,
		Good,
		Great,
		Perfect
	}

	public static State state;
	public Rate rate;


	int[][] scores = new int[][]{
		new int[] {0,0,1,1,0,0,1,0},
		new int[] {0,1,0,1,0,1,1,1}, 
		new int[] {0,1,0,1,1,1,0,1},
		new int[] {1,0,1,0,0,1,0,1},
		new int[] {0,0,1,0,0,0,1,1,0,0,1,1,0,0,1,0},
		new int[] {1,0,1,0,1,1,0,1,1,0,1,0,1,1,0,1},
		new int[] {0,1,1,0,1,1,1,0,1,0,1,1},
		new int[] {0,1,0,1,0,1,1,1,0,1,0,1,0,1,1,1}
	};


	float BPM;

	float timeCounterInASong;

	float timePerQuarterBeat;
	float timeStompForBar;
	float timeStompForQuarterBeat;
	float timeStompForBeat;

	int turnIndex;
	int beatIndex;
	int scoreIndex;
	float timeFromLastTap;

	public GameObject noteWrapperEnemy;
	public GameObject noteWrapperPlayer;
	public GameObject noteRest;
	public GameObject noteNormal;
	public GameObject noteLoud;
	public GameObject noteCircle;
	public GameObject iconEnemy;
	public GameObject iconPlayer;
	public GameObject score;

	public GameObject player;
	public GameObject enemy;

	public Text scoreText;
	public Text timingText;
	public Text fpsText;
	public Text bpmText;
	public GameObject resultBoard;
	public Text resultScoreText;
	public GameObject buttonRestart;

	private AudioSource[] audioSources = new AudioSource[5];
	public AudioClip snare;
	public AudioClip kick;
	public AudioClip perfect;
	public AudioClip great;
	public AudioClip good;


	float posIconMoveX;
	float posIconMoveToX;


	int paddingOfScore;
	int point;

	bool isActiveMic = false;

	#if UNITY_EDITOR
	#elif UNITY_IOS

	static IntPtr audioInput;

	[DllImport("__Internal")]
	private static extern IntPtr audioInputInit();

	[DllImport("__Internal")]
	private static extern void setupAudio(IntPtr audioInput);

	[DllImport("__Internal")]
	private static extern float getAudioVolume(IntPtr audioInput);

	[DllImport("__Internal")]
	private static extern void stopVolume(IntPtr audioInput);

	[DllImport("__Internal")]
	private static extern void cfRelease(IntPtr audioInput);

	private static void setupMic() {
		Debug.Log ("defined setupMic");
		audioInput = audioInputInit ();
		setupAudio (audioInput);
	}

	private static float getAudioVolume() {

		Debug.Log ("defined getAudioVolume");
		return getAudioVolume (audioInput);
	}

	private static void stopMic() {
		stopVolume (audioInput);
		cfRelease (audioInput);
	}

	#endif

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		// 音の設定
		for (int i = 0; i < 5; i++) {
			GameObject child = new GameObject("AudioPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
		}

		audioSources [0].clip = kick;
		audioSources [1].clip = snare;
		audioSources [2].clip = perfect;
		audioSources [3].clip = great;
		audioSources [4].clip = good;

		BPM = 112.5f;

		state = State.Idle;

		// スコアの表示
		drawScore ();
		calcPositionForIcon ();

		init ();
	}

	void init () {

		point = 0;

		// リズムの設定
		turnIndex = 0;
		beatIndex = 0;
		scoreIndex = -1;

		timePerQuarterBeat = 60f / BPM;
		timeStompForBar = 0;
		timeStompForQuarterBeat = 0;
		timeStompForBeat = 0;
		timeCounterInASong = 0;

		iconEnemy.transform.position = new Vector3(0, iconEnemy.transform.position.y);
		iconPlayer.transform.position = new Vector3(0, iconPlayer.transform.position.y);

		scoreText.text = "Score: " + 0;

		timeFromLastTap = timePerQuarterBeat / 5;
	}
	
	// Update is called once per frame
	void Update () {

		if (state == State.Enemy) {

			iconEnemy.transform.Translate (Vector3.right * Time.deltaTime * ((posIconMoveToX - posIconMoveX) / 2) * (BPM / 120f));

		} else if (state == State.Player) {

			iconPlayer.transform.Translate (Vector3.right * Time.deltaTime * ((posIconMoveToX - posIconMoveX) / 2) * (BPM / 120f));
			onTap ();
		}

		if (state != State.Idle) {
			eachABar ();
			eachQuarterBeat ();
			eachABeat ();

			timeCounterInASong += Time.deltaTime;
		}


		fpsText.text = 1f / Time.deltaTime + "fps";
	}


	void eachABar () {
		// 1小節ごとに1回呼ぶ
		if (timeCounterInASong >= timeStompForBar) {

			timeStompForBar += timePerQuarterBeat * 4f;
			
			if (state == State.Player || state == State.Idle) {
				state = State.Enemy;

				if (turnIndex == 8) {
					stopMusic ();
				}

				turnIndex++;
				timingText.text = "Turn: " + turnIndex + "/8";
				scoreIndex++;
//				scoreIndex = Random.Range(0, scores.Length);
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

			audioSources [0].PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);
			player.GetComponent<playerBehavier> ().playBaseRhythm ();
			enemy.GetComponent<playerBehavier> ().playBaseRhythm ();
		}
	}


	void eachABeat () {

		// 1音符ごと
		if (timeCounterInASong > timeStompForBeat) {
			timeStompForBeat += (timePerQuarterBeat * 4) / scores[scoreIndex].Length;

			if (state == State.Enemy) {
				if (scores[scoreIndex][beatIndex] > 0) {
					
					audioSources [1].PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);
					enemy.GetComponent<playerBehavier> ().changeGrapics ();
					enemy.GetComponent<playerBehavier> ().playParticle ();
				}
			}

			beatIndex++;
		}
	}


	void onTap () {
		timeFromLastTap += Time.deltaTime;
		float vol = 0;

		if (isActiveMic) {
			#if UNITY_EDITOR
			#elif UNITY_IOS
			vol = getAudioVolume();
			#endif
		}
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space) || vol > 0.4) {

			if (timeFromLastTap > timePerQuarterBeat / 5) {
				timeFromLastTap = 0;
				Instantiate(noteCircle, iconPlayer.transform.position, Quaternion.identity, noteWrapperPlayer.transform);
				checkTiming ();
				tapBeat ();
			}
		}
	}

	void checkTiming () {

		if (scores [scoreIndex] [beatIndex-1] > 0) {
			float startPoint = timeStompForBeat - (timePerQuarterBeat * 4) / scores [scoreIndex].Length;
			if (startPoint + timePerQuarterBeat / 16 < timeCounterInASong && timeCounterInASong < startPoint + 3 * timePerQuarterBeat / 16) {
				rate = Rate.Perfect;
				point += 100;
				audioSources [2].Play ();
			} else if (startPoint + timePerQuarterBeat / 32 < timeCounterInASong && timeCounterInASong < startPoint + 7 * timePerQuarterBeat / 32) {
				rate = Rate.Great;
				point += 50;
				audioSources [3].Play ();
			} else {
				rate = Rate.Good;
				point += 20;
				audioSources [4].Play ();
			}
			scoreText.text = "Score: " + point;
			resultScoreText.text = "" + point;
			player.GetComponent<playerBehavier> ().drawRate (rate);
		}
	}

	void tapBeat() {
		audioSources [1].Play ();
		player.GetComponent<playerBehavier> ().changeGrapics ();
		player.GetComponent<playerBehavier> ().playParticle ();
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


	public void onClick () {
		#if UNITY_EDITOR
		Debug.Log("editor");
		#elif UNITY_IOS
		Debug.Log("ios");
		#endif
		
		resultBoard.SetActive (false);
		state = State.Player;
		init ();
	}

	public void onSlide (float value) {
		
		BPM = value;
		bpmText.text = "BPM: " + value;
	}

	public void stopMusic() {

		state = State.Idle;
		resultBoard.SetActive (true);
		return;
	}

	public void onValueChanged (bool value) {
		isActiveMic = value;
		#if UNITY_EDITOR
		#elif UNITY_IOS
		if (isActiveMic) {

			setupMic ();
		} else {

			stopMic ();
		}
		#endif
	}
}

