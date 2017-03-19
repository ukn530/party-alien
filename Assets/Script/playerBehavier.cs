using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVGImporter;
using UnityEngine.UI;

public class playerBehavier : MonoBehaviour {

	private SVGRenderer svgRenderer;
	public GameObject particle;
	public SVGAsset player1, player2, player3, player4, player5, player6;
	public Mesh p1, p2, p3, p4, p5, p6, p7, p8;

	public Text scoreText;
	public Text timingText;

	AudioSource audioSource;
	public AudioClip snare;
	public AudioClip failSound;
	public AudioClip goodSound;
	public AudioClip greatSound;
	public AudioClip perfectSound;


	bool tapped;
	float timeCounterInABeat;
	float timeCounterInABar;
	int beatCounter;
	int point;

	public GameObject textRate;
	public SVGAsset goodRate;
	public SVGAsset greatRate;
	public SVGAsset perfectRate;

	 enum Rate {
		Fail,
		Good,
		Great,
		Perfect,
		Model
	}

	Rate rate;

	Mesh[] meshs;

	// Use this for initialization
	void Start () {
		svgRenderer = gameObject.GetComponent<SVGRenderer> ();

		meshs = new Mesh[] {p1, p2, p3, p4, p5, p6, p7, p8};

		timeCounterInABeat = 0;
		timeCounterInABar = 0.0625f;
		beatCounter = 0;

		point = 0;

		audioSource = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (GameScript.state == GameScript.State.Player && tag == "Player") {
			
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) {
				
				timingText.text = "Fail";
				// 判定
				for (int i = 0; i < GameScript.scores[GameScript.scoreNum].Length; i++) {
					if (GameScript.scores[GameScript.scoreNum][i] > 0) {
						float timing = 0;
						if (i * GameScript.beatTime + 0.046875 < timeCounterInABar && timeCounterInABar < i * GameScript.beatTime + 0.078125) {
							point += 500;
							scoreText.text = "Score: " + point;
							timing = timeCounterInABar - 0.0625f - i * GameScript.beatTime;
							timingText.text = "Perfect " + timing;
							tapBeat (Rate.Perfect);
						} else if (i * GameScript.beatTime + 0.03125 < timeCounterInABar && timeCounterInABar < i * GameScript.beatTime + 0.09375) {
							point += 300;
							scoreText.text = "Score: " + point;
							timing = timeCounterInABar - 0.0625f - i * GameScript.beatTime;
							timingText.text = "Great " + timing;
							tapBeat (Rate.Great);
						} else if (i * GameScript.beatTime < timeCounterInABar && timeCounterInABar < i * GameScript.beatTime + 0.125) {
							point += 100;
							scoreText.text = "Score: " + point;
							timing = timeCounterInABar - 0.0625f - i * GameScript.beatTime;
							timingText.text = "Good " + timing;
							tapBeat (Rate.Good);
						}
					}
				}
				Debug.Log ("tap in Player");
			}

			if (timeCounterInABar >= 2) {
				timeCounterInABar -= 2;
			}

			timeCounterInABar += Time.deltaTime;
		}

	}

	void tapBeat(Rate rate) {
		if (rate == Rate.Model) {
			audioSource.PlayOneShot (snare);
		} else if (rate == Rate.Perfect) {
			audioSource.PlayOneShot (perfectSound);
			drawRate (rate);
		} else if (rate == Rate.Great) {
			audioSource.PlayOneShot (greatSound);
			drawRate (rate);
		} else if (rate == Rate.Good) {
			audioSource.PlayOneShot (goodSound);
			drawRate (rate);
		} else if (rate == Rate.Fail) {
			audioSource.PlayOneShot (failSound);
		}
		changeGrapics ();
		playParticle ();
	}

	void changeGrapics () {
		int randomNum = Random.Range (0,6);
		if (randomNum <= 1 && svgRenderer.vectorGraphics != player1) {
			svgRenderer.vectorGraphics = player1;
		} else if (randomNum <= 2 && svgRenderer.vectorGraphics != player2) {
			svgRenderer.vectorGraphics = player2;
		} else if (randomNum <= 3 && svgRenderer.vectorGraphics != player3) {
			svgRenderer.vectorGraphics = player3;
		} else if (randomNum <= 4 && svgRenderer.vectorGraphics != player4) {
			svgRenderer.vectorGraphics = player4;
		} else if (randomNum <= 5 && svgRenderer.vectorGraphics != player5) {
			svgRenderer.vectorGraphics = player5;
		} else if (randomNum <= 6 && svgRenderer.vectorGraphics != player6) {
			svgRenderer.vectorGraphics = player6;
		} else {
			changeGrapics ();
		}
	}

	void playParticle () {
		for (int i = 0; i < 8; i++) {
			GameObject ptcle = Instantiate(particle);
			ParticleSystemRenderer psr = ptcle.GetComponent<ParticleSystemRenderer>();
			psr.mesh = meshs [i];
			Vector3 pos = transform.position;
			pos.z = -1.0f;
			pos.y += gameObject.GetComponent<MeshRenderer>().bounds.size.y/2;
			ptcle.transform.position = pos;
			ptcle.transform.parent = transform;
			Destroy (ptcle, .5f);
		}
	}

	void drawRate (Rate rate) {
		GameObject rateObject = Instantiate (textRate);
		rateObject.transform.parent = transform;
		rateObject.GetComponent<Animator> ().SetTrigger ("Success");
		if (rate == Rate.Perfect) {
			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = perfectRate;
		} else if (rate == Rate.Great) {
			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = greatRate;
		} else if (rate == Rate.Good) {
			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = goodRate;
		}
		Destroy (rateObject, .5f);
	}
}

