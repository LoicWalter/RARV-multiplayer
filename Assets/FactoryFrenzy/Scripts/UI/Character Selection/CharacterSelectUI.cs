using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
  [SerializeField] private Button _mainMenuButton;
  [SerializeField] private Button _readyButton;
  [SerializeField] private Button _mapSelectButton;
  [SerializeField] private MapSelectUI _mapSelectUI;
  [SerializeField] private TextMeshProUGUI _lobbyNameText;
  [SerializeField] private TextMeshProUGUI _lobbyCodeText;
  [SerializeField] private TextMeshProUGUI _mapNameText;

  private void Awake()
  {
    _mainMenuButton.onClick.AddListener(() =>
    {
      FactoryFrenzyLobby.Instance.LeaveLobby();
      NetworkManager.Singleton.Shutdown();
      Loader.Load(Loader.Scene.MainMenuScene);
    });

    _readyButton.onClick.AddListener(() =>
    {
      CharacterSelectReady.Instance.SetPlayerReady();
    });

    _mapSelectButton.onClick.AddListener(() =>
    {
      _mapSelectUI.Show();
    });
  }

  private void Start()
  {
    _lobbyNameText.text = "Lobby Name: " + FactoryFrenzyLobby.Instance.LobbyName;
    _lobbyCodeText.text = "Lobby Code: " + FactoryFrenzyLobby.Instance.LobbyCode;
    UpdateMapNameText();

    if (NetworkManager.Singleton.IsHost)
    {
      _mapSelectButton.gameObject.SetActive(true);
    }
    else
    {
      _mapSelectButton.gameObject.SetActive(false);
    }

    FactoryFrenzyLobby.Instance.OnMapNameChanged += FactoryFrenzyLobby_OnMapNameChanged;
  }

  private void FactoryFrenzyLobby_OnMapNameChanged(object sender, EventArgs e)
  {
    UpdateMapNameText();
  }

  private void UpdateMapNameText()
  {
    _mapNameText.text = "Map: " + FactoryFrenzyLobby.Instance.MapName;
  }

  private void OnDestroy()
  {
    FactoryFrenzyLobby.Instance.OnMapNameChanged -= FactoryFrenzyLobby_OnMapNameChanged;
  }
}
