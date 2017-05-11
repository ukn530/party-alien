using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVGImporter;
using UnityEngine.UI;

public class playerBehavier : MonoBehaviour {

	public GameObject particle;
	public Mesh p1, p2, p3, p4, p5, p6, p7, p8;

	public GameObject trailParticle;
	public GameObject effectBomb;

	
	Mesh[] meshs;

	// Use this for initialization
	void Start () {

		meshs = new Mesh[] {p1, p2, p3, p4, p5, p6, p7, p8};
	}

	// Update is called once per frame
	void Update () {
	}

	public void playBaseRhythm () {
		transform.localScale = new Vector3 (1.1f, 1.1f, 1.1f);

		Hashtable animationBaseRhythm = new Hashtable ();
		animationBaseRhythm.Add("scale", new Vector3(1f, 1f, 1f));
		animationBaseRhythm.Add("time", 0.1);
		animationBaseRhythm.Add("easetype", "easeOutBounce");
		iTween.ScaleTo(gameObject, animationBaseRhythm);
	}

	public void playStartAnimation () {
		Hashtable animationGetStartPos = new Hashtable ();
		if (gameObject.name == "Enemy") {
			animationGetStartPos.Add ("position", new Vector3 (17f, 4f, - 2f));
		} else {
			animationGetStartPos.Add ("position", new Vector3 (9f, 4f, - 2f));
		}
		animationGetStartPos.Add("delay", 2);
		animationGetStartPos.Add("time", 4);
		animationGetStartPos.Add("easetype", "easeInOutCubic");
		iTween.MoveTo(gameObject, animationGetStartPos);
	}

	public void translateCharacter (float[] angleAndDistance) {
		float angle = angleAndDistance[0];
		float distance = angleAndDistance[1] * 10;

		float rad = angle * Mathf.Deg2Rad;

		float x = Mathf.Sin(rad) * distance;
		float y = Mathf.Cos(rad) * distance;

		Hashtable animationTranslateToNextPos = new Hashtable ();

		animationTranslateToNextPos.Add ("amount", new Vector3 (x, y, 0));
		animationTranslateToNextPos.Add("time", angleAndDistance[1] * 0.5f);
		animationTranslateToNextPos.Add("easetype", "easeOutCubic");
		iTween.MoveBy (gameObject, animationTranslateToNextPos);
	}

	public void playParticle () {
//		for (int i = 0; i < 8; i++) {
//			GameObject ptcle = Instantiate(particle);
//			ParticleSystemRenderer psr = ptcle.GetComponent<ParticleSystemRenderer>();
//			psr.mesh = meshs [i];
//			Vector3 pos = transform.position;
//			pos.z = -1.0f;
//			ptcle.transform.position = pos;
//			ptcle.transform.parent = transform;
//			Destroy (ptcle, .5f);
//			if (i == 0) {
//				GameObject trailPtcle = Instantiate(trailParticle);
//				trailPtcle.transform.position = pos;
//				trailPtcle.transform.parent = transform;
//				Destroy (ptcle, .5f);
//
//				GameObject effect = Instantiate(effectBomb);
//				pos.z = -1.5f;
//				effect.transform.position = pos;
//				effect.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
//				effect.transform.parent = transform;
//				Destroy (effect, .5f);
//			}
//		}

		Vector3 pos = transform.position;

		GameObject trailPtcle = Instantiate(trailParticle);
		pos.z = -0.5f;
		trailPtcle.transform.position = pos;
		trailPtcle.transform.parent = transform;
		Destroy (trailPtcle, 2.5f);

		GameObject effect = Instantiate(effectBomb);
		pos.z = -1.5f;
		effect.transform.position = pos;
		effect.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
		effect.transform.parent = transform;
		Destroy (effect, .5f);
	}

//	public void drawRate (GameScript.Rate rate) {
//		GameObject rateObject = Instantiate (textRate);
//		rateObject.transform.parent = transform;
//		rateObject.GetComponent<Animator> ().SetTrigger ("Success");
//		if (rate == GameScript.Rate.Perfect) {
//			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = perfectRate;
//		} else if (rate == GameScript.Rate.Great) {
//			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = greatRate;
//		} else if (rate == GameScript.Rate.Good) {
//			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = goodRate;
//		}
//		Destroy (rateObject, .4f);
//	}
}

