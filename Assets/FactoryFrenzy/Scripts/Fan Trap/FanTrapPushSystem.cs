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
    // check if the object parent has the player tag
    if (other.transform.parent.CompareTag("Player"))
    {
      Debug.Log("Pushing player");
      var player = other.transform.parent.GetComponent<IMovable>();
      player?.Rb.AddForce(transform.forward * pushForceMultiplier, ForceMode.Force);
    }
  }
}
