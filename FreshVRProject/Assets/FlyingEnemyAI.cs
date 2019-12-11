using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FlyingEnemyAI : MonoBehaviour {

	private NavMeshAgent agent;
	public GameObject Player;
	public float DistToDest;
	public Transform MeasurePt;
	public Transform Body;
	public GameObject ShotEffects;
    public GameObject Gun;
    public GameObject Shot;
	public bool approaching;
	public bool attacking;
	public Transform[] DodgePoints;
	public bool dodging;
	public int TargetIndex;
	public Transform nextTarget;
	public Transform DiveTarget;
	public float attackTimer;
	public Transform RotationTarget;
	public float speed;

	//private Quaternion StartRotation;

	// Use this for initialization
	void Start () {
		attackTimer = 0;
		agent = GetComponent<NavMeshAgent> ();
		agent.enabled = true;
		//StartRotation = Body.localRotation;
		
		ShotEffects.SetActive (false);
		attacking = false;
		approaching = true;
		TargetIndex = 0;
		nextTarget = DodgePoints [0];
        agent.SetDestination(Player.transform.position);
        DistToDest = Vector3.Distance(MeasurePt.position, Player.transform.position);
        Debug.Log(DistToDest);
    }
	
	// Update is called once per frame
	void Update () {
		attackTimer += Time.deltaTime;
		if (approaching) {
			
			agent.enabled = true;
			agent.SetDestination (Player.transform.position);
			attacking = false;
			dodging = false;

			DistToDest = Vector3.Distance (MeasurePt.position, Player.transform.position);

			if (DistToDest < agent.stoppingDistance + 1.0f) {
				agent.speed = 0;
               
				dodging = true;
				attacking = false;
				approaching = false;
			}
		} else if (dodging) {
			//nextTarget = DodgePoints [TargetIndex];
			DistToDest = Vector3.Distance (transform.position, nextTarget.position);
			agent.SetDestination (nextTarget.position);
            /*
			Vector3 targetDir = nextTarget.position - transform.position;

			float step = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 100.0F);
            
			transform.rotation = Quaternion.LookRotation(newDir);
            */

			//transform.position = Vector3.MoveTowards (transform.position, nextTarget.position, 0.1f);
			if (DistToDest < 2.0f) {
				TargetIndex++;
				if (TargetIndex > 3) {

					TargetIndex = 0;
				}
				DistToDest = Vector3.Distance (transform.position, nextTarget.position);
				if (attackTimer > 4.0f) {
					attacking = true;
					dodging = false;
                    approaching = false;
				}
				/*
					agent.enabled = true;
					agent.SetDestination (Player.transform.position);
					DistToDest = Vector3.Distance (transform.position, Player.transform.position);
					attacking = true;
					dodging = false;
					approaching = false;
					*/

					
			}


		} else if (attacking) {
			dodging = false;
            approaching = false;
			ShotEffects.SetActive (true);
			nextTarget = DiveTarget;
			DistToDest = Vector3.Distance (transform.position, nextTarget.position);
			if(DistToDest >2.0f)
				transform.position = Vector3.Lerp (transform.position, nextTarget.position, 0.15f);

			if (DistToDest < 2.0f) 

				attack ();




		}

	}

	void LateUpdate(){
		if(!attacking)
		Body.LookAt (nextTarget.position);

	}

	void attack(){
        if (attacking)
        {
            StartCoroutine("Attack");
        }
	}




	IEnumerator Attack(){
        attacking = false;
        int RandomIndex;
		RandomIndex = Random.Range (0, 3);
		TargetIndex = RandomIndex;
		 
		dodging = false;
		approaching = false;
		Body.LookAt (Player.transform);

		ShotEffects.SetActive (true);
        GameObject newShot = Instantiate(Shot, Gun.transform.position, Gun.transform.rotation) as GameObject;
        newShot.GetComponent<Rigidbody>().velocity = Vector3.forward * 200f;
        Destroy(newShot, 2);
        yield return new WaitForSeconds (1.0f);
        transform.RotateAround (Player.transform.position, Vector3.up, 2.0f);
		dodging = true;
		nextTarget = DodgePoints [TargetIndex];
		attackTimer = 0.0f;
		ShotEffects.SetActive (false);
		nextTarget = DodgePoints [0];
		StopCoroutine ("Attack");
	}



}
