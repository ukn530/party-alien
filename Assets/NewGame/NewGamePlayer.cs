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
	public AudioClip kick;
	public AudioClip snare;
	public AudioClip hihat;
	public GameObject leftWing;
	public GameObject rightWing;
	public GameObject topWing;
	public GameObject bottomWing;
	public GameObject CircleEffect;

	Vector3 firstPos;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator> ();
		animator.speed = 120f / 120f;
		yPos = 0.5f;

		// 音の設定
		for (int i = 0; i < audioSources.Length; i++) {
			GameObject child = new GameObject("AudioPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
//			audioSources [i].volume = 1.5f;
		}

		audioSources [0].clip = clap;
		audioSources [1].clip = can;
		audioSources [2].clip = kick;
		audioSources [3].clip = snare;
		audioSources [4].clip = hihat;

		firstPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, yPos, 0);

		if (yPos != 0.5f && yPos != 0.2f) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
	}

	public void TapOnTime(bool[] notesMemory) {

		topWing.SetActive (false);
		bottomWing.SetActive (false);
		leftWing.SetActive (false);
		rightWing.SetActive (false);

		if (yPos == 0.2f) {
			animator.SetTrigger ("JumpHigh");
			GameObject ce = Instantiate (CircleEffect);
			ce.transform.position = transform.position;
			Destroy (ce, 1);
		} else if (yPos <= 0.5f) {
			animator.SetTrigger ("Jump");
			GameObject ce = Instantiate (CircleEffect);
			ce.transform.position = transform.position;
			Destroy (ce, 1);
		}

		int noteNum = 0;
		for (int i = 0; i < notesMemory.Length; i++) {
			if (notesMemory [i] == true) {
				noteNum = i;
			}
		}

		if (noteNum == 0 || noteNum == 8 ) {
			audioSources [2].Play ();
		}

		if (noteNum == 4|| noteNum == 12) {
			if (notesMemory [2] || notesMemory [10]) {
				audioSources [3].Play ();
				animator.SetTrigger ("RotateForward");
				topWing.SetActive (true);
				bottomWing.SetActive (true);
			} else {
				audioSources [2].Play ();
			}
		}

		if (noteNum == 2 || noteNum == 10) {
			audioSources [4].Play ();
			animator.SetTrigger ("RotateRight");
			leftWing.SetActive (true);
			rightWing.SetActive (true);

		}

		if (noteNum == 6 || noteNum == 14) {
			audioSources [4].Play ();
			animator.SetTrigger ("RotateLeft");
			leftWing.SetActive (true);
			rightWing.SetActive (true);
		}

		if (noteNum == 3 || noteNum == 11) {
			audioSources [1].Play ();
		}
	}

	public void TapOnBadTime () {
		animator.SetTrigger ("ChangeColorToRed");
	}

	public void DownBody () {
		animator.SetTrigger ("DownBody");
		
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
