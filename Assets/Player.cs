using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 10;
    public GameObject Target;

    private Rigidbody rigidBody;
    private TargetHandler targetHandler;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        targetHandler = Target.GetComponent<TargetHandler>();
    }
	
	void Update ()
    {
        ProcessInput();
        ProcessPosition();
    }

    private void ProcessPosition()
    {
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.transform.position);
        // reached target
        if (distanceToTarget < 1.2f)
        {
            targetHandler.NewPosition();
        }
        // check if fell off
        if (this.transform.position.y < -1.0)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this.rigidBody.angularVelocity = Vector3.zero;
        this.rigidBody.velocity = Vector3.zero;
    }

    private void ProcessInput()
    {
        float move_x = 0.0f;
        float move_z = 0.0f;

        if (Input.GetKey(KeyCode.UpArrow)){move_z = 1.0f;}
        if (Input.GetKey(KeyCode.DownArrow)){move_z = -1.0f;}
        if (Input.GetKey(KeyCode.RightArrow)) { move_x = 1.0f; }
        if (Input.GetKey(KeyCode.LeftArrow)) { move_x = -1.0f; }

        rigidBody.AddForce(new Vector3(move_x, 0, move_z) * speed * Time.deltaTime);
    }
}
