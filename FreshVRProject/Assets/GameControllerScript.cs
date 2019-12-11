using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    public GameObject Player;
    public GameObject MainCam;
    public float Wave1Delay;
    public float Wave2Delay;
    public GameObject[] WarriorWave;
    //public GameObject[] DroneWave;
    public Canvas MainUI;
    public TMPro.TextMeshProUGUI ScoreText;
    public int Score;
    public int Wavecount;
    public Transform[] TeleportPoints;
    public int CurrentTeleportIndex = 0;
    private Transform CurrentTeleportPoint;
    private int WaveCount;

    // Use this for initialization
    void Start() {
        //Teleportball.SetActive(false);
        foreach (GameObject warrior in WarriorWave)
            warrior.SetActive(false);
        /*
        foreach (GameObject drone in DroneWave)
            drone.SetActive(false);
            */


    }

    // Update is called once per frame
    void Update() {



    }
    IEnumerator Waves() {
        yield return new WaitForSeconds(Wave1Delay);
        foreach (GameObject warrior in WarriorWave)
        {
            warrior.SetActive(true);
            warrior.GetComponent<Animator>().SetBool("Forward", true);
        }
        /*
        yield return new WaitForSeconds(Wave2Delay);
        foreach (GameObject drone in DroneWave)
            drone.SetActive(true);
            */
    }

    public void ScorePoints(int points) {
        Score += points;
        ScoreText.text = "Score = " + Score;
        if (Score >= 20)
        {
            CurrentTeleportIndex++;

            StopCoroutine("Waves");
            if (CurrentTeleportIndex < TeleportPoints.Length - 1)
            {
                Player.transform.position = TeleportPoints[CurrentTeleportIndex].position;
            }
            else
            {
                foreach (GameObject warrior in WarriorWave)
                    warrior.GetComponent<Animator>().SetTrigger("Dead");
                //Teleportball.SetActive(true);
            }

            Score = 0;
        }
    }

    public void StartGame()
    {
        StartCoroutine("Waves");
    }
    public void GameOver()
    {
        StopCoroutine("Waves");
        ScoreText.text = "GAMEOVER";
    }

    
}
