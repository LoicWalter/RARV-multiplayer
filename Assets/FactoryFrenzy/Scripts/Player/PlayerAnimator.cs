using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
  //Might be null if we only want the player visuals (ex: character selection)
  [Tooltip("The player controller to get the player's state. Might be null if we only want the player visuals (ex: character selection)")]
  [SerializeField] private PlayerController _playerController;
  private Animator _animator;
  private const string IS_WALKING = "IsWalking";
  private const string IS_RUNNING = "IsRunning";
  private const string IS_GROUNDED = "IsGrounded";
  private const string JUMP = "Jump";
  private const string IS_FALLING = "IsFalling";
  private const string IS_RISING = "IsRising";

  private bool _isJumping = false;
  private bool _hasUngrounded = false;


  // Start is called before the first frame update
  void Start()
  {
    _animator = GetComponent<Animator>();
    if (_playerController != null)
    {
      _playerController.OnJumpEvent += PlayerController_OnJumpEvent;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (_playerController == null)
    {
      return;
    }

    if (!_hasUngrounded && !_playerController.IsGrounded)
    {
      _hasUngrounded = true;
    }

    if (_isJumping && _playerController.IsGrounded && _hasUngrounded)
    {
      _isJumping = false;
      _hasUngrounded = false;
      _animator.ResetTrigger(JUMP);
    }

    bool isMoving = _playerController.IsMoving;
    bool isRunning = _playerController.IsRunning;
    bool isGrounded = _playerController.IsGrounded;
    bool isFalling = _playerController.IsFalling;
    bool isRising = _playerController.IsRising;

    if (!isGrounded)
    {
      isMoving = false;
      isRunning = false;
    }

    _animator.SetBool(IS_WALKING, isMoving);
    _animator.SetBool(IS_RUNNING, isRunning);
    _animator.SetBool(IS_GROUNDED, isGrounded);
    _animator.SetBool(IS_FALLING, isFalling);
    _animator.SetBool(IS_RISING, isRising);
  }

  private void OnDestroy()
  {
    if (_playerController == null)
    {
      return;
    }
    _playerController.OnJumpEvent -= PlayerController_OnJumpEvent;
  }


  private void PlayerController_OnJumpEvent(object sender, EventArgs e)
  {
    Logger.Log("Jumping");
    _animator.SetTrigger(JUMP);
    _isJumping = true;
    _hasUngrounded = false;
  }
}
