using UnityEngine;

/// <summary>
/// The aim state of the turret
/// This scriptable object is used to aim the turret at the player, but we anticipate the player's movement
/// </summary>
[CreateAssetMenu(fileName = "Turret Aim - Lead Targeting", menuName = "Turret/Behaviour Logic/Aim/Lead Targeting")]
public class TurretAimLeadTargeting : TurretAimSOBase
{
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
    if (playerToAimAt == null)
    {
      return;
    }

    Vector3 playerPosition = playerToAimAt.AimPoint.position;
    Vector3 playerVelocity = playerToAimAt.Rb.velocity;

    // Calculating the direction to the player
    Vector3 direction = playerPosition - transform.position;

    // Calculating the time to target
    float distance = direction.magnitude;
    float timeToTarget = distance / turret.TurretAttackBaseInstance.BulletSpeed;

    // Calculating the future position of the player
    Vector3 futurePosition = playerPosition + playerVelocity * timeToTarget;

    // Calculating the direction to the future position
    Vector3 leadDirection = futurePosition - transform.position;

    if (RotateTo(leadDirection))
    {
      TriggerShot();
    }
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