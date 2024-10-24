using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PlayerVisual : MonoBehaviour
{
  [SerializeField] private MeshRenderer _originalMeshRenderer;
  [Tooltip("The mesh renderers to apply the material to.")]
  [SerializeField] private List<MeshRenderer> _meshRenderers = new();
  private Material _material;

  private void Awake()
  {
    Setup();
  }

  public void SetPlayerColor(Color color)
  {
    _material.color = color;
  }

  void Setup()
  {
    if (_originalMeshRenderer == null)
    {
      Debug.LogError("The original mesh renderer is not set.");
      return;
    }

    if (_meshRenderers.Contains(_originalMeshRenderer))
    {
      Debug.LogWarning("The original mesh renderer is in the list of mesh renderers by default. Removing it from the list.");
      _meshRenderers.Remove(_originalMeshRenderer);
    }

    _material = new Material(_originalMeshRenderer.material);
    _originalMeshRenderer.material = _material;
    foreach (MeshRenderer meshRenderer in _meshRenderers)
    {
      meshRenderer.material = _material;
    }
  }
}