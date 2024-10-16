using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects players within a specified range.
/// It uses a sphere collider to detect players.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class PlayersDetector : MonoBehaviour
{
  [Header("Debug")]
  [Tooltip("Whether to draw gizmos in the scene view.")]
  [SerializeField] private bool DrawGizmos = true;
  [Tooltip("The color of the gizmos.")]
  [SerializeField] private Color gizmosColor = Color.green;

  [field: SerializeField, Header("Settings"), Tooltip("The range at which the gameObject is detected.")]
  public float DetectionRange { get; private set; } = 10f;

  private SphereCollider detectionCollider;

  public List<PlayerController> PlayersInRange { get; private set; } = new List<PlayerController>();

  private void Awake()
  {
    detectionCollider = GetComponent<SphereCollider>();
    detectionCollider.isTrigger = true;
    detectionCollider.radius = DetectionRange;
  }

  /// <summary>
  ///  Called when another collider enters the trigger.
  ///  Adds the player to the list of players in range.
  /// </summary>
  /// <param name="other"></param>
  private void OnTriggerEnter(Collider other)
  {
    var otherRb = other.attachedRigidbody;
    if (otherRb && otherRb.CompareTag("Player"))
    {
      var playerController = otherRb.GetComponent<PlayerController>();
      if (!PlayersInRange.Contains(playerController))
        PlayersInRange.Add(playerController);
    }
  }

  /// <summary>
  /// Called when another collider exits the trigger.
  /// Removes the player from the list of players in range.
  /// </summary>
  /// <param name="other"></param>
  private void OnTriggerExit(Collider other)
  {
    var otherRb = other.attachedRigidbody;
    if (otherRb && otherRb.CompareTag("Player"))
    {
      var playerController = otherRb.GetComponent<PlayerController>();
      if (PlayersInRange.Contains(playerController))
        PlayersInRange.Remove(playerController);
    }
  }

  private void OnDrawGizmos()
  {
    if (!DrawGizmos) return;
    Gizmos.color = gizmosColor;
    Gizmos.DrawWireSphere(transform.position, DetectionRange);
  }
}
