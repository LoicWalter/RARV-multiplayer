using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagColor : MonoBehaviour
{
  [SerializeField] private Material _CheckpointFlagged;
  [SerializeField] private Renderer _FlagRenderer;
  private bool _HasFlagged = false;

  private void OnTriggerEnter(Collider other)
  {
    if (_HasFlagged) return;

    if (other.attachedRigidbody.CompareTag("Player"))
    {

      if (other.attachedRigidbody.TryGetComponent<PlayerController>(out var playerController))
      {
        if (!playerController.GetIsOwner()) return;
      }

      if (_FlagRenderer != null && _CheckpointFlagged != null)
      {
        _FlagRenderer.material = _CheckpointFlagged;
        _HasFlagged = true;
      }
    }
  }
}
