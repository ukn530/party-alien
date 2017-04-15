using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggingSVG : MonoBehaviour {

	public Transform[] bones;
	public Transform power;
	Vector3 beforePowerPos;
	public float radius = 1f; 

	MeshFilter filter;
	MeshVertex[] vertices;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		filter = GetComponent<MeshFilter> ();

		vertices = new MeshVertex[filter.sharedMesh.vertices.Length];
		beforePowerPos = new Vector3(power.position.x, power.position.y, power.position.z);
//		beforePowerPos.position = ;

		// 頂点配列の設定
		for (int i = 0; i < filter.sharedMesh.vertices.Length; i++) {
			MeshVertex myMeshVertex = new MeshVertex();
			myMeshVertex.friction = 0;
			float x = filter.mesh.vertices[i].x;
			float y = filter.mesh.vertices[i].y;
			myMeshVertex.setup(new Vector3(x, y), new Vector3(0, 0));
			vertices[i] = myMeshVertex;
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		for (int i = 0; i < bones.Length; i++) {
			for (int j = 0; j < filter.sharedMesh.vertices.Length; j++) {
				if ((filter.sharedMesh.vertices [j] - bones [i].position).sqrMagnitude < radius) {
					
				}
			}
		}
*/
		for (int j = 0; j < filter.sharedMesh.vertices.Length; j++) {

			if ((filter.sharedMesh.vertices [j] - power.localPosition).sqrMagnitude < radius) {
				//Debug.Log ("vertices: " + filter.sharedMesh.vertices[j]);
				//Debug.Log ("powerPos.position - beforePowerPos.position: " + (power.position - beforePowerPos));
//				Vector3 distancePerFrame = powerPos.position - beforePowerPos.position;
				vertices [j].position += (power.position - beforePowerPos);
			}
		}
		beforePowerPos = new Vector3(power.position.x, power.position.y, power.position.z);

		drawCircle ();
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
