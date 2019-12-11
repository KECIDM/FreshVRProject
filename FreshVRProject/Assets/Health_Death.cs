using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Death : MonoBehaviour {

	public GameObject Explosion;
	public GameObject Body;
	//public GameObject Hit;
	public int Health;


	// Use this for initialization
	void Start () {

		Explosion.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Health <= 0) {
			Explosion.SetActive (true);
			Body.SetActive (false);
		}
		
	}
}
