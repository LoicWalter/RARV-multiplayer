using UnityEngine;

public class TurretAttackState : TurretState
{

  public TurretAttackState(Turret turret, TurretStateMachine turretStateMachine) : base(turret, turretStateMachine) { }

  public override void EnterState()
  {
    base.EnterState();
    turret.TurretAttackBaseInstance.DoEnterLogic();
  }

  public override void ExitState()
  {
    base.ExitState();
    turret.TurretAttackBaseInstance.DoExitLogic();
  }

  public override void FrameUpdate()
  {
    base.FrameUpdate();
    turret.TurretAttackBaseInstance.DoFrameLogic();


  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
    turret.TurretAttackBaseInstance.DoPhysicsLogic();
  }

  public override void AnimationTriggerEvent(Turret.AnimationTriggerType animationTriggerType)
  {
    base.AnimationTriggerEvent(animationTriggerType);
    turret.TurretAttackBaseInstance.DoAnimationTriggerEventLogic(animationTriggerType);
  }
}