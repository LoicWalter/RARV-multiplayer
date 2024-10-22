using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsListSO", menuName = "PrefabsListSO", order = 0)]
public class PrefabsListSO : ScriptableObject
{
  [SerializeField] private GameObject[] _prefabs;
  [SerializeField] private string _startPlatformPrefabName;

  [SerializeField] private Dictionary<string, GameObject> _prefabsDictionary;

  public GameObject GetPrefab(string prefabName)
  {
    if (_prefabsDictionary.ContainsKey(prefabName))
    {
      return _prefabsDictionary[prefabName];
    }

    return null;
  }

  public GameObject StartPlatformPrefab => GetPrefab(_startPlatformPrefabName);

  [ExecuteInEditMode]
  private void OnValidate()
  {
    foreach (var prefab in _prefabs)
    {
      if (prefab == null)
      {
        Logger.LogError("Prefab is null in PrefabsListSO, please assign all prefabs.");
        return;
      }

      if (string.IsNullOrEmpty(prefab.name))
      {
        Logger.LogError("Prefab name is empty in PrefabsListSO, please assign all prefab names.");
        return;
      }

      if (!prefab.TryGetComponent<NetworkObject>(out _))
      {
        Logger.LogWarning($"Prefab {prefab.name} is missing a NetworkObject component. Adding one now...");
        prefab.AddComponent<NetworkObject>();
        Logger.LogWarning($"NetworkObject component added to {prefab.name}. Don't forget to assign it to the NetworkManager.");
      }
    }

    Logger.Log("PrefabsListSO validated.");
  }

  private void OnEnable()
  {
    Logger.Log("Generating dictionary...");
    _prefabsDictionary = new Dictionary<string, GameObject>();
    foreach (var prefab in _prefabs)
    {
      _prefabsDictionary.Add(prefab.name, prefab);
    }
    Logger.Log("Dictionary generated.");
  }
}