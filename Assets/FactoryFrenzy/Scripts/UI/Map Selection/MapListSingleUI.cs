using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapListSingleUI : MonoBehaviour
{

  [SerializeField] private TextMeshProUGUI _mapNameText;
  private string _map;

  private void Awake()
  {
    GetComponent<Button>().onClick.AddListener(() =>
    {
      FactoryFrenzyLobby.Instance.SetMapName(_map);
      GetComponentInParent<MapSelectUI>().Hide();
    });
  }

  public void SetMap(string map)
  {
    _map = map;
    _mapNameText.text = map;
  }
}
