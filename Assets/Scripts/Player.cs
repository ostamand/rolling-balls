using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    public float speed = 10;
    public GameObject target;

    private Rigidbody _rigidBody;
    private TargetHandler _target;
    private GameController _game;
    private bool _isActive = true;

    #region Public Methods

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    #endregion

    void Start ()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _target = target.GetComponent<TargetHandler>();
        _game = FindObjectOfType<GameController>();
    }
	
	void Update ()
    {
        ProcessInput();
        ProcessPosition();
    }

    private void ProcessPosition()
    {
        // update position wrt target
        _target.UpdatePosition(this.transform.position, TypeOf.Player);

        if(Helper.CheckFellOff(this.transform.position))
        {
            _game.PlayerFell();
            ResetPosition();
        }

        // check if fell off
        // TODO more robust
        if (this.transform.position.y < -1.0)
        {
            _game.PlayerFell();
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        this.transform.position = new Vector3(-2f, 0.5f, -2f);
        this._rigidBody.angularVelocity = Vector3.zero;
        this._rigidBody.velocity = Vector3.zero;
    }

    private void ProcessInput()
    {
        if (!_isActive) { return; }

        float move_x = 0.0f;
        float move_z = 0.0f;

        /*
        if (Input.GetKey(KeyCode.UpArrow)){move_z = 1.0f;}
        if (Input.GetKey(KeyCode.DownArrow)){move_z = -1.0f;}
        if (Input.GetKey(KeyCode.RightArrow)) { move_x = 1.0f; }
        if (Input.GetKey(KeyCode.LeftArrow)) { move_x = -1.0f; }
        */

        move_x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        move_z = CrossPlatformInputManager.GetAxisRaw("Vertical");

        _rigidBody.AddForce(new Vector3(move_x, 0, move_z) * speed * Time.deltaTime);
    }
}
