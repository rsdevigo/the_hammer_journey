using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
  protected PlayerController _playerController;
  protected string _animatorBool;
  protected float _startTime;
  public PlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm)
  {
    _playerController = playerController;
    _animatorBool = animatorBool;
  }

  public override void Enter()
  {
    DoChecks();
    _startTime = Time.time;
    _playerController._animator.SetBool(_animatorBool, true);
    base.Enter();
  }

  public override void Exit()
  {
    _playerController._animator.SetBool(_animatorBool, false);
    base.Exit();
  }

  public override void Update()
  {
    base.Update();
  }

  public override void FixedUpdate()
  {
    DoChecks();
    base.FixedUpdate();
  }

  public virtual void DoChecks()
  {

  }
}
