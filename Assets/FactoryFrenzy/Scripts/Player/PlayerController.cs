using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// The player controller
/// It handles the player's movement and input, and references some useful Transform objects
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : NetworkBehaviour, IPlayerMovable
{
  #region Variables

  [Header("Debug")]
  [Tooltip("Whether to draw gizmos in the scene view.")]
  [SerializeField] private bool DrawGizmos = true;
  [Tooltip("Whether the player is the owner of the object. Useful when testing without a NetworkManager.")]
  [SerializeField] private bool _forceIsOwner = false;

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
  public float JumpForce { get; private set; } = 2f;

  [field: SerializeField, Header("Ground Check"), Tooltip("Whether the player is grounded or not.")]
  public bool IsGrounded { get; private set; } = false;

  [Tooltip("The radius of the sphere used to check if the player is grounded.")]
  [SerializeField] private float GroundedRadius = 0.03f;

  [field: SerializeField, Tooltip("The layer mask used to check if the player is grounded.")]
  public LayerMask GroundLayer { get; private set; }
  public Rigidbody Rb { get; private set; }
  public float Speed { get => IsRunning ? RunSpeed : WalkSpeed; }
  public bool IsMoving { get => Rb.velocity.magnitude > 0.1f; }

  [field: SerializeField, Header("Camera"), Tooltip("The camera used to follow the player.")]
  public CinemachineVirtualCamera VirtualCameraPrefab { get; private set; }
  [Tooltip("The point at which the camera will look.")]
  public Transform CameraLookPoint;
  [Tooltip("The point at which the camera will follow.")]
  public Transform CameraFollowPoint;
  [field: SerializeField, Header("Other"), Tooltip("The point at which enemies will aim.")]
  public Transform AimPoint { get; private set; }
  [Tooltip("The player's visual representation.")]
  [SerializeField] private PlayerVisual _playerVisual;
  private Vector3 _currentVelocity;
  private CinemachineVirtualCamera _virtualCamera;

  #endregion

  #region Input Actions

  public PlayerInput playerInput;

  #endregion

  #region Network Methods

  public override void OnNetworkSpawn()
  {
    base.OnNetworkSpawn();
    if (!IsOwner && !_forceIsOwner)
    {
      playerInput.enabled = false;
      return;
    }

    _virtualCamera = Instantiate(VirtualCameraPrefab, transform.position, Quaternion.identity);
    _virtualCamera.Follow = CameraFollowPoint;
    _virtualCamera.LookAt = CameraLookPoint;

    _virtualCamera.enabled = true;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    FactoryFrenzyGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    FactoryFrenzyGameManager.Instance.SetLocalPlayerReady();
    playerInput.enabled = true;
  }

  #endregion

  #region Unity Methods

  private void Start()
  {
    PlayerData playerData = FactoryFrenzyMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
    _playerVisual.SetPlayerColor(FactoryFrenzyMultiplayer.Instance.GetPlayerColor(playerData.colorId));
  }

  private void Awake()
  {
    Rb = GetComponent<Rigidbody>();
    OnNetworkSpawn();
  }

  private void Update()
  {
    if (!IsOwner && !_forceIsOwner) return;
    GroundedCheck();
    if (Mouse.current.delta.ReadValue().magnitude > 0)
    {
      Rotate(new Vector3(Mouse.current.delta.ReadValue().x, 0, Mouse.current.delta.ReadValue().y));
    }
  }

  private void FixedUpdate()
  {
    if (!IsOwner && !_forceIsOwner) return;
    Move(_moveDirection);
  }

  private void OnDrawGizmos()
  {
    if (!DrawGizmos) return;
    Gizmos.color = IsGrounded ? Color.green : Color.red;
    Gizmos.DrawWireSphere(transform.position, GroundedRadius);
  }

  #endregion

  #region Event Handlers

  private void GameManager_OnGameStateChanged(object sender, EventArgs e)
  {
    if (FactoryFrenzyGameManager.Instance.IsPlaying())
    {
      playerInput.enabled = true;
    }
    else
    {
      playerInput.enabled = false;
    }
  }

  #endregion

  #region Input Methods

  public void OnMove(CallbackContext context)
  {
    _moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
  }

  public void OnRun(CallbackContext context)
  {
    IsRunning = context.ReadValueAsButton();
  }

  public void OnJump(CallbackContext context)
  {
    Jump();
  }

  //? Ne fonctionne pas pour une raison inconnue => n'est pas appelée
  public void OnLook(CallbackContext context)
  {
    Rotate(new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y));
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

  public void Move(Vector3 requestedDirection)
  {
    Vector3 currentvelocity = Rb.velocity;
    Vector3 MoveDirection = transform.TransformDirection(requestedDirection);
    Vector3 targetVelocity = MoveDirection * Speed;

    // Smoothly interpolate between the current velocity and the target velocity
    Vector3 newVelocity = Vector3.SmoothDamp(currentvelocity, targetVelocity, ref _currentVelocity, 0.1f);
    Rb.velocity = new Vector3(newVelocity.x, currentvelocity.y, newVelocity.z);
  }

  #endregion
}
