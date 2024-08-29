using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
   public Player player { get; }
   public PlayerStateReusableData reusableData { get; }
   public PlayerIdlingState idlingState { get; }
   public PlayerWalkingState walkingState { get; }
   public PlayerRunningState runningState { get; }
   public PlayerSpritingState spritingState { get; }
   
    public PlayerMovementStateMachine(Player player1)
    {
        player = player1;
        reusableData = new PlayerStateReusableData();
        idlingState = new PlayerIdlingState(this);
        walkingState = new PlayerWalkingState(this);
        runningState = new PlayerRunningState(this);
        spritingState = new PlayerSpritingState(this);
    }
}
