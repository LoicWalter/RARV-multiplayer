using UnityEngine;

[CreateAssetMenu(fileName = "Turret Aim - PreShot Player", menuName = "Turret/Behaviour Logic/Aim/PreShot PLayer")]
public class TurretAimPreshotPlayer : TurretAimSOBase
{
  [Header("Settings")]
  [Tooltip("The speed of the rotation.")]
  [SerializeField] private float rotationSpeed = 5f;

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
    var playerMovementDirection = playerToAimAt.GetComponent<Rigidbody>().velocity;

    float distance = Vector3.Distance(transform.position, playerPosition);
    float timeToHit = distance / turret.TurretAttackBaseInstance.BulletSpeed;
    Vector3 predictedPosition = playerPosition + playerMovementDirection * timeToHit;

    Vector3 direction = predictedPosition - transform.position;

    Quaternion lookRotation = Quaternion.LookRotation(direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

    //calculate the angle

    float angle = Vector3.Angle(transform.forward, direction);
    _timer += Time.deltaTime;
    if (angle < 1 && _timer >= turret.TurretAttackBaseInstance.AttackCooldown)
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