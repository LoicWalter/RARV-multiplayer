using UnityEngine;

/// <summary>
/// A generic state for the turret state machine
/// </summary>
public class TurretState
{
  protected Turret turret;
  protected TurretStateMachine turretStateMachine;

  public TurretState(Turret turret, TurretStateMachine turretStateMachine)
  {
    this.turret = turret;
    this.turretStateMachine = turretStateMachine;
  }

  public virtual void EnterState()
  {
    //Debug.Log("Entering state: " + this.GetType().Name);
  }

  public virtual void ExitState()
  {
    //Debug.Log("Exiting state: " + this.GetType().Name);
  }

  public virtual void FrameUpdate() { }

  public virtual void PhysicsUpdate() { }
}