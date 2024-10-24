using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FactoryFrenzyMapGenerator : MonoBehaviour
{
  public static FactoryFrenzyMapGenerator Instance { get; private set; }

  [SerializeField] private PrefabsListSO _prefabsListSO;

  public event EventHandler OnMapGenerated;


  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  /// <summary>
  /// Generates the map based on the level objects.
  /// </summary>
  /// <param name="levelObjects">
  ///   The level objects to generate the map from.
  /// </param>
  /// <returns>
  ///   The start platform of the generated map.
  /// </returns>
  public void GenerateMap(List<LevelObject> levelObjects)
  {
    Logger.Log("Generating map...");
    foreach (var levelObject in levelObjects)
    {
      SpawnPrefab(levelObject);
    }
    Logger.Log("Map generated.");
    OnMapGenerated?.Invoke(this, EventArgs.Empty);
  }

  private void SpawnPrefab(LevelObject levelObject)
  {
    var prefab = _prefabsListSO.GetPrefab(levelObject.prefabName, out PrefabItem prefabItem);
    if (prefab != null)
    {
      GameObject go = Instantiate(prefab, levelObject.position, levelObject.rotation);
      go.transform.localScale = levelObject.scale;

      if (prefabItem.IsMovingPlatform)
      {
        if (go.TryGetComponent(out MovingPlatform movingPlatformComponent))
        {
          movingPlatformComponent.SetPoints(levelObject.position, levelObject.MoveToPosition);
        }
        else
        {
          Logger.LogError("Moving platform prefab does not have MovingPlatform component. Maybe the prefab name is incorrect?");
        }
      }

      if (prefabItem.IsStartPlatform)
      {
        if (go.TryGetComponent(out StartPlatform startPlatformComponent))
        {
          FactoryFrenzyGameManager.Instance.SetStartPlatform(startPlatformComponent);
        }
        else
        {
          Logger.LogError("Start platform prefab does not have StartPlatform component. Maybe the prefab name is incorrect?");
        }
      }

      if (go.TryGetComponent<NetworkObject>(out var networkObject))
      {
        networkObject.Spawn();
      }
      else
      {
        Logger.LogError($"Prefab {levelObject.prefabName} does not have NetworkObject component.");
      }
    }
  }
}
