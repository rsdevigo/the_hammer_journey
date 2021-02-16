using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlayerState : PlayerState
{
  private bool _isGrounded;
  private bool _isTouchingWall;
  private bool attack;
  protected Vector2 input;
  public AirPlayerState(PlayerStateMachine fsm, PlayerController playerController, string animatorBool) : base(fsm, playerController, animatorBool)
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
    if (!_isGrounded)
    {
      _playerController.Move(input.x * Time.fixedDeltaTime);
    }
  }

  public override void Update()
  {
    base.Update();
    input = _playerController._playerInputHandler.movementInput;
    attack = _playerController._playerInputHandler.attack;
    if (attack)
    {
      _playerController._playerInputHandler.AttackButtonUsed();
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._ATTACK_));
    }
    else if (_isGrounded && _playerController._currentVelocity.y < 0.01f)
    {
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._IDLE_));
    }
    else if (!_isGrounded && _isTouchingWall && input.x != 0 && _playerController._currentVelocity.y < 0.0f)
    {
      _fsm.SetCurrentState(_fsm.GetState((int)PlayerStatesEnum._WALLSLIDING_));
    }
  }
}
