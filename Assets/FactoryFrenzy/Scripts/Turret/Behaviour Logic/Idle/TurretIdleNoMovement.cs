using UnityEngine;

[CreateAssetMenu(fileName = "Turret Idle - No Movement", menuName = "Turret/Behaviour Logic/Idle/No Movement")]
public class TurretIdleNoMovement : TurretIdleSOBase
{

  public override void DoAnimationTriggerEventLogic(Turret.AnimationTriggerType animationTriggerType)
  {
    base.DoAnimationTriggerEventLogic(animationTriggerType);
  }

  public override void DoEnterLogic()
  {
    base.DoEnterLogic();

  }

  public override void DoExitLogic()
  {
    base.DoExitLogic();
  }

  public override void DoFrameLogic()
  {
    base.DoFrameLogic();
  }

  public override void DoPhysicsLogic()
  {
    base.DoPhysicsLogic();
  }

  public override void ResetValues()
  {
    base.ResetValues();
  }

  public override void Initialize(GameObject gameObject, Turret turret)
  {
    base.Initialize(gameObject, turret);
  }

}