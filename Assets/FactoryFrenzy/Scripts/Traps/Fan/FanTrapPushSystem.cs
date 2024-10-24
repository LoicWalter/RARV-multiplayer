using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The system that pushes the player away from the fan trap.
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class FanTrapPushSystem : MonoBehaviour
{
  [Header("Settings")]
  [Tooltip("The force multiplier applied to the player when they are pushed by the fan trap.")]
  [SerializeField] private float pushForceMultiplier = 100f;

  /// <summary>
  ///  Called when another collider enters the trigger.
  ///  Applies a force to the player to push them away from the fan trap.
  /// </summary>
  /// <param name="other"></param>
  private void OnTriggerStay(Collider other)
  {
    var otherRb = other.attachedRigidbody;
    if (otherRb && otherRb.CompareTag("Player"))
    {
      otherRb.AddForce(transform.forward * pushForceMultiplier, ForceMode.Force);
    }
  }
}
