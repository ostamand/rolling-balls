using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{

    // TODO add back & quit button for android
    // TODO add sounds
    // TODO change graphics

    #region Public Fields

    public Text playerScoreLabel;
    public Text agentScoreLabel;
    public Text centerLabel;
    public Image imageSplash;
    public Image imageScore;
       
    public int pointsPerHit = 1;
    public int pointsPerFell = 1;

    #endregion

    #region Private Fields

    private int _playerScore = 0;
    private int _agentScore = 0;
    private Player _player;
    private RollerTrainedAgent agent;
    private bool _isSplash;

    #endregion

    void Start ()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0) { _isSplash = true; }

        agent = FindObjectOfType<RollerTrainedAgent>();
        if (!_isSplash)
        {
            SetLeveUI();
        }
        else
        {
            SetSplashUI();
        }
    }

    private void SetLeveUI()
    {
        int zero = 0;
        imageSplash.enabled = false;
        imageScore.enabled = true;
        playerScoreLabel.text = zero.ToString();
        agentScoreLabel.text = zero.ToString();

        // set button display off
        Button button = GetComponentInChildren<Button>();
        button.gameObject.SetActive(false);

        // set center label text
        centerLabel.text = "Level " + (SceneManager.GetActiveScene().buildIndex);

        // set player off 
        _player = FindObjectOfType<Player>();
        _player.SetActive(false);

        Invoke("Activate", 1f);
    }

    private void SetSplashUI()
    {
        imageScore.enabled = false;
        imageSplash.enabled = true;
        playerScoreLabel.enabled = false;
        agentScoreLabel.enabled = false;
        centerLabel.enabled = false;
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

    public void Play()
    {
        print("there");
        LoadNextLevel();
    }

    #endregion

    #region Private Helpers

    private void Activate()
    {
        centerLabel.enabled = false;
        _player.SetActive(true);
        agent.SetActive(true);
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
        else
        {
            SceneManager.LoadScene(0);
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
        if (_isSplash) { return; }
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
