using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayersDetector : MonoBehaviour
{
  [Header("Debug")]
  [Tooltip("Whether to draw gizmos in the scene view.")]
  [SerializeField] private bool DrawGizmos = true;
  [Tooltip("The color of the gizmos.")]
  [SerializeField] private Color gizmosColor = Color.green;

  [Header("Settings")]
  [Tooltip("The range at which the gameObject is detected.")]
  [SerializeField] private float detectionRange = 10f;

  private SphereCollider detectionCollider;

  [field: SerializeField] public List<PlayerController> PlayersInRange { get; private set; } = new List<PlayerController>();

  private void Awake()
  {
    detectionCollider = GetComponent<SphereCollider>();
    detectionCollider.isTrigger = true;
    detectionCollider.radius = detectionRange;
  }

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
    Gizmos.DrawWireSphere(transform.position, detectionRange);
  }
}
