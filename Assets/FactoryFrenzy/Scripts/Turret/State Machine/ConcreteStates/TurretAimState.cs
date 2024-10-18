using UnityEngine;

/// <summary>
/// The aim state of the turret
/// </summary>
public class TurretAimState : TurretState
{
  public TurretAimState(Turret turret, TurretStateMachine turretStateMachine) : base(turret, turretStateMachine) { }

  public override void EnterState()
  {
    base.EnterState();
    turret.TurretAimBaseInstance.DoEnterLogic();
  }

  public override void ExitState()
  {
    base.ExitState();
    turret.TurretAimBaseInstance.DoExitLogic();
  }

  public override void FrameUpdate()
  {
    base.FrameUpdate();
    turret.TurretAimBaseInstance.DoFrameLogic();


  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
    turret.TurretAimBaseInstance.DoPhysicsLogic();
  }
}