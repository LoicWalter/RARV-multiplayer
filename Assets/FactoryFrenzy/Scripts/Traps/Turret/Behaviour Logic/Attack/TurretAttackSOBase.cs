using System.Collections;
using UnityEngine;

/// <summary>
/// The base class for the turret attack logic
/// </summary>
public class TurretAttackSOBase : ScriptableObject
{
  [Header("Settings")]
  protected Turret turret;
  protected Transform transform;
  protected GameObject gameObject;

  [Tooltip("The speed of the bullet.")]
  [field: SerializeField] public float BulletSpeed { get; set; } = 10f;

  [Tooltip("The time between attacks.")]
  [field: SerializeField] public float AttackCooldown { get; set; } = 2f;

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
    turret.Animator.SetTrigger("Shoot");
    turret.TurretAudioSource.Play();
    Shoot();
    turret.StateMachine.ChangeState(turret.AimState);
    turret.StartCoroutine(WaitTillResetAnimator());
  }

  public virtual void DoPhysicsLogic() { }

  public virtual void ResetValues()
  {
  }

  public virtual void Shoot() { }

  private IEnumerator WaitTillResetAnimator()
  {
    yield return new WaitForSeconds(AttackCooldown / 2);
    turret.Animator.ResetTrigger("Shoot");
  }
}