using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatform : MonoBehaviour
{
  public static StartPlatform Instance { get; private set; }

  [Header("Debug")]
  [Tooltip("Draw gizmos for the spawn points")]
  [SerializeField] private bool _drawGizmos;
  [Tooltip("The color of the gizmos")]
  [SerializeField] private Color _gizmoColor = Color.green;

  [Header("Spawn Points")]
  [Tooltip("The spawn points for the start platform")]
  [SerializeField] private Transform[] _spawnPoints;
  private int[] _usedSpawnPoints;

  private void Awake()
  {
    Instance = this;
    _usedSpawnPoints = new int[_spawnPoints.Length];
    if (_spawnPoints.Length == 0)
    {
      Logger.LogError("No spawn points found for the start platform");
    }
  }

  public Transform GetRandomUnusedPlateformSpawnPoint()
  {
    int randomIndex;
    do
    {
      randomIndex = Random.Range(0, _spawnPoints.Length);
    }
    while (_usedSpawnPoints[randomIndex] == 1);

    _usedSpawnPoints[randomIndex] = 1;
    return _spawnPoints[randomIndex];
  }

  private void OnDrawGizmos()
  {
    if (!_drawGizmos) return;
    foreach (var spawnPoint in _spawnPoints)
    {
      Gizmos.color = _gizmoColor;
      Gizmos.DrawSphere(spawnPoint.position, 0.01f);
    }
  }
}
