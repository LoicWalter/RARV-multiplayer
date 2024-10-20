using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
  [SerializeField] private MeshRenderer _bodyMeshRenderer;
  [SerializeField] private MeshRenderer _backpackMeshRenderer;
  private Material material;

  private void Awake()
  {
    material = new Material(_bodyMeshRenderer.material);
    _bodyMeshRenderer.material = material;
    _backpackMeshRenderer.material = material;
  }

  public void SetPlayerColor(Color color)
  {
    material.color = color;
  }
}