using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
  [SerializeField] private Button _mainMenuButton;
  [SerializeField] private Button _createLobbyButton;
  [SerializeField] private Button _quickJoinButton;
  [SerializeField] private Button _joinByCodeButton;
  [SerializeField] private TMP_InputField _codeInputField;
  [SerializeField] private TMP_InputField _playerNameInputField;
  [SerializeField] private LobbyCreateUI _lobbyCreateUI;
  [SerializeField] private Transform _lobbyContainer;
  [SerializeField] private Transform _lobbyTemplate;

  private void Awake()
  {
    _mainMenuButton.onClick.AddListener(() =>
    {
      FactoryFrenzyLobby.Instance.LeaveLobby();
      Loader.Load(Loader.Scene.MainMenuScene);
    });
    _createLobbyButton.onClick.AddListener(() => { _lobbyCreateUI.Show(); });
    _quickJoinButton.onClick.AddListener(() => { FactoryFrenzyLobby.Instance.QuickJoin(); });
    _joinByCodeButton.onClick.AddListener(() => { FactoryFrenzyLobby.Instance.JoinByCode(_codeInputField.text); });

    _lobbyTemplate.gameObject.SetActive(false);
  }

  private void Start()
  {
    _playerNameInputField.text = FactoryFrenzyMultiplayer.Instance.GetPlayerName();
    _playerNameInputField.onEndEdit.AddListener(FactoryFrenzyMultiplayer.Instance.SetPlayerName);
    FactoryFrenzyLobby.Instance.OnLobbyListChanged += FactoryFrenzyLobby_OnLobbyListChanged;
    UpdateLobbyList(new List<Lobby>());
  }

  private void FactoryFrenzyLobby_OnLobbyListChanged(object sender, FactoryFrenzyLobby.LobbyListChangedEventArgs e)
  {
    UpdateLobbyList(e.LobbyList);
  }

  private void UpdateLobbyList(List<Lobby> lobbyList)
  {
    foreach (Transform child in _lobbyContainer)
    {
      if (child == _lobbyTemplate) continue;
      Destroy(child.gameObject);
    }

    foreach (Lobby lobby in lobbyList)
    {
      Transform lobbyTransform = Instantiate(_lobbyTemplate, _lobbyContainer);
      lobbyTransform.gameObject.SetActive(true);
      lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
    }
  }

  private void OnDestroy()
  {
    FactoryFrenzyLobby.Instance.OnLobbyListChanged -= FactoryFrenzyLobby_OnLobbyListChanged;
  }
}
