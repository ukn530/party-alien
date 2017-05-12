using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGamePlayer : MonoBehaviour {

	public float upForce;
	public float downForce;
	public float speed;
	public float yPos = 0.5f;
	int numOfJump;

	Animator animator;

	private AudioSource[] audioSources = new AudioSource[5];
	public AudioClip can;
	public AudioClip clap;
	public AudioClip perfect;
	public AudioClip great;
	public AudioClip good;

	Vector3 firstPos;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator> ();
		animator.speed = 104f / 120f;
		yPos = 0.5f;

		// 音の設定
		for (int i = 0; i < audioSources.Length; i++) {
			GameObject child = new GameObject("AudioPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
		}

		audioSources [0].clip = clap;
		audioSources [1].clip = can;
		audioSources [2].clip = perfect;
		audioSources [3].clip = great;
		audioSources [4].clip = good;

		firstPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = new Vector3 (transform.position.x, yPos, 0);

		if (yPos != 0.5f) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
	}

	public void TapOnTime(int noteNum) {
		
		if (noteNum == 1) {
			audioSources [0].Play ();
			jump ();
		} else if (noteNum == 2) {
			audioSources [3].Play ();
			animator.SetTrigger ("RotateRight");
		} else if (noteNum == 3) {
			audioSources [2].Play ();
			animator.SetTrigger ("RotateForward");
		} else if (noteNum == 4) {
			audioSources [4].Play ();
			animator.SetTrigger ("RotateLeft");
		}
	}

	public void TapOnBadTime () {
		Debug.Log ("bad time");
		audioSources [1].Play ();
		animator.SetTrigger ("ChangeColorToRed");
	}

	public void PlayBaseRhythm() {

		transform.localScale = new Vector3 (1.1f, 1.0f, 1.1f);
		Hashtable animationBaseRhythm = new Hashtable ();
		animationBaseRhythm.Add("scale", new Vector3(1f, 1f, 1f));
		animationBaseRhythm.Add("time", 0.1);
		animationBaseRhythm.Add("easetype", "easeOutBounce");
		iTween.ScaleTo(gameObject, animationBaseRhythm);

	}

	void jump () {
		animator.SetTrigger ("Jump");
	}

	public void reset () {
		transform.position = firstPos;
	}
}
