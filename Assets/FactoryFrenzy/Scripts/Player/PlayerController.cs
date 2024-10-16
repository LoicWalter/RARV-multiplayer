using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IMovable
{
  #region Variables

  [Header("Debug")]
  [Tooltip("Whether to draw gizmos in the scene view.")]
  [SerializeField] private bool DrawGizmos = true;

  [Header("Settings"), Space(5)]
  private Vector3 _moveDirection;
  [Header("Movement")]
  [Tooltip("The speed at which the player moves when running.")]
  [SerializeField] private float RunSpeed = 10f;
  [Tooltip("The speed at which the player moves when walking.")]
  [SerializeField] private float WalkSpeed = 5f;
  [Tooltip("Whether the player is running or walking.")]
  [SerializeField] private bool IsRunning = false;

  [field: SerializeField, Tooltip("The speed at which the player rotates their view.")]
  public float LookSpeed { get; private set; } = 2f;

  [field: SerializeField, Tooltip("The force applied to the player when they jump.")]
  public float JumpForce { get; private set; } = 5f;

  [field: SerializeField, Header("Ground Check"), Tooltip("Whether the player is grounded or not.")]
  public bool IsGrounded { get; private set; } = false;

  [Tooltip("The radius of the sphere used to check if the player is grounded.")]
  [SerializeField] private float GroundedRadius = 0.03f;

  [field: SerializeField, Tooltip("The layer mask used to check if the player is grounded.")]
  public LayerMask GroundLayer { get; private set; }
  public Rigidbody Rb { get; private set; }
  public float Speed { get => IsRunning ? RunSpeed : WalkSpeed; }
  public bool IsMoving { get => Rb.velocity.magnitude > 0.1f; }

  #endregion

  #region Input Actions

  public PlayerInputActions InputActions { get; private set; }
  private InputAction MoveAction => InputActions.Player.Move;
  private InputAction RunAction => InputActions.Player.Run;
  private InputAction LookAction => InputActions.Player.Look;
  private InputAction JumpAction => InputActions.Player.Jump;

  #endregion

  #region Unity Methods

  private void Awake()
  {
    Rb = GetComponent<Rigidbody>();
    InputActions = new PlayerInputActions();
    InputActions.Enable();

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  private void Update()
  {
    GroundedCheck();
    _moveDirection = new Vector3(MoveAction.ReadValue<Vector2>().x, 0, MoveAction.ReadValue<Vector2>().y);
  }

  private void FixedUpdate()
  {
    Move(_moveDirection);
  }

  private void OnEnable()
  {
    RunAction.performed += OnRunPerformed;
    JumpAction.performed += OnJumpPerformed;
    LookAction.performed += OnLookPerformed;
  }

  private void OnDisable()
  {
    RunAction.performed -= OnRunPerformed;
    JumpAction.performed -= OnJumpPerformed;
    LookAction.performed -= OnLookPerformed;
  }

  private void OnDrawGizmos()
  {
    if (!DrawGizmos) return;
    Gizmos.color = IsGrounded ? Color.green : Color.red;
    Gizmos.DrawWireSphere(transform.position, GroundedRadius);
  }

  #endregion

  #region Input Methods

  private void OnRunPerformed(InputAction.CallbackContext context)
  {
    IsRunning = context.ReadValueAsButton();
  }

  private void OnJumpPerformed(InputAction.CallbackContext context)
  {
    Jump();
  }

  private void OnLookPerformed(InputAction.CallbackContext context)
  {
    Rotate(context.ReadValue<Vector2>());
  }

  #endregion

  #region IMovable Methods

  public void GroundedCheck()
  {
    IsGrounded = Physics.CheckSphere(transform.position, GroundedRadius, GroundLayer);
  }

  public void Jump()
  {
    if (!IsGrounded) return;
    Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
  }

  public void Rotate(Vector3 rotation)
  {
    transform.Rotate(Vector3.up, rotation.x * LookSpeed);
  }

  public void Move(Vector3 direction)
  {
    float _targetSpeed = Speed * direction.magnitude;
    if (direction == Vector3.zero) _targetSpeed = 0;

    Vector3 moveDirection = transform.TransformDirection(direction);
    moveDirection.y = 0;
    Rb.velocity = new Vector3(moveDirection.x * _targetSpeed, Rb.velocity.y, moveDirection.z * _targetSpeed);
  }

  #endregion
}
