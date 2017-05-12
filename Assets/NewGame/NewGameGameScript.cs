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
	float BPM;

	AudioSource baseBeatAS = new AudioSource ();
	public AudioClip baseBeatAC;

	GameObject player;
	GameObject ground;


	void Start () {
		Application.targetFrameRate = 60;
		BPM = 104f;

		newGameState = NewGameState.Idle;

		player = GameObject.FindGameObjectWithTag ("Player");
		ground = GameObject.FindGameObjectWithTag ("Ground");

		GameObject child = new GameObject("AudioPlayer");
		child.transform.parent = gameObject.transform;
		baseBeatAS = child.AddComponent<AudioSource>();
		baseBeatAS.clip = baseBeatAC;

		resetSong ();
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)||Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began ) {
			if (newGameState == NewGameState.Idle) {
				newGameState = NewGameState.Play;
			} else if (newGameState == NewGameState.Play) {
				checkTiming ();
			}
		}
		// 4拍子の各1拍
		if (newGameState == NewGameState.Play) {
			eachABar ();
			eachQuarterBeat ();
			each16Beat ();
			timeCounterInASong += Time.deltaTime;
		}
	}

	// 1小節ごとに1回呼ぶ
	void eachABar () {
		if (timeCounterInASong >= timeStompForBar) {
			timeStompForBar += timePerQuarterBeat * 4;
			if (baseBeatAS.isPlaying) {
				baseBeatAS.Stop ();
			}
			baseBeatAS.PlayScheduled (AudioSettings.dspTime + timePerQuarterBeat / 8);
		}
	}

	// 4拍子の各1拍
	void eachQuarterBeat () {
		if (timeCounterInASong >= timeStompForQuarterBeat) {
			timeStompForQuarterBeat += timePerQuarterBeat;

//			player.GetComponent<NewGamePlayer> ().PlayBaseRhythm ();
			ground.GetComponent<NewGameGround> ().ChangeColorWhite ();
		}
	}

	void each16Beat () {
		
	}

	void checkTiming () {
		float errorDuration = timePerQuarterBeat / 32; // 大きくすると判定が厳しくなる
		if (timeStompForQuarterBeat - timePerQuarterBeat + errorDuration < timeCounterInASong && timeCounterInASong < timeStompForQuarterBeat - timePerQuarterBeat + timePerQuarterBeat / 4 - errorDuration) {

			player.GetComponent<NewGamePlayer> ().TapOnTime (1);
		} else if (timeStompForQuarterBeat - timePerQuarterBeat * 0.75 + errorDuration < timeCounterInASong && timeCounterInASong < timeStompForQuarterBeat - timePerQuarterBeat * 0.75 + timePerQuarterBeat / 4 - errorDuration) {

			player.GetComponent<NewGamePlayer> ().TapOnTime (2);
		} else if (timeStompForQuarterBeat - timePerQuarterBeat * 0.5 + errorDuration < timeCounterInASong && timeCounterInASong < timeStompForQuarterBeat - timePerQuarterBeat * 0.5 + timePerQuarterBeat / 4 - errorDuration) {

			player.GetComponent<NewGamePlayer> ().TapOnTime (3);
		} else if (timeStompForQuarterBeat - timePerQuarterBeat * 0.25 + errorDuration < timeCounterInASong && timeCounterInASong < timeStompForQuarterBeat - timePerQuarterBeat * 0.25 + timePerQuarterBeat / 4 - errorDuration) {

			player.GetComponent<NewGamePlayer> ().TapOnTime (4);
		} else {

			player.GetComponent<NewGamePlayer> ().TapOnBadTime ();
		}
	}

	public void resetSong () {

		timePerQuarterBeat = 60f / BPM;
		timeStompForQuarterBeat = 0;
		timeCounterInASong = 0;
		timeStompForBar = 0;

		player.GetComponent<NewGamePlayer> ().reset ();

		newGameState = NewGameState.Idle;
	}
}

