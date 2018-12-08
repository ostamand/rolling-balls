using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{
    #region Public Fields

    public Text playerScoreLabel;
    public Text agentScoreLabel;
    public Text centerLabel;

    public int pointsPerHit = 1;
    public int pointsPerFell = 1;

    #endregion

    #region Private Fields

    private int _playerScore = 0;
    private int _agentScore = 0;
    private Player _player;
    private RollerTrainedAgent _agent;

    #endregion

    void Start ()
    {
        // start deactivated
        centerLabel.text = "Level " + (SceneManager.GetActiveScene().buildIndex+1);
        centerLabel.enabled = true;
        _player = FindObjectOfType<Player>();
        _agent = FindObjectOfType<RollerTrainedAgent>();
        _player.SetActive(false);
        _agent.SetActive(false);
        Invoke("Activate", 1f);
    }
	
	void Update ()
    {
        ProcessInput();
	}

    #region Public Methods 

    public void AddToScore(int score, TypeOf update)
    {
        if (update == TypeOf.Player) { _playerScore++; }
        if (update == TypeOf.Agent) { _agentScore++; }
        playerScoreLabel.text = _playerScore.ToString();
        agentScoreLabel.text = _agentScore.ToString();

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

    private void Activate()
    {
        centerLabel.enabled = false;
        _player.SetActive(true);
        _agent.SetActive(true);
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex != SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
    }

    private void ReloadCurrentLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current);
    }

    private void LoadLevel(int index)
    {
        if (index != SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
    }

    private void StartTransition()
    {
        // deactive player movements
        _player.SetActive(false);
    }

    private void CheckScore()
    {
        if (_playerScore == 20)
        {
            centerLabel.text = "Win!";
            centerLabel.enabled = true;
            StartTransition();
            Invoke("LoadNextLevel", 3f);
        }
        if(_agentScore==20)
        {
            centerLabel.text = "Loose!";
            centerLabel.enabled = true;
            StartTransition();
            Invoke("ReloadCurrentLevel", 3f);
        }
    }

    #endregion
}
