using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorer : MonoBehaviour {

    public enum TypeOfUpdate { PLAYER, AGENT }

    public Text playerScoreLabel;
    public Text agentScoreLabel;
    public Text centerLabel;

    public int pointsPerHit = 1;
    public int pointsPerFell = 1;

    private int playerScore = 0;
    private int agentScore = 0;

    // Use this for initialization
    void Start () {
        centerLabel.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToScore(int score, TypeOfUpdate target)
    {
        if(target == TypeOfUpdate.PLAYER){playerScore++;}
        if(target == TypeOfUpdate.AGENT) {agentScore++; }
        playerScoreLabel.text = playerScore.ToString();
        agentScoreLabel.text = agentScore.ToString();

        CheckScore();
    }

    private void CheckScore()
    {
        if (playerScore == 20)
        {
            centerLabel.text = "Win!";
            centerLabel.enabled = true;
        }
        if(agentScore==20)
        {
            centerLabel.text = "Loose!";
            centerLabel.enabled = true;
        }
    }

    public void PlayerHit()
    {
        AddToScore(pointsPerHit, TypeOfUpdate.PLAYER);
    }

    public void PlayerFell()
    {
        AddToScore(pointsPerFell, TypeOfUpdate.AGENT);
    }

    public void AgentHit()
    {
        AddToScore(pointsPerHit, TypeOfUpdate.AGENT);
    }

    public void AgentFell()
    {
        AddToScore(pointsPerFell, TypeOfUpdate.PLAYER);
    }

}
