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
		Idle,
		Stop
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
		new int[] {16},
		new int[] {4,315,0,225,180,0,270,360,0}, 
		new int[] {4,180,0,270,0,360,90,360,90},
		new int[] {4,135,0,225,0,0,315,45,0},
		new int[] {4,270,0,180,0,0,270,360,0},
		new int[] {4,90,0,360,0,0,90,180,0},
		new int[] {8},
		new int[] {2,1,0,1,1},
		new int[] {2,1,0,1,1},
		new int[] {2,1,0,1,1,1,0,0,0},
		new int[] {2,1,0,1,1,1,0,0,0},
		new int[] {8},
		new int[] {4,1,0,0,1,1,0,0,1,1,0,1,1,1,0,0,0},
		new int[] {2,1,0,1,1,1,0,0,0},
		new int[] {4},
		new int[] {4,1,0,0,1,1,0,0,1,1,0,1,0,1,0,0,0},
		new int[] {2,1,1,1,0},
		new int[] {12},
		new int[] {4,0,0,1,1,1,0,0,1,0,1,0,1,1,0,0,0},
		new int[] {1,1,0,0,0},
		new int[] {1,1,0,0,0},
		new int[] {4,0,0,1,1,1,0,0,1,0,1,0,1,1,0,0,0},
		new int[] {1,1,0,0,0},
		new int[] {1,1,0,0,0},
		new int[] {4,1,0,1,0,1,0,1,0},
		new int[] {16},
		new int[] {4,1,0,1,0,1,1,1,0},
		new int[] {4,1,0,1,0,1,0,1,0,1,0,1,1,1,0,0,0},
		new int[] {4,1,0,1,0,1,1,1,0},
		new int[] {4,1,0,1,0,1,0,1,0,1,0,1,1,1,0,0,0},
		new int[] {2,1,1,1,0},
		new int[] {4,0,0,1,1,1,0,0,1,0,1,0,1,1,0,1,0},
		new int[] {8}
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
	int noteIndex;
	float timeFromLastTap;

	public GameObject noteWrapperEnemy;
	public GameObject noteWrapperPlayer;
	public GameObject noteRest;
	public GameObject noteNormal;
	public GameObject noteLoud;
	public GameObject noteBlank;
	public GameObject noteCircle;
	public GameObject iconEnemy;
	public GameObject iconPlayer;
	public GameObject score;

	public GameObject player;
	public GameObject enemy;
	public Text fpsText;

	private AudioSource[] audioSources = new AudioSource[6];
	public AudioClip snare;
	public AudioClip kick;
	public AudioClip perfect;
	public AudioClip great;
	public AudioClip good;

	public AudioClip bgm;

	float[,] motionAngleAndDistance;

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
	private static extern void setupSession(IntPtr audioInput);

	[DllImport("__Internal")]
	private static extern float getAudioVolume(IntPtr audioInput);

	[DllImport("__Internal")]
	private static extern void stopVolume(IntPtr audioInput);

	[DllImport("__Internal")]
	private static extern void cfRelease(IntPtr audioInput);


	private static void setupAudioSession() {
		audioInput = audioInputInit ();
		setupSession (audioInput);
	}

	private static void setupMic() {
		setupAudio (audioInput);
	}

	private static float getAudioVolume() {
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
		for (int i = 0; i < 6; i++) {
			GameObject child = new GameObject("AudioPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
		}

		audioSources [0].clip = kick;
		audioSources [1].clip = snare;
		audioSources [2].clip = perfect;
		audioSources [3].clip = great;
		audioSources [4].clip = good;

		audioSources [5].clip = bgm;

		BPM = 120f;

		state = State.Stop;

		init ();
	}

	void init () {

		point = 0;

		// リズムの設定
		turnIndex = 0;
		beatIndex = 1;
		scoreIndex = -1;

		timePerQuarterBeat = 60f / BPM;
		timeStompForBar = 0;
		timeStompForQuarterBeat = 0;
		timeStompForBeat = 0;
		timeCounterInASong = 0;

		iconEnemy.transform.position = new Vector3(0, iconEnemy.transform.position.y);
		iconPlayer.transform.position = new Vector3(0, iconPlayer.transform.position.y);

		timeFromLastTap = timePerQuarterBeat / 5;

		#if UNITY_EDITOR
		#elif UNITY_IOS
		setupAudioSession ();
		#endif
	}
	
	// Update is called once per frame
	void Update () {

		if (state == State.Enemy) {

			iconEnemy.transform.Translate (Vector3.right * Time.deltaTime * ((posIconMoveToX - posIconMoveX) / 2) * (BPM / 120f));

		} else if (state == State.Player) {

			iconPlayer.transform.Translate (Vector3.right * Time.deltaTime * ((posIconMoveToX - posIconMoveX) / 2) * (BPM / 120f));
			onTap ();
		} else if (state == State.Idle) {
			onTap ();
		}

		if (state != State.Stop) {
			eachABar ();
			eachQuarterBeat ();
			eachABeat ();

			timeCounterInASong += Time.deltaTime;
		}


		fpsText.text = 1f / Time.deltaTime + "fps";
	}


	void eachABar () {
		// 1ターンごとに1回呼ぶ
		if (timeCounterInASong >= timeStompForBar) {
			// 休憩中の場合
			if (scores [scoreIndex + 1].Length == 1 && state != State.Enemy) {
				state = State.Idle;
				scoreIndex++;
				score.SetActive (false);

				turnIndex++;

				iconEnemy.transform.position = new Vector3 (posIconMoveX, iconEnemy.transform.position.y);

				iconPlayer.SetActive (false);
				iconEnemy.SetActive (false);
			// バトル中の場合
			} else {
				score.SetActive (true);
				// 敵のターン中の処理
				if (state == State.Player || state == State.Idle) {
					state = State.Enemy;
					if (turnIndex == scores.Length) {
						stopMusic ();
						return;
					}
					noteIndex = 0;
					scoreIndex++;
					drawScore ();
					defineCharactersMotionInATurn ();

					GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");
					foreach (GameObject marker in markers) {
						Destroy (marker);
					}

					turnIndex++;

					iconEnemy.transform.position = new Vector3 (posIconMoveX, iconEnemy.transform.position.y);

					iconPlayer.SetActive (false);
					iconEnemy.SetActive (true);

				// プレイヤーのターン中の処理
				} else if (state == State.Enemy) {
					state = State.Player;

					noteIndex = 0;
					iconPlayer.transform.position = new Vector3 (posIconMoveX, iconPlayer.transform.position.y);

					iconEnemy.SetActive (false);
					iconPlayer.SetActive (true);
				}
			}
			beatIndex = 0;
			timeStompForBar += timePerQuarterBeat * scores[scoreIndex][0];
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
			if (state == State.Idle) {
				timeStompForBeat += (timePerQuarterBeat * 4) / 8;
			} else {
				timeStompForBeat += (timePerQuarterBeat * scores [scoreIndex] [0]) / (scores [scoreIndex].Length - 1);
			}

			beatIndex++;

			if (state == State.Enemy) {
				if (scores[scoreIndex][beatIndex] > 0) {
					
					audioSources [1].PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);
					float[] angleAndDistance = { motionAngleAndDistance [noteIndex, 0], motionAngleAndDistance [noteIndex, 1] };
					enemy.GetComponent<playerBehavier> ().translateCharacter (angleAndDistance);
					enemy.GetComponent<playerBehavier> ().playParticle ();
//					enemy.GetComponent<playerBehavier> ().drawMarker ();
					noteIndex++;
				}
			}

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

			if (timeFromLastTap > timePerQuarterBeat / 8) {
				timeFromLastTap = 0;
				Instantiate(noteCircle, iconPlayer.transform.position, Quaternion.identity, noteWrapperPlayer.transform);
				if (state == State.Player) {
					checkTiming ();
				}
				tapBeat ();
			}
		}
	}

	void checkTiming () {
		if (scores [scoreIndex] [beatIndex] > 0) {
			float startPoint = timeStompForBeat - (timePerQuarterBeat * scores [scoreIndex] [0]) / scores [scoreIndex].Length;
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
			player.GetComponent<playerBehavier> ().drawRate (rate);
		}
	}

	void tapBeat() {
		audioSources [1].Play ();
		float[] angleAndDistance = { motionAngleAndDistance [noteIndex, 0], motionAngleAndDistance [noteIndex, 1] };
		player.GetComponent<playerBehavier> ().translateCharacter (angleAndDistance);
		player.GetComponent<playerBehavier> ().playParticle ();
		noteIndex++;
		if (noteIndex == motionAngleAndDistance.GetLength (0)) {
			noteIndex = 0;
		}
//		player.GetComponent<playerBehavier> ().drawMarker ();
	}

	void defineCharactersMotionInATurn () {
		// 角度と距離の多次元配列を作るために、動く回数を求める
		int beatCounter = 0;
		for (int i = 1; i < scores[scoreIndex].Length; i++) {
			if (scores [scoreIndex] [i] > 0) {
				beatCounter++;
			}
		}
		// 動く回数 x 2(角度と距離)の多次元配列を定義
		motionAngleAndDistance = new float[beatCounter, 2];

		beatCounter = 0;
		for (int i = 1; i < scores[scoreIndex].Length; i++) {
			if (scores [scoreIndex] [i] > 0) {

				// 各動きの角度を入れる
				motionAngleAndDistance [beatCounter, 0] = scores[scoreIndex][i];

				// 各動きの距離を入れる
				float distance = 0;
				bool flag = true;
				for (int j = i+1; j < scores[scoreIndex].Length; j++) {
					if (scores [scoreIndex] [j] > 0 && flag){
						distance = j - i;
						flag = false;
					} else if (j == scores [scoreIndex].Length-1 && flag) {
						distance = j - i + 1;
						flag = false;
					}
				}

				if (i == scores [scoreIndex].Length-1) {
					distance = 1;
				}

				distance *= (float)scores [scoreIndex] [0] / (float)(scores [scoreIndex].Length-1);

				motionAngleAndDistance [beatCounter, 1] = distance;
				beatCounter++;
			}
		}
	}

	void drawScore () {
		
		// スコアを削除 
		foreach ( Transform n in noteWrapperEnemy.transform ) {
			GameObject.Destroy(n.gameObject);
		}

		foreach ( Transform n in noteWrapperPlayer.transform ) {
			GameObject.Destroy(n.gameObject);
		}

		for (int i = 1; i < scores [scoreIndex].Length; i++) {
			if (scores [scoreIndex] [i] == 0) {

				Instantiate (noteRest, noteWrapperEnemy.transform);

			} else if (scores [scoreIndex] [i] > 0) {

				Instantiate (noteNormal, noteWrapperEnemy.transform);
				
			}
		}


		if (scores [scoreIndex] [0] > 0) {
			for (int i = 0; i < (scores[scoreIndex].Length - 1) * 3; i++) {
				Instantiate (noteBlank, noteWrapperEnemy.transform);
			}
		}

		StartCoroutine ("waitForEndOfThisFrame");

		// なぜかscaleがでかくなるのでいれたけど消したい
		foreach ( Transform n in noteWrapperEnemy.transform ) {
			n.GetComponent<RectTransform> ().localScale = Vector3.one;
		}
	}

	private IEnumerator waitForEndOfThisFrame() {
		yield return new WaitForEndOfFrame();
		calcPositionForIcon ();
	}

	void calcPositionForIcon () {
		posIconMoveX = noteWrapperEnemy.transform.GetChild (0).transform.position.x;
		float intervalIconMoveX = noteWrapperEnemy.transform.GetChild (1).transform.position.x - posIconMoveX;

		posIconMoveX -= intervalIconMoveX / (32 / noteWrapperEnemy.transform.childCount);
		posIconMoveToX = posIconMoveX + noteWrapperEnemy.transform.childCount * intervalIconMoveX;
	}


	public void onClick () {
		audioSources [5].PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);

		player.GetComponent<playerBehavier> ().playStartAnimation ();
		enemy.GetComponent<playerBehavier> ().playStartAnimation ();
		Camera.main.GetComponent<CameraBehavior> ().playStartAnimation ();

		init ();
		state = State.Idle;
	}

	public void stopMusic() {

		state = State.Stop;
		audioSources [5].Stop ();
		return;
	}

	public void onTapPlayPauseButton (GameObject button) {
		GameObject iconPause = button.transform.FindChild("IconPause").gameObject;
		GameObject iconPlay = button.transform.FindChild("IconPlay").gameObject;
		if (iconPlay.activeSelf) {
			iconPlay.SetActive (false);
			iconPause.SetActive (true);

			audioSources [5].PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);

			player.GetComponent<playerBehavier> ().playStartAnimation ();
			enemy.GetComponent<playerBehavier> ().playStartAnimation ();
			Camera.main.GetComponent<CameraBehavior> ().playStartAnimation ();

			init ();
			state = State.Idle;
		} else {
			iconPlay.SetActive (true);
			iconPause.SetActive (false);

			state = State.Stop;
			audioSources [5].Stop ();
		}
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

