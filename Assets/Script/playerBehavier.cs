using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVGImporter;
using UnityEngine.UI;

public class playerBehavier : MonoBehaviour {

	public GameObject particle;
	public Mesh p1, p2, p3, p4, p5, p6, p7, p8;

	public GameObject textRate;
	public SVGAsset goodRate;
	public SVGAsset greatRate;
	public SVGAsset perfectRate;

	
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
			animationGetStartPos.Add ("position", new Vector3 (16f, 4.6f, - 2f));
		} else {
			animationGetStartPos.Add ("position", new Vector3 (10f, 4.6f, - 2f));
		}
		animationGetStartPos.Add("time", 4);
		animationGetStartPos.Add("easetype", "easeInOutCubic");
		iTween.MoveTo(gameObject, animationGetStartPos);
	}

	public void shakeGraphic () {
	}

	public void translateCharacter (float[] angleAndDistance) {
		float angle = angleAndDistance[0];
		float distance = angleAndDistance[1] * 10;
						Debug.Log ("distsnce:" + distance);

		float rad = angle * Mathf.Deg2Rad;

		//rad(ラジアン角)から発射用ベクトルを作成
		float x = Mathf.Sin(rad) * distance;
		float y = Mathf.Cos(rad) * distance;
		Debug.Log ("x,y : " + x + "," + y);

		Hashtable animationTranslateToNextPos = new Hashtable ();

		animationTranslateToNextPos.Add ("amount", new Vector3 (x, y, 0));
//
//		if (gameObject.name == "Enemy") {
//			animationTranslateToNextPos.Add ("position", new Vector3 (x, y));
//		} else {
////			animationTranslateToNextPos.Add ("position", new Vector3 (3.2f, 4.4f - 2f));
//		}
		animationTranslateToNextPos.Add("time", angleAndDistance[1] * 0.5f);
		animationTranslateToNextPos.Add("easetype", "easeOutCubic");
		iTween.MoveBy (gameObject, animationTranslateToNextPos);
	}

	public void playParticle () {
		for (int i = 0; i < 8; i++) {
			GameObject ptcle = Instantiate(particle);
			ParticleSystemRenderer psr = ptcle.GetComponent<ParticleSystemRenderer>();
			psr.mesh = meshs [i];
			Vector3 pos = transform.position;
			pos.z = -1.0f;
			ptcle.transform.position = pos;
			ptcle.transform.parent = transform;
			Destroy (ptcle, .5f);
		}
	}

	public void drawRate (GameScript.Rate rate) {
		GameObject rateObject = Instantiate (textRate);
		rateObject.transform.parent = transform;
		rateObject.GetComponent<Animator> ().SetTrigger ("Success");
		if (rate == GameScript.Rate.Perfect) {
			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = perfectRate;
		} else if (rate == GameScript.Rate.Great) {
			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = greatRate;
		} else if (rate == GameScript.Rate.Good) {
			rateObject.GetComponent<SVGRenderer> ().vectorGraphics = goodRate;
		}
		Destroy (rateObject, .4f);
	}
}

