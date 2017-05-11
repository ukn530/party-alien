using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRateView : MonoBehaviour {


	public Sprite goodRate;
	public Sprite greatRate;
	public Sprite perfectRate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void setRate(GameScript.Rate rate) {
		
		Debug.Log (rate);
		if (rate == GameScript.Rate.Good) {
			GetComponent<SpriteRenderer> ().sprite = goodRate;
		} else if (rate == GameScript.Rate.Great) {
			GetComponent<SpriteRenderer> ().sprite = greatRate;
		} else {
			GetComponent<SpriteRenderer> ().sprite = perfectRate;
		}

	}
}
