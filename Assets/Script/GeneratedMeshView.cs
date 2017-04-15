using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]

public class GeneratedMeshView : MonoBehaviour {
	
	[SerializeField]
	private Material _mat;
	public static int circleResolution = 60;
	private MeshFilter filter;
	public Text fpsText;

	public GameObject yellowEgg;
	public float friction = 0.04f;
	public float springiness = 0.3f;


	Spring[] springs = new Spring[circleResolution];
	MeshVertex[] vertices = new MeshVertex[circleResolution];
	bool pressed = false;

	Vector3 mousePosition;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		filter = GetComponent<MeshFilter> ();

		var mesh = new Mesh();

		// 頂点を定義
		Vector3[] vers = new Vector3[circleResolution];
		mesh.vertices = vers;

		// Meshのどの頂点を結んで三角形を作るかを定義
		int[] triangles = new int[(circleResolution - 2) * 3]; //常に3の倍数になる
		for (int i = 0; i < (circleResolution - 2) * 3; i++) {
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


		// TODO: mesh頂点をあとで値をわたすのではなく
		// verticiesで参照して直接いじる

		// 頂点配列の設定
		for (int i = 0; i < circleResolution; i++) {
			MeshVertex myMeshVertex = new MeshVertex();
			myMeshVertex.friction = friction;
			float x = Mathf.Sin (Mathf.PI * 2f * ((float)i / (float)circleResolution)) * 100;
			float y = Mathf.Cos (Mathf.PI * 2f * ((float)i / (float)circleResolution)) * 100;
			myMeshVertex.setup(new Vector3(x, y), new Vector3(0, 0));
			vertices[i] = myMeshVertex;
		}

		//全ての配列を順番にspringで接続していく
		for (int i = 0; i < circleResolution; i++) {
			Spring mySpring = new Spring ();
			mySpring.distance = 0;
			mySpring.springiness = springiness;
			mySpring.particleA = vertices [i];
			mySpring.particleB = vertices [(i + 1) % vertices.Length];
			springs [i] = mySpring;
		}

		var renderer = GetComponent<MeshRenderer> ();
		renderer.material = _mat;
	}


	// Update is called once per frame
	void Update () {

		Vector3 pos = Input.mousePosition;
		pos.z = 570f;
		mousePosition = Camera.main.ScreenToWorldPoint(pos);

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				// Construct a ray from the current touch coordinates
				mousePosition = Camera.main.ScreenToWorldPoint (touch.position);
			}
		}

		yellowEgg.transform.position = mousePosition;

		if (Input.GetMouseButtonDown(0)) {
			vibrateCircle ();
		}

		
		// 全てのparticleの力をリセット
		for (int i = 0; i < vertices.Length; i++){
			vertices[i].resetForce();
		}

		for (int i = 0; i < vertices.Length; i++){
			if(pressed){
				// マウスの位置に反発する力
				vertices[i].addAttractionForce(mousePosition.x, mousePosition.y, 150f, 4f);
			} else {
				// マウスの位置に引きつけられる力
				vertices[i].addRepulsionForce(mousePosition.x, mousePosition.y, 150f, 4f);
			}
			// パーティクル同士の反発する力
			for (int j = 0; j < i; j++){
				vertices[i].addRepulsionForce(vertices[j], 100f, 1f);
			}
		}

		// バネを更新
		for (int i = 0; i < springs.Length; i++){
			springs[i].update();
		}

		// パーティクルの状態を更新
		// Vector3[] vers = new Vector3[circleResolution];
		for (int i = 0; i < vertices.Length; i++){
			vertices[i].updateForce();
			vertices[i].update();
		}

		drawCircle ();
		fpsText.text = 1f / Time.deltaTime + "fps";
	}

	void vibrateCircle() {
		
		// 中央のpositionを算出
		Vector3 centerPos = Vector3.zero;

		for (int i = 0; i < vertices.Length; i++) {
			centerPos += vertices [i].position;
		}

		centerPos /= vertices.Length;


//		mySpring.particleA = vertices [i];
//		mySpring.particleB = vertices [(i + 1) % vertices.Length];

		for (int i = 0; i < vertices.Length; i++) {
			
			Vector3 pA = vertices [(i + vertices.Length - 1)%vertices.Length].position;
			Vector3 pB = vertices [(i + 1)%vertices.Length].position;
			Vector3 diff = (pA - pB).normalized;
			Debug.Log ("pA:pB:diff " + diff);
			Vector3 vector = Vector3.zero;
			if (i % 2 == 0) {
				vector = Quaternion.Euler (0f, 0f, 90f) * diff * 30f;
			} else {
				vector = Quaternion.Euler (0f, 0f, 90f) * diff * -30f;
			}
			Vector3 pos = vertices [i].position + vector;
			vertices [i].position = pos;
		}

		yellowEgg.GetComponent<Animator> ().SetTrigger ("Tap");
	}

	void drawCircle(){

		Mesh mesh = filter.sharedMesh;

		// Meshの頂点を作成
		Vector3[] vers = mesh.vertices;
		for (int i = 0; i < circleResolution; i++) {
			vers[i] = vertices[i].position;
		}
		mesh.vertices = vers;
		mesh.RecalculateNormals();
	}
}
