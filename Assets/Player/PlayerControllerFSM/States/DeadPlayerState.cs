using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPlayerState : PlayerState
{
  public DeadPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
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
  }

  public override void Update()
  {
    base.Update();
    if (!_playerController.isDead)
    {
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._IDLE_));
    }
  }
}
