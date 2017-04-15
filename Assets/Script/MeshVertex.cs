using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshVertex : MonoBehaviour {
	
	// 位置ベクトルの配列
	public Vector3 position;
	// 速度ベクトルの配列
	Vector3 velocity;
	// 力ベクトルの配列
	Vector3 force;
	// 摩擦係数
	public float friction;
	// 固定するかどうか
	bool bFixed;
	// パーティクルの質量
	float mass;


	public MeshVertex () {
		friction = 0.01f;
		mass = 1f;
		bFixed = false;
	}

	public void setup(Vector3 _position, Vector3 _velocity){
		// 位置を設定
		position = _position;
		// 初期速度を設定
		velocity = _velocity;
	}

	void setup(float positionX, float positionY, float velocityX, float velocityY){
		// 位置を設定
		position = new Vector3(positionX, positionY);
		// 初期速度を設定
		velocity = new Vector3(velocityX, velocityY);
	}

	// 力をリセット
	public void resetForce(){
		force = Vector3.zero;
	}

	// 力を加える
	public void addForce(Vector3 _force){
		force += _force / mass;
	}

	void addForce(float forceX, float forceY){
		force += new Vector3(forceX, forceY);
	}

	// 摩擦力の更新
	public void updateForce(){
		force -= velocity * friction;
	}

	// 位置の更新
	void updatePos(){
		if (!bFixed) {
			velocity += force;
			position += velocity * Time.deltaTime * Application.targetFrameRate;
		}
	}

	// 力の更新と座標の更新をupdateとしてまとめる
	public void update(){
		updateForce();
		updatePos();
	}

	// 反発する力
	public void addRepulsionForce(float x, float y, float radius, float scale){
		Vector3 posOfForce = new Vector3 (x, y, 0);
		Vector3 diff = position - posOfForce;
		float length = diff.magnitude;
		bool bAmCloseEnough = true;
		if (radius > 0){
			if (length > radius){
				bAmCloseEnough = false;
			}
		}
		if (bAmCloseEnough == true){
			float pct = 1 - (length / radius);
			force.x = force.x + diff.normalized.x * scale * pct;
			force.y = force.y + diff.normalized.y * scale * pct;
		}
	}

	public void addRepulsionForce(MeshVertex p, float radius, float scale){
		Vector3 posOfForce = new Vector3 (p.position.x, p.position.y, 0);
		Vector3 diff = position - posOfForce;
		float length = diff.magnitude;
		bool bAmCloseEnough = true;
		if (radius > 0){
			if (length > radius){
				bAmCloseEnough = false;
			}
		}
		if (bAmCloseEnough == true){
			float pct = 1 - (length / radius);
			force.x = force.x + diff.normalized.x * scale * pct;
			force.y = force.y + diff.normalized.y * scale * pct;
			p.force.x = p.force.x - diff.normalized.x * scale * pct;
			p.force.y = p.force.y - diff.normalized.y * scale * pct;
		}
	}

	// 引き付けあう力
	public void addAttractionForce(float x, float y, float radius, float scale){
		Vector3 posOfForce = new Vector3 (x,y,0);
		Vector3 diff = position - posOfForce;
		float length = diff.magnitude;
		bool bAmCloseEnough = true;
		if (radius > 0){
			if (length > radius){
				bAmCloseEnough = false;
			}
		}
		if (bAmCloseEnough == true){
			float pct = 1 - (length / radius);
			force.x = force.x - diff.normalized.x * scale * pct;
			force.y = force.y - diff.normalized.y * scale * pct;
		}
	}

	public void addAttractionForce(MeshVertex p, float radius, float scale){
		Vector3 posOfForce = new Vector3 (p.position.x,p.position.y,0);
		Vector3 diff = position - posOfForce;
		float length = diff.magnitude;
		bool bAmCloseEnough = true;
		if (radius > 0){
			if (length > radius){
				bAmCloseEnough = false;
			}
		}
		if (bAmCloseEnough == true){
			float pct = 1 - (length / radius);
			force.x = force.x - diff.normalized.x * scale * pct;
			force.y = force.y - diff.normalized.y * scale * pct;
			p.force.x = p.force.x + diff.normalized.x * scale * pct;
			p.force.y = p.force.y + diff.normalized.y * scale * pct;
		}
	}
}


