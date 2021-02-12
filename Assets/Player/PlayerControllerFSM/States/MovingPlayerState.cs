using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayerState : GroundedPlayerState
{
  public MovingPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
  {
  }

  public override void DoChecks()
  {
    base.DoChecks();
  }

  public override void Enter()
  {
    base.Enter();
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();
    _playerController.Move(input.x * Time.fixedDeltaTime);
  }

  public override void Update()
  {
    base.Update();
    if (input.x == 0f)
    {
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._IDLE_));
    }
  }
}
