using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlidePlayerState : PlayerState
{
  private bool _isGrounded;
  private bool _isTouchingWall;

  private bool attack;
  private bool jump;
  private Vector2 input;
  public WallSlidePlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
  {
  }

  public override void DoChecks()
  {
    base.DoChecks();
    _isGrounded = _playerController.CheckIfTouchGround();
    _isTouchingWall = _playerController.CheckIfTouchWall();
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
    if (input.x != 0 && !_isGrounded && _isTouchingWall && _playerController._currentVelocity.y < 0.0f)
    {
      _playerController.Slide();
    }
  }

  public override void Update()
  {
    base.Update();
    input = _playerController._playerInputHandler.movementInput;
    attack = _playerController._playerInputHandler.attack;
    jump = _playerController._playerInputHandler.jump;

    if (input.x == 0 && !_isGrounded)
    {
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._INAIR_));
    }

    if (_isGrounded && _playerController._currentVelocity.y < 0.01f)
    {
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._IDLE_));
    }

    if (attack)
    {
      _playerController._playerInputHandler.AttackButtonUsed();
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._ATTACK_));
    }

    if (jump)
    {
      _playerController._playerInputHandler.JumpButtonUsed();
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._JUMPING_));
    }
  }
}
