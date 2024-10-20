using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
  [SerializeField] private int _colorId;
  [SerializeField] private Image _image;
  [SerializeField] private GameObject _selectedGameObject;

  private void Start()
  {
    FactoryFrenzyMultiplayer.Instance.OnPlayerDataNetworkListChanged += FactoryFrenzyGameMultiplayer_OnPlayerDataNetworkListChanged;
    _image.color = FactoryFrenzyMultiplayer.Instance.GetPlayerColor(_colorId);
    UpdateIsSelected();
  }

  private void FactoryFrenzyGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
  {
    UpdateIsSelected();
  }

  private void Awake()
  {
    GetComponent<Button>().onClick.AddListener(() =>
    {
      FactoryFrenzyMultiplayer.Instance.ChangePlayerColor(_colorId);
      UpdateIsSelected();
    });
  }

  private void UpdateIsSelected()
  {
    if (FactoryFrenzyMultiplayer.Instance.GetPlayerData().colorId == _colorId)
    {
      _selectedGameObject.SetActive(true);
    }
    else
    {
      _selectedGameObject.SetActive(false);
    }
  }

  private void OnDestroy()
  {
    FactoryFrenzyMultiplayer.Instance.OnPlayerDataNetworkListChanged -= FactoryFrenzyGameMultiplayer_OnPlayerDataNetworkListChanged;
  }
}
