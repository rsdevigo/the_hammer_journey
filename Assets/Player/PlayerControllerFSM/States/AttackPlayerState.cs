using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerState : AbilityPlayerState
{
  private bool attackCheckIsActived;
  public AttackPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
  {
  }

  public override void DoChecks()
  {
    base.DoChecks();
    attackCheckIsActived = _playerController.CheckAttackPosition();
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
    if (attackCheckIsActived && !abilityIsDone)
    {
      abilityIsDone = true;
      _playerController.Attack();
    }
  }

  public override void Update()
  {
    base.Update();
  }
}
