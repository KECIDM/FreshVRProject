using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class WarriorAI : MonoBehaviour {
	public GameObject Player;
	public Transform Target;
	public UnityEngine.AI.NavMeshAgent agent;
	private Animator anim;
	public float DistToDest;
	public float WaitTime;
    private bool isAttacking = false;
	//public GameObject ShotPoint;
	public GameObject Sting;
	// Use this for initialization
	void Start () {
		Sting.SetActive (false);
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		agent.enabled = true;
		anim = GetComponent<Animator> ();
		anim.SetBool("Forward", true);
		agent.SetDestination (Target.position);		
	}

	// Update is called once per frame
	void Update () {
		agent.SetDestination (Target.position);
		bool isEnabled = agent.isActiveAndEnabled;
		DistToDest = Vector3.Distance (transform.position, Target.transform.position);

		if (DistToDest < 5f) {
			Sting.transform.LookAt (Target.transform);
			anim.SetBool("Forward",false);
			anim.SetBool ("Attacking", true);
			ShootLaser ();


		} else {
			anim.SetBool ("Forward", true);
			Sting.SetActive (false);
		}


	}
	IEnumerator DelayActive(float WaitTime){
		yield return new WaitForSeconds(WaitTime);
		agent.enabled = true;
		agent.SetDestination (Target.position);
	}

	void ShootLaser (){
        if(!isAttacking)
		    StartCoroutine ("Laser", 0.25f);
	}


	IEnumerator Laser(){
        isAttacking = true;
		yield return new WaitForSeconds (1);
		Sting.SetActive (true);
		LineRenderer Beam = Sting.GetComponent<LineRenderer> ();
		Beam.SetPosition (1, new Vector3(0.0f,0.0f,400.0f));

		RaycastHit hit;

		if (Physics.Raycast (Sting.transform.position, Sting.transform.forward, out hit, 6.0f)) {

			if(hit.collider.gameObject.tag == "Player"){
				PlayerHealth HealthScript = Player.GetComponent<PlayerHealth> ();
				HealthScript.Health--;
			}
		}

		yield return new WaitForSeconds(0.25f);
		Sting.SetActive(false);
        isAttacking = false;
		StopCoroutine ("Laser");
	}

}
