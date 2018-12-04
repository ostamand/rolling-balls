using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerTrainedAgent : Agent
{
    
    public GameObject Target;
    public float speed = 10;

    Rigidbody rBody;
    private TargetHandler targetHandler;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        targetHandler = Target.GetComponent<TargetHandler>();
    }

    public override void AgentReset()
    {
        if (this.transform.position.y < -1.0)
        {
            // the Agent fell
            ResetPosition();
        }
        else
        {
            targetHandler.NewPosition();
        }
    }

    private void ResetPosition()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
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
        AddVectorObs(rBody.velocity.x / 5);
        AddVectorObs(rBody.velocity.z / 5);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.transform.position);

        // reached target
        if (distanceToTarget < 1.2f)
        {
            AddReward(2.0f);
            targetHandler.NewPosition();
        }

        // time penalty
        AddReward(-0.05f);

        // distance penality
        AddReward(distanceToTarget / 11.0f * -0.50f);

        // fell off platform
        if (this.transform.position.y < -1.0)
        {
            AddReward(-10.0f);
            ResetPosition();
        }

        // actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);
    }
}
