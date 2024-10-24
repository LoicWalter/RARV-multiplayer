using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;

[CreateAssetMenu(fileName = "PrefabsListSO", menuName = "PrefabsListSO", order = 0)]
public class PrefabsListSO : ScriptableObject
{
  [SerializeField] private List<PrefabItem> _prefabs = new();

  public GameObject GetPrefab(string prefabName, out PrefabItem prefabItem)
  {
    prefabItem = _prefabs.Find(prefab => prefab.Name == prefabName);
    return prefabItem?.Prefab;
  }

  [ContextMenu("Init")]
  private void Init()
  {
    foreach (PrefabItem prefab in _prefabs)
    {
      if (prefab.Prefab == null)
      {
        Debug.LogError($"A Prefab is missing a reference. Please assign it.");
        return;
      }

      if (prefab.Name.IsNullOrEmpty())
      {
        Logger.LogWarning($"Prefab {prefab.Prefab.name} is missing a name. Adding it now...");
        prefab.Name = prefab.Prefab.name;
      }

      if (!prefab.Prefab.TryGetComponent<NetworkObject>(out _))
      {
        Logger.LogWarning($"Prefab {prefab.Name} is missing a NetworkObject component. Adding one now...");
        prefab.Prefab.AddComponent<NetworkObject>();
        Logger.LogWarning($"NetworkObject component added to {prefab.Name}. Don't forget to assign it to the NetworkManager.");
      }
    }
  }

  private void OnEnable()
  {
    Init();
  }
}

[Serializable]
public class PrefabItem
{
  public GameObject Prefab;
  public string Name;
  public bool IsStartPlatform;
  public bool IsMovingPlatform;
}