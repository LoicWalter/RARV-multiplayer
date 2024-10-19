using UnityEngine;

[DisallowMultipleComponent]
public class DontDestroyOnLoad : MonoBehaviour
{
  [Header("Settings")]
  [Tooltip("If true, the object will not be destroyed when loading a new scene.")]
  [SerializeField] private bool _isPersistent = true;

  private void Awake()
  {
    if (_isPersistent)
      DontDestroyOnLoad(gameObject);
  }
}