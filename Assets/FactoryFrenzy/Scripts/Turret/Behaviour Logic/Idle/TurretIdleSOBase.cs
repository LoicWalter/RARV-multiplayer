using UnityEngine;

public class TurretIdleSOBase : ScriptableObject
{
  [Header("Settings")]
  protected Turret turret;
  protected Transform transform;
  protected GameObject gameObject;


  public virtual void Initialize(GameObject gameObject, Turret turret)
  {
    this.gameObject = gameObject;
    this.turret = turret;
    transform = gameObject.transform;
  }

  public virtual void DoEnterLogic() { }

  public virtual void DoExitLogic()
  {
    ResetValues();
  }

  public virtual void DoFrameLogic()
  {
    if (turret.PlayersInRange != null && turret.PlayersInRange.Count > 0)
    {
      turret.StateMachine.ChangeState(turret.AimState);
    }
  }

  public virtual void DoPhysicsLogic() { }

  public virtual void DoAnimationTriggerEventLogic(Turret.AnimationTriggerType animationTriggerType) { }

  public virtual void ResetValues() { }
}