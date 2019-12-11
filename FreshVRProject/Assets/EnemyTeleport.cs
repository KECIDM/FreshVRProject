// Copyright 2014 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

	[RequireComponent(typeof(Collider))]
	public class EnemyTeleport : MonoBehaviour {
		
	public Vector3 startingPosition;
	//smokeeffects
	public GameObject PS;
    public GameObject Explosion;
	//materials for enemies
	public Material inactiveMaterial;
	public Material gazedAtMaterial;
	//public GvrReticlePointer reticle;
	//gamecontroller
	public GameObject GC;
	//targetingcanvas
	public GameObject targeting;
    public bool hit = false;
	protected Animator anim;


    void Awake(){
        startingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		//reticle.GetComponent<Renderer> ().material.color = Color.white;
		targeting.SetActive (false);
    }

  void Start() {
		anim = GetComponent<Animator> ();
		startingPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		PS.SetActive (false);
	
    SetGazedAt(false);
  }

  public void SetGazedAt(bool gazedAt) {

    if (inactiveMaterial != null && gazedAtMaterial != null) {
      GetComponentInChildren<Renderer>().material = gazedAt ? gazedAtMaterial : inactiveMaterial;
			//reticle.GetComponent<Renderer> ().material.color = gazedAt ? Color.red : Color.white;
			if (gazedAt){
				targeting.SetActive (true);
			}else{
				targeting.SetActive(false);
			}
      		return;
    }
		GetComponentInChildren<Renderer>().material.color = gazedAt ? Color.green : Color.red;


  }

	public void die(){
        if (hit)
        {
            anim.SetBool("Attacking", false);
            anim.SetBool("Forward", false);
            anim.SetTrigger("Dead");
            StartCoroutine("Dying", 0.5);
            hit = false;
        }
	}

  public void ResetEnemy() { 
    transform.position = startingPosition;
	anim.ResetTrigger ("Dead");
	SetGazedAt (false);
	anim.SetBool ("Attacking", false);
	anim.SetBool ("Forward", true);
	PS.SetActive (false);

  }

	IEnumerator Dying(float WaitTime){
		PS.SetActive (true);
		yield return new WaitForSeconds (WaitTime);
		GC.GetComponent<GameControllerScript> ().ScorePoints (1);
        Instantiate(Explosion, transform.position, transform.rotation);
		Debug.Log (GC.GetComponent<GameControllerScript>().Score);

        ResetEnemy();

	}
	/*
  public void TeleportRandomly() {
    Vector3 direction = Random.onUnitSphere;
    direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
    float distance = 2 * Random.value + 1.5f;
    transform.localPosition = direction * distance;
  }
  */
}
