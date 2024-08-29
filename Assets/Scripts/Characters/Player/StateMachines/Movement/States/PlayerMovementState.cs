using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine stateMachine;

    protected PlayerGroundedData movementData;

    

    protected float baseSpeed = 5f;
    

    

    


    public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        stateMachine = playerMovementStateMachine;
        movementData = stateMachine.player.data.groundedData;
        InitialzeData();

    }
    private void InitialzeData()
    {
        stateMachine.reusableData.TimeToReachTargetRotation = movementData.baseRotationData.targertRotationReachTime;

    }



    #region IState Methods
    public virtual  void Enter()
    {
        Debug.Log("State:" + GetType().Name);
        AddInputActionsCallbacks();
    }


    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    public virtual void Update()
    {
    }
#endregion

    #region Main Methods
    private void ReadMovementInput()
    {
        stateMachine.reusableData.movementInput = stateMachine.player.input.playerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        if(stateMachine.reusableData.movementInput ==Vector2.zero|| stateMachine.reusableData.movementSpeedModifier ==0f)////
        {
            return;
        }
        Vector3 movementDirection=GetMovementInputDirection();

        float targetRotationYAngle = Rotate(movementDirection);

        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        float movementSpeed=GetMovementSpeed();

        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        stateMachine.player.GetComponent<Rigidbody>().AddForce(targetRotationDirection * movementSpeed-currentPlayerHorizontalVelocity, ForceMode.VelocityChange);//�������Բ��ÿ������������mass�����ң�����ʹ��addforce
    }

    

    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);/*Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;//弧度转为角度*/
        //directionAngle = AddCameraRotationToAngle(directionAngle);
        //if(directionAngle !=currentTargetRotation.y)
        //{
        //    UpdateTargetRotationData(directionAngle);
        //}

        RotateTowardsTargetRotation();

        return directionAngle;
    }

    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle=Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg;
        if(directionAngle < 0)
        {
            directionAngle += 360f;
        }
        return directionAngle;
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.reusableData.CurrentTargetRotation.y = targetAngle;
        stateMachine.reusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.player.mainCameraTransform.eulerAngles.y;//这里有些不大清楚
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return angle;
    }
    #endregion

    #region Reusable Methods
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.reusableData.movementInput.x, 0f, stateMachine.reusableData.movementInput.y);
    }

    protected float GetMovementSpeed()
    {
        return movementData.baseSpeed * stateMachine.reusableData.movementSpeedModifier;
    }
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity=stateMachine.player.GetComponent<Rigidbody>().velocity;
        playerHorizontalVelocity.y = 0f;
        return playerHorizontalVelocity;
    }
    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.player.rigidbody.rotation.eulerAngles.y;
        if(currentYAngle== stateMachine.reusableData.CurrentTargetRotation.y)
        {
            return;//                                                                                                                              DampedTargetRotationcurrentVelocity
        }
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.reusableData.CurrentTargetRotation.y, ref stateMachine.reusableData.DampedTargetRotationcurrentVelocity.y, stateMachine.reusableData.TimeToReachTargetRotation.y - stateMachine.reusableData.DampedTargetRotationPassedTime.y);//////
        stateMachine.reusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;
        Quaternion targetRotation=Quaternion.Euler(0f,smoothedYAngle,0f);
        stateMachine.player.rigidbody.MoveRotation(targetRotation);
    }
    protected float UpdateTargetRotation(Vector3 direction,bool shouldConsiderCamerRotation=true)
    {
        float directionAngle = GetDirectionAngle(direction);
        if(shouldConsiderCamerRotation)
        {
            directionAngle=AddCameraRotationToAngle(directionAngle);
        }
        

        if (directionAngle != stateMachine.reusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;//四元数和向量相乘可以表示这个向量按照这个四元数旋转之后得到的新的向量
    }

    protected void ResetVelocity()
    {
        stateMachine.player.rigidbody.velocity= Vector3.zero;
    }


    //下面两个就是事件订阅和取消
    protected virtual void AddInputActionsCallbacks()
    {
        stateMachine.player.input.playerActions.WalkToggle.started += OnWalkToggleStarted;
    }

    

    protected virtual void RemoveInputActionsCallbacks()
    {
        stateMachine.player.input.playerActions.WalkToggle.started -= OnWalkToggleStarted;
    }


    #endregion

    #region Input Methods
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        stateMachine.reusableData.shouldWalk = !stateMachine.reusableData.shouldWalk;
    }


    #endregion



}
