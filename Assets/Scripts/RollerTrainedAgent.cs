using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Assets.Scripts.Common;

public class RollerTrainedAgent : Agent
{
    
    public GameObject Target;
    public float speed = 10;

    private Rigidbody _rigidBody;
    private TargetHandler _target;
    private GameController _game;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _target = Target.GetComponent<TargetHandler>();
        _game = FindObjectOfType<GameController>();
    }

    public override void AgentReset()
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this._rigidBody.angularVelocity = Vector3.zero;
        this._rigidBody.velocity = Vector3.zero;
    }

    public override void CollectObservations()
    {
        Vector3 relativePosition = Target.transform.position - this.transform.position;

        // relative position target VS ball
        AddVectorObs(relativePosition.x / 5);
        AddVectorObs(relativePosition.z / 5);

        // distance to edge of platform 
        AddVectorObs((this.transform.position.x + 5) / 5);
        AddVectorObs((this.transform.position.x - 5) / 5);
        AddVectorObs((this.transform.position.z + 5) / 5);
        AddVectorObs((this.transform.position.z - 5) / 5);

        // velocity
        AddVectorObs(_rigidBody.velocity.x / 5);
        AddVectorObs(_rigidBody.velocity.z / 5);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // update position wrt target
        _target.UpdatePosition(this.transform.position, TypeOf.Agent);
       
        // fell off platform
        // TODO more robust
        if (this.transform.position.y < -1.0)
        {
            _game.AgentFell();
            ResetPosition();
        }

        // actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        _rigidBody.AddForce(controlSignal * speed);
    }
}
