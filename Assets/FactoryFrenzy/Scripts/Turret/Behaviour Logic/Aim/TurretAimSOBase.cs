using UnityEngine;

/// <summary>
/// The base class for the aim state of the turret
/// </summary>
public class TurretAimSOBase : ScriptableObject
{
  protected Turret turret;
  protected Transform transform;
  protected GameObject gameObject;
  protected PlayerController playerToAimAt;
  private float _timer = 0;


  public virtual void Initialize(GameObject gameObject, Turret turret)
  {
    this.gameObject = gameObject;
    this.turret = turret;
    transform = gameObject.transform;
  }

  public virtual void DoEnterLogic()
  {
    turret.LineOfSight.enabled = true;
  }

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
    else if (turret.PlayersInRange.Count == 0)
    {
      turret.StateMachine.ChangeState(turret.IdleState);
    }
  }

  public virtual void DoPhysicsLogic() { }



  public virtual void ResetValues()
  {
    turret.LineOfSight.enabled = false;
  }


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
      playerToAimAt = turret.PlayersInRange[0];
      return true;
    }
    playerToAimAt = null;
    return false;
  }

  /// <summary>
  ///  Sets the line of sight from the turret to the current aim direction.
  /// </summary>
  /// <param name="direction"></param>
  public virtual void SetLineOfSight(Vector3 direction)
  {
    turret.LineOfSight.SetPosition(0, turret.BulletSpawnPoint.position);
    turret.LineOfSight.SetPosition(1, turret.BulletSpawnPoint.position + direction * turret.DetectionRange);
  }

  /// <summary>
  ///  Triggers the shot when the timer is greater than the attack cooldown.
  /// </summary>
  /// <param name="currentPosition"></param>
  /// <param name="targetPosition"></param>
  public virtual void TriggerShot(Vector3 currentPosition, Vector3 targetPosition)
  {
    _timer += Time.deltaTime;
    if (_timer >= turret.TurretAttackBaseInstance.AttackCooldown)
    {
      turret.StateMachine.ChangeState(turret.AttackState);
      _timer = 0;
    }
  }
}