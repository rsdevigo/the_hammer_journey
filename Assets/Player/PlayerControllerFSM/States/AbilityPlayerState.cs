using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPlayerState : PlayerState
{
  private bool _isGrounded;
  protected bool abilityIsDone;
  public AbilityPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
  {
  }

  public override void DoChecks()
  {
    base.DoChecks();
    _isGrounded = _playerController.CheckIfTouchGround();
  }

  public override void Enter()
  {
    base.Enter();
    abilityIsDone = false;
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
    if (abilityIsDone)
    {
      if (_isGrounded && _playerController._currentVelocity.y < 0.01f)
      {
        _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._IDLE_));
      }
      else
      {
        _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._INAIR_));
      }
    }
  }
}
