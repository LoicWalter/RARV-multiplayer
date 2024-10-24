using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
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
    [SerializeField]
    private bool DrawGizmos = true;

    [Tooltip(
        "Whether the player is the owner of the object. Useful when testing without a NetworkManager."
    )]
    [SerializeField]
    private bool _forceIsOwner = false;

    [Header("Settings"), Space(5)]
    private Vector2 _moveDirection;

    [Header("Movement")]
    [Tooltip("The speed at which the player moves when running.")]
    [SerializeField]
    private float RunSpeed = 10f;

    [Tooltip("The speed at which the player moves when walking.")]
    [SerializeField]
    private float WalkSpeed = 5f;
    public bool IsRunning { get; private set; } = false;

    [field: SerializeField, Tooltip("The speed at which the player rotates their view.")]
    public float LookSpeed { get; private set; } = 1f;

    [field: SerializeField, Tooltip("The force applied to the player when they jump.")]
    public float JumpForce { get; private set; } = 2f;

    [field:
        SerializeField,
        Header("Ground Check"),
        Tooltip("Whether the player is grounded or not.")
    ]
    public bool IsGrounded { get; private set; } = false;

    [Tooltip("The radius of the sphere used to check if the player is grounded.")]
    [SerializeField]
    private float GroundedRadius = 0.03f;

    [field: SerializeField, Tooltip("The layer mask used to check if the player is grounded.")]
    public LayerMask GroundLayer { get; private set; }
    public Rigidbody Rb { get; private set; }
    public float Speed
    {
        get => IsRunning ? RunSpeed : WalkSpeed;
    }
    public bool IsMoving { get; private set; } = false;
    public bool IsFalling
    {
        get => Rb.velocity.y < -0.1f;
    }
    public bool IsRising
    {
        get => Rb.velocity.y > 0.1f;
    }
    public Vector3 RespawnPos { get; set; }

    [field: SerializeField, Header("Camera"), Tooltip("The camera used to follow the player.")]
    public CinemachineVirtualCamera VirtualCameraPrefab { get; private set; }

    [Tooltip("The camera used after crossing the end line.")]
    [SerializeField]
    public GameObject freeCameraPrefab;

    [Tooltip("The point at which the camera will look.")]
    public Transform CameraLookPoint;

    [Tooltip("The point at which the camera will follow.")]
    public Transform CameraFollowPoint;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip(
        "Additional degress to override the camera. Useful for fine tuning camera position when locked"
    )]
    public float CameraAngleOverride = 0.0f;

    [field: SerializeField, Header("Other"), Tooltip("The point at which enemies will aim.")]
    public Transform AimPoint { get; private set; }

    [Tooltip("The player's visual representation.")]
    [SerializeField]
    private PlayerVisual _playerVisual;

    [Tooltip("The Y limit at which the player will respawn.")]
    [SerializeField]
    private float _limitY = -5f;
    private CinemachineVirtualCamera _virtualCamera;
    private const float _threshold = 0.1f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private float _targetRotation;
    private float _rotationVelocity;
    private GameObject _mainCamera;
    public event EventHandler OnJumpEvent;
    private bool _isFreeCameraActive = false;
    private GameObject _freeCameraInstance;

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
        //_virtualCamera.LookAt = CameraLookPoint;

        _mainCamera = Camera.main.gameObject;

        _cinemachineTargetYaw = CameraFollowPoint.transform.rotation.eulerAngles.y;

        _virtualCamera.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (FactoryFrenzyGameManager.Instance != null)
        {
            FactoryFrenzyGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
            FactoryFrenzyGameManager.Instance.SetLocalPlayerReady();
        }

        playerInput.enabled = _forceIsOwner;
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        RespawnPos = transform.position;
        if (FactoryFrenzyGameManager.Instance != null)
        {
            PlayerData playerData = FactoryFrenzyMultiplayer.Instance.GetPlayerDataFromClientId(
                OwnerClientId
            );
            _playerVisual.SetPlayerColor(
                FactoryFrenzyMultiplayer.Instance.GetPlayerColor(playerData.colorId)
            );
        }
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _moveDirection = Vector2.zero;
        if (_forceIsOwner)
            OnNetworkSpawn();
    }

    private void Update()
    {
        if (!IsOwner && !_forceIsOwner)
            return;
        GroundedCheck();
        RespawnPlayerAt();
    }

    private void FixedUpdate()
    {
        if (!IsOwner && !_forceIsOwner)
            return;
        Move(_moveDirection);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void RespawnPlayerAt()
    {
        if (transform.position.y < _limitY)
        {
            transform.position = RespawnPos;
            Rb.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;
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
        _moveDirection = context.ReadValue<Vector2>();
        if (_moveDirection != Vector2.zero)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnRun(CallbackContext context)
    {
        IsRunning = context.ReadValueAsButton();
    }

    public void OnJump(CallbackContext context)
    {
        if (context.performed)
            Jump();
    }

    // //? Ne fonctionne pas pour une raison inconnue => n'est pas appelée
    // public void OnLook(CallbackContext context)
    // {
    //   Rotate(new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y));
    // }

    #endregion

    #region IMovable Methods

    public void GroundedCheck()
    {
        IsGrounded = Physics.CheckSphere(transform.position, GroundedRadius, GroundLayer);
    }

    public void Jump()
    {
        if (!IsGrounded)
            return;
        OnJumpEvent?.Invoke(this, EventArgs.Empty);
        Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    public void Rotate(Vector3 rotation)
    {
        transform.Rotate(Vector3.up, rotation.x * LookSpeed);
    }

    public void Move(Vector2 requestedDirection)
    {
        // Calculer la vitesse cible en fonction de l'état de course ou de marche
        float targetSpeed = Speed;

        // Si aucune direction n'est demandée, la vitesse cible est 0
        if (requestedDirection == Vector2.zero)
        {
            targetSpeed = 0.0f;
        }

        // Calculer la direction d'entrée normalisée
        Vector3 inputDirection = new Vector3(
            requestedDirection.x,
            0.0f,
            requestedDirection.y
        ).normalized;

        // Si une direction est demandée, calculer la rotation cible
        if (requestedDirection != Vector2.zero)
        {
            _targetRotation =
                Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
                + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                _targetRotation,
                ref _rotationVelocity,
                0.12f
            );

            // Faire pivoter le joueur pour faire face à la direction d'entrée relative à la position de la caméra
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        // Calculer la direction cible en fonction de la rotation cible
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // Calculer la position cible en fonction de la direction cible et de la vitesse cible
        Vector3 targetPosition =
            transform.position + targetSpeed * Time.fixedDeltaTime * targetDirection;

        // Utiliser MovePosition pour déplacer le joueur sans affecter directement la vélocité
        Rb.MovePosition(targetPosition);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (
            Mouse.current.delta.ReadValue().sqrMagnitude >= _threshold /* && !LockCameraPosition */
        )
        {
            var readValue = Mouse.current.delta.ReadValue();
            _cinemachineTargetYaw += readValue.x;
            if (readValue.y > 0)
            {
                readValue.y--;
            }
            if (readValue.y < 0)
            {
                readValue.y++;
            }
            _cinemachineTargetPitch += readValue.y;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CameraFollowPoint.transform.rotation = Quaternion.Euler(
            _cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw,
            0.0f
        );
    }
    #endregion

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
            lfAngle += 360f;
        if (lfAngle > 360f)
            lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public FreeCamera ActivateFreeCamera()
    {
        if (!_isFreeCameraActive)
        {
            _isFreeCameraActive = true;

            // Instantiate the free camera
            _freeCameraInstance = Instantiate(
                freeCameraPrefab,
                transform.position,
                transform.rotation
            );
            _freeCameraInstance.SetActive(true);
        }

        return _freeCameraInstance.GetComponent<FreeCamera>();
    }
}
