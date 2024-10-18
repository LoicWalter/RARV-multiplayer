using UnityEngine;

/// <summary>
/// The idle state of the turret
/// This one is for when the turret is not moving at all
/// </summary>
[CreateAssetMenu(fileName = "Turret Idle - No Movement", menuName = "Turret/Behaviour Logic/Idle/No Movement")]
public class TurretIdleNoMovement : TurretIdleSOBase
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