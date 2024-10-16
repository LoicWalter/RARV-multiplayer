using UnityEngine;

public interface IMovable
{
  float Speed { get; }
  float LookSpeed { get; }
  float JumpForce { get; }
  Rigidbody Rb { get; }
  bool IsMoving { get; }
  bool IsGrounded { get; }

  void GroundedCheck();
  void Move(Vector3 direction);
  void Rotate(Vector3 rotation);
  void Jump();
}