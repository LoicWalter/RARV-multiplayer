using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectUI : MonoBehaviour, IHidable
{
  [SerializeField] private Transform _mapContainer;
  [SerializeField] private Transform _mapTemplate;
  [SerializeField] private Button _closeButton;

  private void Awake()
  {
    _closeButton.onClick.AddListener(Hide);
  }

  private void Start()
  {
    Hide();
    FactoryFrenzyMapManager.Instance.OnMapListChanged += FactoryFrenzyMapManager_OnMapListChanged;

    UpdateMapList(FactoryFrenzyMapManager.Instance.GetMapNames());
  }

  private void FactoryFrenzyMapManager_OnMapListChanged(object sender, FactoryFrenzyMapManager.MapListChangedEventArgs e)
  {
    UpdateMapList(e.MapList);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  private void UpdateMapList(List<string> mapList)
  {
    foreach (Transform child in _mapContainer)
    {
      if (child == _mapTemplate) continue;
      Destroy(child.gameObject);
    }

    foreach (string map in mapList)
    {
      Transform mapTransform = Instantiate(_mapTemplate, _mapContainer);
      mapTransform.gameObject.SetActive(true);
      mapTransform.GetComponent<MapListSingleUI>().SetMap(map);
    }
  }

  private void OnDestroy()
  {
    FactoryFrenzyMapManager.Instance.OnMapListChanged -= FactoryFrenzyMapManager_OnMapListChanged;
  }
}