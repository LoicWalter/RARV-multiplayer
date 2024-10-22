using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
  [SerializeField] private MeshRenderer _bodyMeshRenderer;
  [SerializeField] private MeshRenderer _backpackMeshRenderer;
  private Material _material;

  private void Awake()
  {
    _material = new Material(_bodyMeshRenderer.material);
    _bodyMeshRenderer.material = _material;
    _backpackMeshRenderer.material = _material;
  }

  public void SetPlayerColor(Color color)
  {
    _material.color = color;
  }
}