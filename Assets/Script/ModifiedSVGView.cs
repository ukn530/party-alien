using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(MeshRenderer))]
//[RequireComponent (typeof(MeshFilter))]

public class ModifiedSVGView : MonoBehaviour {

	[SerializeField]

	public float friction = 0.04f;
	public float springiness = 0.3f;

	MeshFilter filter;
	Spring[] springs;
	MeshVertex[] vertices;
	Vector3[] basePosition;

	Vector3 mousePosition;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		filter = GetComponent<MeshFilter> ();

		basePosition = filter.mesh.vertices;

		/*
		var mesh = new Mesh();

		// 頂点を定義
		Vector3[] vers = new Vector3[filter.mesh.vertices.Length];
		mesh.vertices = vers;

		// Meshのどの頂点を結んで三角形を作るかを定義
		int[] triangles = new int[(filter.mesh.vertices.Length - 2) * 3]; //常に3の倍数になる
		for (int i = 0; i < (filter.mesh.vertices.Length - 2) * 3; i++) {

			if (i % 3 == 0) {
				triangles [i] = 0;
			} else if (i % 3 == 1) {
				triangles [i] = (i + 2) / 3;
			} else {
				triangles [i] = (i + 1) / 3 + 1;
			}
		}

		mesh.triangles = triangles;
		filter.sharedMesh = mesh;
*/

		// TODO: mesh頂点をあとで値をわたすのではなく
		// verticiesで参照して直接いじる

		springs = new Spring[filter.sharedMesh.vertices.Length];
		vertices = new MeshVertex[filter.sharedMesh.vertices.Length];

		// 頂点配列の設定
		for (int i = 0; i < filter.sharedMesh.vertices.Length; i++) {
			MeshVertex myMeshVertex = new MeshVertex();
			myMeshVertex.friction = friction;
			float x = filter.mesh.vertices[i].x;
			float y = filter.mesh.vertices[i].y;
			myMeshVertex.setup(new Vector3(x, y), new Vector3(0, 0));
			vertices[i] = myMeshVertex;
		}

		//全ての配列を順番にspringで接続していく
		for (int i = 0; i < filter.sharedMesh.vertices.Length; i++) {
			Spring mySpring = new Spring ();
			mySpring.distance = 0;
			mySpring.springiness = springiness;
			mySpring.particleA = vertices [i];
			mySpring.basePosition = basePosition[i];
			springs [i] = mySpring;
		}

	}


	// Update is called once per frame
	void Update () {

		Vector3 pos = Input.mousePosition;
		pos.z = Camera.main.transform.position.z * -1;
		mousePosition = Camera.main.ScreenToWorldPoint(pos);

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				// Construct a ray from the current touch coordinates
				mousePosition = Camera.main.ScreenToWorldPoint (touch.position);
			}
		}

		if (Input.GetMouseButtonDown(0)) {
			vibrateCircle ();
		}


		// 全てのparticleの力をリセット
		for (int i = 0; i < vertices.Length; i++){
			vertices[i].resetForce();
		}

		for (int i = 0; i < vertices.Length; i++){

			vertices[i].addRepulsionForce(mousePosition.x, mousePosition.y, 2f, 0.05f);

			// パーティクル同士の反発する力
			for (int j = 0; j < i; j++){
				//vertices[i].addRepulsionForce(vertices[j], 100f, 1f);
			}
		}

		// バネを更新
		for (int i = 0; i < springs.Length; i++){
			springs[i].updateAsSVG();
		}

		// パーティクルの状態を更新
		// Vector3[] vers = new Vector3[circleResolution];
		for (int i = 0; i < vertices.Length; i++){
			vertices[i].updateForce();
			vertices[i].update();
		}

		drawCircle ();
	}

	void vibrateCircle() {

		// 中央のpositionを算出
		Vector3 centerPos = Vector3.zero;

		for (int i = 0; i < vertices.Length; i++) {
			centerPos += vertices [i].position;
		}

		centerPos /= vertices.Length;

		for (int i = 0; i < vertices.Length; i++) {

			Vector3 pA = vertices [(i + vertices.Length - 1)%vertices.Length].position;
			Vector3 pB = vertices [(i + 1)%vertices.Length].position;
			Vector3 diff = (pA - pB).normalized;
			Debug.Log ("pA:pB:diff " + diff);
			Vector3 vector = Vector3.zero;
			if (i % 2 == 0) {
				vector = Quaternion.Euler (0f, 0f, 90f) * diff * 3f;
			} else {
				vector = Quaternion.Euler (0f, 0f, 90f) * diff * -3f;
			}
			Vector3 pos = vertices [i].position + vector;
			vertices [i].position = pos;
		}
	}

	void drawCircle(){

		Mesh mesh = filter.sharedMesh;

		// Meshの頂点を作成
		Vector3[] vers = mesh.vertices;
		for (int i = 0; i < filter.sharedMesh.vertices.Length; i++) {
			vers[i] = vertices[i].position;
		}
		mesh.vertices = vers;
		mesh.RecalculateNormals();
	}
}
