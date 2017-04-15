using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spring : MonoBehaviour {

	// 両端のMeshVertexへの参照
	public MeshVertex particleA;
	public MeshVertex particleB;
	public Vector3 basePosition;

	// 力をかけていない状態のばねの長さ
	public float distance = 0;
	// ばね定数
	public float springiness = 0;

	public Spring(){
		
	}

	public void update(){

		// 両端のMeshVertexの位置を取得
		Vector3 pta = particleA.position;
		Vector3 ptb = particleB.position;
		// 距離を算出
		float theirDistance = (pta - ptb).magnitude;
		// 距離からばねにかかる力を計算（フックの法則）
		float springForce = (springiness * (distance - theirDistance));
		// ばねの力から、両端への力ベクトルを計算
		Vector3 frcToAdd = (pta-ptb).normalized * springForce;
		// それぞれのMeshVertexに力を加える
//		Debug.Log("frcToAdd: " + frcToAdd);
		particleA.addForce(new Vector3(frcToAdd.x, frcToAdd.y));
		particleB.addForce(new Vector3(-frcToAdd.x, -frcToAdd.y));
	}

	public void updateAsSVG(){

		// 両端のMeshVertexの位置を取得
		Vector3 pta = particleA.position;
		Vector3 ptb = basePosition;
		// 距離を算出
		float theirDistance = (pta - ptb).magnitude;
		// 距離からばねにかかる力を計算（フックの法則）
		float springForce = (springiness * (distance - theirDistance));
		// ばねの力から、両端への力ベクトルを計算
		Vector3 frcToAdd = (pta-ptb).normalized * springForce;
		// それぞれのMeshVertexに力を加える
		//		Debug.Log("frcToAdd: " + frcToAdd);
		particleA.addForce(new Vector3(frcToAdd.x, frcToAdd.y));
		//particleB.addForce(new Vector3(-frcToAdd.x, -frcToAdd.y));
	}
}

