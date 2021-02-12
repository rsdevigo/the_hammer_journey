using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlayerState : AbilityPlayerState
{
  public JumpingPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
  {
  }

  public override void DoChecks()
  {
    base.DoChecks();
  }

  public override void Enter()
  {
    base.Enter();
    _playerController.Jump();
    abilityIsDone = true;
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
  }
}
