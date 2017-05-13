using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class NewGameGameScript : MonoBehaviour {

	public enum NewGameState {
		Play,
		Idle
	}

	public static NewGameState newGameState;

	float timeCounterInASong;
	float timeStompForQuarterBeat;
	float timeStompForBar;
	float timePerQuarterBeat;
	float timePerBar;
	float timeCounterFromLastTap;
	public static float BPM;

	AudioSource baseBeatAS = new AudioSource ();
	public AudioClip baseBeatAC;

	GameObject player;
	GameObject[] grounds;
	bool[] notesMemory = new bool[16];

	bool isDownBody = false;


	void Start () {
		Application.targetFrameRate = 60;
		BPM = 120f;

		newGameState = NewGameState.Idle;

		player = GameObject.FindGameObjectWithTag ("Player");
		grounds = GameObject.FindGameObjectsWithTag ("Ground");

		GameObject child = new GameObject("AudioPlayer");
		child.transform.parent = gameObject.transform;
		baseBeatAS = child.AddComponent<AudioSource>();
		baseBeatAS.clip = baseBeatAC;
		baseBeatAS.volume = 0.6f;
		baseBeatAS.loop = true;

		for (int i = 0; i < notesMemory.Length; i++) {
			notesMemory [i] = false;
		}

		resetSong ();
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)||Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began ) {
			onTapListner ();
		}

		if (newGameState == NewGameState.Play) {
			// 最後のタップから2拍何もなかったらしゃがむ
			if (timeCounterFromLastTap > timePerQuarterBeat * 2 && !isDownBody) {
				isDownBody = true;
				restForQuarterBeat ();

			}
			timeCounterFromLastTap += Time.deltaTime;
		}

		// 4拍子の各1拍
		if (newGameState == NewGameState.Play) {

			if (timeCounterInASong >= timeStompForBar) {
				timeStompForBar += timePerQuarterBeat * 4;
				eachABar ();
			}

			if (timeCounterInASong >= timeStompForQuarterBeat) {
				timeStompForQuarterBeat += timePerQuarterBeat;
				eachQuarterBeat ();
			}
			each16Beat ();
			timeCounterInASong += Time.deltaTime;
		}
	}

	void onTapListner () {
		if (newGameState == NewGameState.Idle) {
			newGameState = NewGameState.Play;

			if (baseBeatAS.isPlaying) {
				baseBeatAS.Stop ();
			}
			baseBeatAS.PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);

		} else if (newGameState == NewGameState.Play) {
			checkTiming ();
			timeCounterFromLastTap = 0;
			isDownBody = false;
		}
	}

	// 1小節ごとに1回呼ぶ
	void eachABar () {

		for (int i = 0; i < notesMemory.Length; i++) {
			notesMemory [i] = false;
		}
	}

	// 4拍子の各1拍
	void eachQuarterBeat () {

		StartCoroutine(LateStart(0.1F));
	}

	IEnumerator LateStart (float time) {
		yield return new WaitForSeconds (time);

		player.GetComponent<Animator> ().SetTrigger ("BaseRhythm");
		for (int i = 0; i < grounds.Length; i++) {
			grounds[i].GetComponent<NewGameGround> ().ChangeColorWhite ();
		}
	}

	void each16Beat () {
		
	}

	void restForQuarterBeat () {
		player.GetComponent<NewGamePlayer> ().DownBody ();
	}

	void checkTiming () {
		float errorDuration = timePerQuarterBeat/32; // 大きくすると判定が厳しくなる
		bool onTime = false;
		for (int i = 0; i < notesMemory.Length; i++) {
			if (timeStompForBar - timePerBar * (notesMemory.Length-i)/notesMemory.Length + errorDuration <= timeCounterInASong && timeCounterInASong < timeStompForBar - timePerBar * (notesMemory.Length-i-1)/notesMemory.Length - errorDuration) {
				notesMemory [i] = true;
				onTime = true;
			}
		}
		player.GetComponent<NewGamePlayer> ().TapOnTime (notesMemory);
		if (!onTime) {
			player.GetComponent<NewGamePlayer> ().TapOnBadTime ();
		}
	}

	public void resetSong () {

		timePerQuarterBeat = 60f / BPM;
		timePerBar = 240f / BPM;
		timeStompForQuarterBeat = 0;
		timeCounterInASong = 0;
		timeStompForBar = 0;

		player.GetComponent<NewGamePlayer> ().reset ();

		newGameState = NewGameState.Idle;
	}
}

