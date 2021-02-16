using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlayerState : AbilityPlayerState
{
  private float blockDuration = 0.4f;
  public BlockPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
  {
  }

  public override void DoChecks()
  {
    base.DoChecks();
  }

  public override void Enter()
  {
    base.Enter();
    _playerController.isBlocking = true;
  }

  public override void Exit()
  {
    base.Exit();
    _playerController.isBlocking = false;
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();
    if (Time.time > _startTime + blockDuration)
    {
      abilityIsDone = true;
    }
  }

  public override void Update()
  {
    base.Update();
  }

}
