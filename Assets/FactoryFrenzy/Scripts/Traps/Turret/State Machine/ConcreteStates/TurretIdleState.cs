using UnityEngine;

/// <summary>
/// The idle state of the turret
/// </summary>
public class TurretIdleState : TurretState
{

  public TurretIdleState(Turret turret, TurretStateMachine turretStateMachine) : base(turret, turretStateMachine) { }

  public override void EnterState()
  {
    base.EnterState();
    turret.TurretIdleBaseInstance.DoEnterLogic();
  }

  public override void ExitState()
  {
    base.ExitState();
    turret.TurretIdleBaseInstance.DoExitLogic();
  }

  public override void FrameUpdate()
  {
    base.FrameUpdate();
    turret.TurretIdleBaseInstance.DoFrameLogic();


  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
    turret.TurretIdleBaseInstance.DoPhysicsLogic();
  }
}