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
	float timeCounterInABeat;
	int beatCounter = 0;
	float BPM = 120;
	float timePerBar;

	public Text fpsText;

	public static int[][] scores;
	public static float beatTime; // 命名はtimePerBeatとかの方が良い
	public static int scoreNum; // indexOfScoreとか

	public GameObject noteWrapperEnemy;
	public GameObject noteWrapperPlayer;
	public GameObject noteRest;
	public GameObject noteNormal;
	public GameObject noteLoud;
	public GameObject noteCircle;
	public GameObject iconEnemy;
	public GameObject iconPlayer;
	public GameObject score;

	AudioSource audioSource;
	private AudioSource[] audioSources = new AudioSource[2];
	public AudioClip snare;
	public AudioClip kick;

	float posIconMoveX;
	float posIconMoveToX;

	int paddingOfScore;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		scores = new int[][]{
			new int[] {0,1,0,1,0,1,0,1},
//			new int[] {0,0,1,1,0,0,1,0}, 
//			new int[] {0,0,1,1,0,0,1,0,0,0,1,1,0,0,1,0}, 
//			new int[] {0,1,0,1,0,1,1,1}, 
//			new int[] {1,0,1,0,1,1,0,1,1,0,1,0,1,1,0,1},
//			new int[] {1,0,1,0,0,1,0,1},
//			new int[] {0,1,0,1,0,1,1,1,0,1,0,1,0,1,1,1},
//			new int[] {0,1,0,1,1,1,0,1},
//			new int[] {0,0,1,0,0,0,1,1,0,0,1,1,0,0,1,0}
		};

		int i = 0;
		while (i < 2) {
			GameObject child = new GameObject("AudioPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
			i++;
		}

		audioSources [0].clip = kick;
		audioSources [1].clip = snare;

		timeCounter = 0;
		beatTimeCounter = 0;


		scoreNum = 0;
		timePerBar = 60 * 4 / BPM;
		beatTime = timePerBar / scores [scoreNum].Length;

		drawScore ();

		calcPositionForIcon ();

		iconEnemy.transform.position = new Vector3(posIconMoveX, iconEnemy.transform.position.y);
		iconPlayer.transform.position = new Vector3(posIconMoveX, iconPlayer.transform.position.y);

		audioSource = GetComponent<AudioSource> ();

		Debug.Log(state);
	}
	
	// Update is called once per frame
	void Update () {

		if (state == State.Enemy) {

			iconEnemy.transform.Translate (Vector3.right * Time.deltaTime * (posIconMoveToX - posIconMoveX) / 2);

		} else if (state == State.Player) {

			iconPlayer.transform.Translate (Vector3.right * Time.deltaTime * (posIconMoveToX - posIconMoveX) / 2);
			onTap ();
		}

		fpsText.text = 1f / Time.deltaTime + "fps";


		eachABar ();
		eachABeat ();
		eachANote ();
	}

	void FixedUpdate () {
	}

	void onTap () {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) {
			
			Instantiate(noteCircle, iconPlayer.transform.position, Quaternion.identity, noteWrapperPlayer.transform);
			tapBeat ();


		}
	}


	void eachABar () {

		// 1小節ごとに呼ぶ
		if (timeCounter > timePerBar) {
			if (state == State.Player) {
				scoreNum = Random.Range(0, scores.Length);
				beatTime = timePerBar / scores[scoreNum].Length;
				state = State.Enemy;
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
			timeCounter -= timePerBar;
		}

		timeCounter += Time.deltaTime;
	}

	void eachABeat () {

		// 1拍ごと
		if (beatTimeCounter >= 0) {
			audioSources [0].PlayScheduled (AudioSettings.dspTime + timePerBar / 32);
			beatTimeCounter -= timePerBar/4;
		}
		beatTimeCounter += Time.deltaTime;
	}


	void eachANote () {

		// 1音符ごと
		if (timeCounterInABeat >= 0) {

			if (state == State.Enemy) {

				if (scores[scoreNum][beatCounter] > 0) {
					audioSources [1].PlayScheduled (AudioSettings.dspTime + timePerBar / 32);
				}
			}

			beatCounter++;

			if (beatCounter == scores[scoreNum].Length) {
				beatCounter = 0;
			}

			timeCounterInABeat -= beatTime;
		}

		timeCounterInABeat += Time.deltaTime;
	}


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

		for (int i = 0; i < scores [scoreNum].Length; i++) {
			if (scores [scoreNum] [i] == 0) {

				Instantiate (noteRest, noteWrapperEnemy.transform);

			} else if (scores [scoreNum] [i] == 1) {

				Instantiate (noteNormal, noteWrapperEnemy.transform);
				
			} else if (scores [scoreNum] [i] == 2) {

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
		posIconMoveX -= intervalIconMoveX / 4; 
		posIconMoveToX = posIconMoveX + scores [scoreNum].Length * intervalIconMoveX;
	}
}
