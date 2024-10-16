using UnityEngine;

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

  public override void AnimationTriggerEvent(Turret.AnimationTriggerType animationTriggerType)
  {
    base.AnimationTriggerEvent(animationTriggerType);
    turret.TurretIdleBaseInstance.DoAnimationTriggerEventLogic(animationTriggerType);
  }
}