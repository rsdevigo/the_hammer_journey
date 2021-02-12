using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
  public bool jump { get; private set; }
  public bool attack { get; private set; }
  public bool block { get; private set; }
  public Vector2 movementInput { get; private set; }

  public void OnJumpInput(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      jump = true;
    }
  }

  public void OnAttackInput(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      attack = true;
    }
  }

  public void OnBlockInput(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      block = true;
    }
  }

  public void OnMoveInput(InputAction.CallbackContext context)
  {
    movementInput = context.ReadValue<Vector2>();
  }

  public void JumpButtonUsed()
  {
    jump = false;
  }
}
