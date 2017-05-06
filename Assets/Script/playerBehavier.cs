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
		GetComponent<Animator> ().SetTrigger ("kick");
	}

	public void playStartAnimation () {
		GetComponent<Animator> ().SetTrigger ("start");
	}

	public void shakeGraphic () {
	}

	public void changeGrapics () {
	}

	public void playParticle () {
		for (int i = 0; i < 8; i++) {
			GameObject ptcle = Instantiate(particle);
			ParticleSystemRenderer psr = ptcle.GetComponent<ParticleSystemRenderer>();
			psr.mesh = meshs [i];
			Vector3 pos = transform.position;
			pos.z = -1.0f;
			pos.y += gameObject.GetComponent<SpriteRenderer>().bounds.size.y/2;
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

