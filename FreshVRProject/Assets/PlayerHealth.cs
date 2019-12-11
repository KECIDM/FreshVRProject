using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int Health;
    public GameObject GC;
    private GameControllerScript GCScript;
	// Use this for initialization
	void Start () {
        GCScript = GC.GetComponent<GameControllerScript>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Health <= 0)
        {
            GCScript.GameOver();
        }
			
			
		
	}


}
