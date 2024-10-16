using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrapPushSystem : MonoBehaviour
{
  [Header("Settings")]
  [Tooltip("The force multiplier applied to the player when they are pushed by the fan trap.")]
  [SerializeField] private float pushForceMultiplier = 100f;

  private void OnTriggerStay(Collider other)
  {
    var otherRb = other.attachedRigidbody;
    if (otherRb && otherRb.CompareTag("Player"))
    {
      other.attachedRigidbody.AddForce(transform.forward * pushForceMultiplier, ForceMode.Force);
    }
  }
}
