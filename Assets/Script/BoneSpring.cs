using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSpring : MonoBehaviour
{
	Vector3 expectedPosition;
	Vector3 expectedVector;
	Vector3 velocityVector;
	Vector3 forceVector;
	Vector3 positionBefore;
	public float friction = 0.3f;
	float mass = 0.2f; 
//	float distance;

	void Start() {
//		basePosition = transform.localPosition;
		positionBefore = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		Vector3 velocityVector = Vector3.zero;
		expectedVector = transform.position - transform.parent.position;
//		transform.position = transform.position;
//		movedVector = Vector3.zero;
//		distance = 0;
	}

	void Update() {

		resetForce ();
		expectedPosition = transform.parent.position + expectedVector;
		Vector3 force = expectedPosition - positionBefore; //摩擦0で進むべき距離

//		velocityVector += force * friction * mass;

		transform.position = positionBefore + (force - force * friction) + velocityVector + Vector3.down * 0.1f * mass;;
		/*
		addForce(force);

		//updateForce ();
		updatePos ();
*/
		positionBefore = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}

	void resetForce(){
		forceVector = Vector3.zero;
	}

	// 力を加える
	void addForce(Vector3 _force){
		forceVector += _force/mass;
	}

	// 摩擦力の更新
	void updateForce(){
		forceVector -= velocityVector * friction;
	}

	// 位置の更新
	void updatePos(){
		velocityVector += forceVector;

		positionBefore += velocityVector * (1-friction) * Time.deltaTime * Application.targetFrameRate;
		transform.position = positionBefore;
	}
}


/*
SpringBoneEx.cs

using UnityEngine;
using System.Collections;

public class SpringBoneEx : MonoBehaviour
{
	//次のボーン
	public Transform child;

	//ボーンの向き
	public Vector3 boneAxis = new Vector3(-1.0f, 0.0f, 0.0f);

	public float radius = 0.04f;

	//バネが戻る力
	public float stiffnessForce = 0.04f;

	//力の減衰力
	public float dragForce = 1.2f;

	public Vector3 springForce = new Vector3(0.0f, 0.0f, 0.0f);

	public SpringCollider[] colliders;

	public bool debug;

	private float springLength;
	private Quaternion localRotation;
	private Transform trs;
	private Vector3 currTipPos;
	private Vector3 prevTipPos;

	private void Awake()
	{
		trs = transform;
		localRotation = transform.localRotation;
		child = transform.GetChild(0).transform;
	}

	private void Start()
	{
		springLength = Vector3.Distance(trs.position, child.position);
		currTipPos = child.position;
		prevTipPos = child.position;
	}

	public void UpdateSpring()
	{
		//回転をリセット
		trs.localRotation = Quaternion.identity * localRotation;

		float sqrDt = Time.deltaTime * Time.deltaTime;

		//stiffness
		Vector3 force = trs.rotation * (boneAxis * stiffnessForce) / sqrDt;

		//drag
		force += (prevTipPos – currTipPos) * dragForce / sqrDt;

		force += springForce / sqrDt;

		//前フレームと値が同じにならないように
		Vector3 temp = currTipPos;

		//verlet
		currTipPos = (currTipPos – prevTipPos) + currTipPos + (force * sqrDt);

		//長さを元に戻す
		currTipPos = ((currTipPos – trs.position).normalized * springLength) + trs.position;

		//衝突判定
		for (int i = 0; i < colliders.Length; i++)
		{
			if (Vector3.Distance(currTipPos, colliders[i].transform.position) <= (radius + colliders[i].radius))
			{
				Vector3 normal = (currTipPos – colliders[i].transform.position).normalized;
				currTipPos = colliders[i].transform.position + (normal * (radius + colliders[i].radius));
				currTipPos = ((currTipPos – trs.position).normalized * springLength) + trs.position;
			}
		}

		prevTipPos = temp;

		//回転を適用；
		Vector3 aimVector = trs.TransformDirection(boneAxis);
		Quaternion aimRotation = Quaternion.FromToRotation(aimVector, currTipPos – trs.position);
		trs.rotation = aimRotation * trs.rotation;
	}

	private void OnDrawGizmos()
	{
		if (debug)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(currTipPos, radius);
		}
	}
}

*/