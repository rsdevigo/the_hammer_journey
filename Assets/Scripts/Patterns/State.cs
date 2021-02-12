using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
  protected FSM _fsm;

  public State(FSM fsm)
  {
    _fsm = fsm;
  }

  public virtual void Enter()
  {

  }

  public virtual void Exit()
  {

  }

  public virtual void Update()
  {

  }

  public virtual void FixedUpdate()
  {

  }
}
