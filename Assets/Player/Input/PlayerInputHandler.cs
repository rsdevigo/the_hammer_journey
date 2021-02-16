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

  private float jumpStartTime;
  private float jumpHoldTime = 0.2f;

  [SerializeField] public float startTimeBtwAttack = 1.0f;
  [SerializeField] private float timeBtwAttack = 0.0f;

  [SerializeField] public float startTimeBtwBlock = 1.0f;
  [SerializeField] private float timeBtwBlock = 0.0f;

  void Update()
  {
    if (Time.time > jumpStartTime + jumpHoldTime)
    {
      jump = false;
    }

    timeBtwAttack -= Time.deltaTime;
    timeBtwBlock -= Time.deltaTime;
  }

  public void OnJumpInput(InputAction.CallbackContext context)
  {
    if (context.started)
    {
      jump = true;
      jumpStartTime = Time.time;
    }
  }

  public void OnAttackInput(InputAction.CallbackContext context)
  {
    if (context.started && timeBtwAttack <= 0.0f)
    {
      attack = true;
    }
  }

  public void OnBlockInput(InputAction.CallbackContext context)
  {
    if (context.started && timeBtwBlock <= 0.0f)
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

  public void AttackButtonUsed()
  {
    attack = false;
    timeBtwAttack = startTimeBtwAttack;
  }

  public void BlockButtonUsed()
  {
    block = false;
    timeBtwBlock = startTimeBtwBlock;
  }
}
