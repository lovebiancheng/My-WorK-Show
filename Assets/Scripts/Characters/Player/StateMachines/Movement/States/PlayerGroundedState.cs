using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region Resuable Methods
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        stateMachine.player.input.playerActions.Movement.canceled += OnMovementCanceled;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        stateMachine.player.input.playerActions.Movement.canceled -= OnMovementCanceled;
    }
    protected virtual void OnMove()
    {
        if (stateMachine.reusableData.shouldWalk)
        {
            stateMachine.ChangeState(stateMachine.walkingState);
        }
        stateMachine.ChangeState(stateMachine.runningState);
    }
    #endregion
    #region Input Methods

    protected void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.idlingState);
    }
    #endregion
}
