using UnityEngine;

/// <summary>
/// The aim at player behaviour logic for the turret
/// This will make the turret aim directly at the player
/// </summary>
[CreateAssetMenu(fileName = "Turret Aim - Aim at Player", menuName = "Turret/Behaviour Logic/Aim/Aim at PLayer")]
public class TurretAimDirectToPlayer : TurretAimSOBase
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

    Vector3 aimAt = playerToAimAt.AimPoint.position;
    Vector3 direction = aimAt - transform.position;

    if (RotateTo(direction))
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