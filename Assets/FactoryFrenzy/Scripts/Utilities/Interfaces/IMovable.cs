using UnityEngine;

/// <summary>
/// Interface for player movement.
/// </summary>
public interface IPlayerMovable
{
  float Speed { get; }
  float LookSpeed { get; }
  float JumpForce { get; }
  Rigidbody Rb { get; }
  bool IsMoving { get; }
  bool IsGrounded { get; }

  /// <summary>
  ///  Checks if the player is grounded.
  /// </summary>
  void GroundedCheck();

  /// <summary>
  /// Moves the player in the specified direction.
  /// </summary>
  /// <param name="direction">
  ///   The direction to move the player in.
  /// </param>
  void Move(Vector2 direction);

  /// <summary>
  /// Rotates the player in the specified direction.
  /// </summary>
  /// <param name="rotation">
  /// The direction to rotate the player in.
  /// </param>
  void Rotate(Vector3 rotation);

  /// <summary>
  /// Makes the player jump.
  /// </summary>
  void Jump();
}