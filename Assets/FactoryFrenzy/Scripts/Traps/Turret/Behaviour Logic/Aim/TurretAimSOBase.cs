using UnityEngine;

/// <summary>
/// The base class for the aim state of the turret
/// </summary>
public class TurretAimSOBase : ScriptableObject
{
  [Header("Settings")]
  [Tooltip("Whether to show the line of sight.")]
  [SerializeField] private bool _showLignOfSight = false;

  [Tooltip("The rotation speed of the turret.")]
  [SerializeField] private float _rotationSpeed = 2f;
  [Tooltip("The angle tolerance of the turret.")]
  [Range(0, 20)]
  [SerializeField] private float _angleTolerance = 5f;

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
    turret.LineOfSight.enabled = _showLignOfSight;
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
  public void SetLineOfSight(Vector3 direction)
  {
    turret.LineOfSight.SetPosition(0, turret.BulletSpawnPoint.position);
    turret.LineOfSight.SetPosition(1, turret.BulletSpawnPoint.position + direction * turret.DetectionRange);
  }

  /// <summary>
  ///  Triggers the shot when the timer is greater than the attack cooldown.
  /// </summary>
  protected void TriggerShot()
  {
    _timer += Time.deltaTime;
    if (_timer >= turret.TurretAttackBaseInstance.AttackCooldown)
    {
      turret.StateMachine.ChangeState(turret.AttackState);
      _timer = 0;
    }
  }

  protected bool RotateTo(Vector3 requestedDirection)
  {
    Quaternion targetRotation = Quaternion.LookRotation(requestedDirection);
    Quaternion newTurretBaseRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    transform.rotation = Quaternion.Slerp(transform.rotation, newTurretBaseRotation, _rotationSpeed * Time.deltaTime);

    // for the turret cannon, only rotate up and down
    Quaternion newTurretCannonRotation = Quaternion.Euler(0, targetRotation.eulerAngles.x, 0);
    turret.TurretCannon.transform.localRotation = Quaternion.Slerp(turret.TurretCannon.transform.localRotation, newTurretCannonRotation, _rotationSpeed * Time.deltaTime);

    SetLineOfSight(turret.TurretCannon.transform.right);

    //check the angle between the current rotation and the requested rotation on the x and z axis
    Vector3 currentRotation = transform.rotation.eulerAngles;
    currentRotation.y = 0;
    Vector3 requestedRotation = newTurretBaseRotation.eulerAngles;
    requestedRotation.y = 0;

    float angle = Vector3.Angle(currentRotation, requestedRotation);

    return angle < _angleTolerance;
  }
}