using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedPlayerState : PlayerState
{
  private bool _isGrounded;
  private bool jump;
  protected Vector2 input;
  public GroundedPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
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
    input = _playerController._playerInputHandler.movementInput;
    jump = _playerController._playerInputHandler.jump;

    if (jump)
    {
      _playerController._playerInputHandler.JumpButtonUsed();
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._JUMPING_));
    }

    if (!_isGrounded)
    {
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._INAIR_));
    }
  }
}
