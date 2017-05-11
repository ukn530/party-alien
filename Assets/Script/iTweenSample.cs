using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTweenSample : MonoBehaviour {

	Hashtable ht = new Hashtable();

	void Awake (){
		ht.Add("x", 3);
		ht.Add("time", 4);
		ht.Add("delay", 1);
		ht.Add("onupdate", "myUpdateFunction");
		ht.Add("looptype", iTween.LoopType.none);
	}

	// Use this for initialization
	void Start () {

		iTween.MoveBy(gameObject, ht);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
