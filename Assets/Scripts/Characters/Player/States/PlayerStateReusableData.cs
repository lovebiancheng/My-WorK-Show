using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReusableData 
{
    public Vector2 movementInput {  get; set; }
    public float movementSpeedModifier { get; set; } = 1f;

    public bool shouldWalk { get; set; }

    private Vector3 currentTargetRotation;
    private Vector3 timeToReachTargetRotation;
    private Vector3 dampedTargetRotationcurrentVelocity;
    private Vector3 dampedTargetRotationPassedTime;
    public ref Vector3 CurrentTargetRotation
    {
        get
        {
            return ref currentTargetRotation;
        }
    }
    public ref Vector3 TimeToReachTargetRotation
    {
        get
        {
            return ref timeToReachTargetRotation;
        }
    }
    public ref Vector3 DampedTargetRotationcurrentVelocity
    {
        get
        {
            return ref dampedTargetRotationcurrentVelocity;
        }
    }
    public ref Vector3 DampedTargetRotationPassedTime
    {
        get
        {
            return ref dampedTargetRotationPassedTime;
        }
    }
}
