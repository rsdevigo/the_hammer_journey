using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayerState : AbilityPlayerState
{
  public HitPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
  {
  }

  public override void DoChecks()
  {
    base.DoChecks();
  }

  public override void Enter()
  {
    base.Enter();
    _playerController.isHited = false;
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
