using UnityEngine;

[CreateAssetMenu(fileName = "Turret Aim - Aim at Player", menuName = "Turret/Behaviour Logic/Aim/Aim at PLayer")]
public class TurretAimDirectToPlayer : TurretAimSOBase
{
  [Header("Settings")]
  [SerializeField] private float rotationSpeed = 5f;
  [SerializeField] private float _attackCooldown = 2f;
  private float _timer = 0;
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
    if (playerToAimAt == null)
    {
      return;
    }

    Vector3 playerPosition = playerToAimAt.transform.position;
    Vector3 direction = playerPosition - transform.position;
    Quaternion lookRotation = Quaternion.LookRotation(direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

    //calculate the angle
    float angle = Vector3.Angle(transform.forward, direction);
    _timer += Time.deltaTime;
    if (angle < 1 && _timer >= _attackCooldown)
    {
      turret.StateMachine.ChangeState(turret.AttackState);
      _timer = 0;
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