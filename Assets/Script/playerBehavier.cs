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

	public GameObject textRate;
	public SVGAsset goodRate;
	public SVGAsset greatRate;
	public SVGAsset perfectRate;

	Mesh[] meshs;

	// Use this for initialization
	void Start () {
		svgRenderer = gameObject.GetComponent<SVGRenderer> ();

		meshs = new Mesh[] {p1, p2, p3, p4, p5, p6, p7, p8};
	}

	// Update is called once per frame
	void Update () {

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		int i = 0;
		while (i < vertices.Length) {
			vertices[i] += Vector3.one * Random.Range(-10, 10);
			i++;
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}

	public void playBaseRhythm () {
		GetComponent<Animator> ().SetTrigger ("kick");
	}

	public void shakeGraphic () {
	}

	public void changeGrapics () {
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

	public void playParticle () {
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

