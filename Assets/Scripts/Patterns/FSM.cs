using System.Collections;
using System.Collections.Generic;

public class FSM
{
  public State currentState;
  protected Dictionary<int, State> _states = new Dictionary<int, State>();

  public FSM()
  {

  }

  public void Add(int key, State state)
  {
    _states[key] = state;
  }

  public void SetCurrentState(State state)
  {
    if (currentState != null)
    {
      currentState.Exit();
    }
    currentState = state;
    if (currentState != null)
    {
      currentState.Enter();
    }
  }

  public State GetState(int key)
  {
    return _states[key];
  }

  public void Update()
  {
    if (currentState != null)
    {
      currentState.Update();
    }
  }

  public void FixedUpdate()
  {
    if (currentState != null)
    {
      currentState.FixedUpdate();
    }
  }
}
