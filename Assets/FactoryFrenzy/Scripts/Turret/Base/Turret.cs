using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The turret class
/// It is responsible for the behavior of the turret
/// </summary>
public class Turret : MonoBehaviour
{
  public Rigidbody Rb { get; set; }

  #region State Machine Variables

  public TurretStateMachine StateMachine { get; set; }
  public TurretIdleState IdleState { get; set; }
  public TurretAttackState AttackState { get; set; }
  public TurretAimState AimState { get; set; }

  #endregion

  #region ScriptableObject Variables

  [Header("Settings")]
  [Tooltip("Turret bullet spawn point.")]
  public Transform BulletSpawnPoint;

  [Tooltip("The behavior of the turret when it is idle.")]
  [SerializeField] private TurretIdleSOBase TurretIdleBase;
  [Tooltip("The behavior of the turret when it is attacking.")]
  [SerializeField] private TurretAttackSOBase TurretAttackBase;
  [Tooltip("The behavior of the turret when it is aiming.")]
  [SerializeField] private TurretAimSOBase TurretAimBase;

  public TurretIdleSOBase TurretIdleBaseInstance { get; set; }
  public TurretAttackSOBase TurretAttackBaseInstance { get; set; }
  public TurretAimSOBase TurretAimBaseInstance { get; set; }

  #endregion

  #region Target Detections

  private PlayersDetector _playersDetector;
  [Tooltip("The players in range of the turret.")]
  public List<PlayerController> PlayersInRange => _playersDetector.PlayersInRange;
  public float DetectionRange => _playersDetector.DetectionRange;

  #endregion

  #region Aesthetics

  [Header("Aesthetics")]
  [Tooltip("The turret's line of sight.")]
  public LineRenderer LineOfSight;

  #endregion

  private void Awake()
  {
    InitializeStateMachine();
    _playersDetector = GetComponentInChildren<PlayersDetector>();
  }

  private void Start()
  {
    Rb = GetComponent<Rigidbody>();

    TurretIdleBaseInstance.Initialize(gameObject, this);
    TurretAttackBaseInstance.Initialize(gameObject, this);
    TurretAimBaseInstance.Initialize(gameObject, this);

    StateMachine.Initialize(IdleState);
  }

  private void Update()
  {
    StateMachine.CurrentState.FrameUpdate();
  }

  private void FixedUpdate()
  {
    StateMachine.CurrentState.PhysicsUpdate();
  }

  #region Initializations

  private void InitializeStateMachine()
  {
    TurretIdleBaseInstance = Instantiate(TurretIdleBase);
    TurretAttackBaseInstance = Instantiate(TurretAttackBase);
    TurretAimBaseInstance = Instantiate(TurretAimBase);

    StateMachine = new TurretStateMachine();

    IdleState = new TurretIdleState(this, StateMachine);
    AttackState = new TurretAttackState(this, StateMachine);
    AimState = new TurretAimState(this, StateMachine);
  }

  #endregion
}