using UnityEngine;

/// <summary>
/// The state machine for the turret
/// </summary>
public class TurretStateMachine
{
  public TurretState CurrentState { get; private set; }

  public void Initialize(TurretState startingState)
  {
    CurrentState = startingState;
    CurrentState.EnterState();
  }

  /// <summary>
  /// Change the state of the turret
  /// </summary>
  /// <param name="newState"></param>
  public void ChangeState(TurretState newState)
  {
    CurrentState.ExitState();
    CurrentState = newState;
    CurrentState.EnterState();
  }
}