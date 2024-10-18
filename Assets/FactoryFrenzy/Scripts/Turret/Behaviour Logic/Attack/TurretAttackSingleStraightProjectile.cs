using System.Collections;
using UnityEngine;

/// <summary>
/// The base class for the attack logic of the turret
/// This class is used to shoot a single straight projectile
/// </summary>
[CreateAssetMenu(fileName = "Turret Attack - Single Straight Projectile", menuName = "Turret/Behaviour Logic/Attack/Single Straight Projectile")]
public class TurretAttackSingleStraightProjectile : TurretAttackSOBase
{
  [Header("Settings")]
  [Tooltip("The bullet prefab to shoot.")]
  [SerializeField] private GameObject _bulletPrefab;

  [Tooltip("The time till the bullet is destroyed.")]
  [SerializeField] private float _timeTillDestroy = 5f;


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
    Shoot();
    turret.StateMachine.ChangeState(turret.AimState);
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

  /// <summary>
  ///  Shoots the bullet and destroys it after a certain time.
  /// </summary>
  public void Shoot()
  {
    GameObject bullet = Instantiate(_bulletPrefab, turret.BulletSpawnPoint.position, Quaternion.identity);
    var bulletRb = bullet.GetComponent<Rigidbody>();
    bulletRb.AddForce(bulletRb.mass * BulletSpeed * turret.BulletSpawnPoint.forward, ForceMode.Impulse);
    Destroy(bullet, _timeTillDestroy);
  }
}