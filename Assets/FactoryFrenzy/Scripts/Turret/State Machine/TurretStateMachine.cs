using UnityEngine;

public class TurretStateMachine
{
  public TurretState CurrentState { get; private set; }

  public void Initialize(TurretState startingState)
  {
    CurrentState = startingState;
    CurrentState.EnterState();
  }

  public void ChangeState(TurretState newState)
  {
    CurrentState.ExitState();
    CurrentState = newState;
    CurrentState.EnterState();
  }
}