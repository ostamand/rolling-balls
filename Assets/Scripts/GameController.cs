using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;

public class GameController: MonoBehaviour
{
    public Text playerScoreLabel;
    public Text agentScoreLabel;
    public Text centerLabel;

    public int pointsPerHit = 1;
    public int pointsPerFell = 1;

    private int playerScore = 0;
    private int agentScore = 0;

    void Start ()
    {
        centerLabel.enabled = false;
	}
	
	void Update ()
    {
		
	}

    #region Public Methods 

    public void AddToScore(int score, TypeOf update)
    {
        if (update == TypeOf.Player) { playerScore++; }
        if (update == TypeOf.Agent) { agentScore++; }
        playerScoreLabel.text = playerScore.ToString();
        agentScoreLabel.text = agentScore.ToString();

        CheckScore();
    }

    public void Hit(TypeOf update)
    {
        AddToScore(pointsPerHit, update);
    }

    public void PlayerFell()
    {
        AddToScore(pointsPerFell, TypeOf.Agent);
    }

    public void AgentFell()
    {
        AddToScore(pointsPerFell, TypeOf.Player);
    }

    #endregion

    #region Private Helpers

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

    #endregion
}
