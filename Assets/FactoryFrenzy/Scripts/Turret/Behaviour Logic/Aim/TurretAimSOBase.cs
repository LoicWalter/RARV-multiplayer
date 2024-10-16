using UnityEngine;

public class TurretAimSOBase : ScriptableObject
{
  [Header("Settings")]
  protected Turret turret;
  protected Transform transform;
  protected GameObject gameObject;
  protected GameObject playerToAimAt;


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
    if (SetPlayerToAimAt())
    {
      turret.StateMachine.ChangeState(turret.AimState);
      return;
    }
  }

  public virtual void DoPhysicsLogic() { }

  public virtual void DoAnimationTriggerEventLogic(Turret.AnimationTriggerType animationTriggerType) { }

  public virtual void ResetValues() { }


  /// <summary>
  ///   Sets the player to aim at.
  /// </summary>
  /// <returns>
  ///   <c>true</c>, if player to aim at was set, <c>false</c> otherwise.
  /// </returns>
  protected bool SetPlayerToAimAt()
  {
    if (turret.PlayersInRange != null && turret.PlayersInRange.Count > 0)
    {
      playerToAimAt = turret.PlayersInRange[0].gameObject;
      return true;
    }
    playerToAimAt = null;
    return false;
  }
}