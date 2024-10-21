using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour, IHidable
{
  [SerializeField] private TextMeshProUGUI _messageText;
  [SerializeField] private Button _closeButton;

  private void Awake()
  {
    _closeButton.onClick.AddListener(() =>
    {
      Hide();
    });
  }

  private void Start()
  {
    FactoryFrenzyMultiplayer.Instance.OnFailedToJoinGame += FactoryFrenzyGameManager_OnFailedToJoinGame;
    FactoryFrenzyLobby.Instance.OnCreateLobbyStarted += FactoryFrenzyLobby_OnCreateLobbyStarted;
    FactoryFrenzyLobby.Instance.OnCreateLobbyFailed += FactoryFrenzyLobby_OnCreateLobbyFailed;
    FactoryFrenzyLobby.Instance.OnJoinFailed += FactoryFrenzyLobby_OnJoinFailed;
    FactoryFrenzyLobby.Instance.OnJoinStarted += FactoryFrenzyLobby_OnJoinStarted;
    FactoryFrenzyLobby.Instance.OnQuickJoinFailed += FactoryFrenzyLobby_OnQuickJoinFailed;
    Hide();
  }

  private void FactoryFrenzyLobby_OnQuickJoinFailed(object sender, EventArgs e)
  {
    ShowMessage("Could not find a lobby to join");
  }

  private void FactoryFrenzyLobby_OnJoinStarted(object sender, EventArgs e)
  {
    ShowMessage("Joining lobby...");
  }

  private void FactoryFrenzyLobby_OnJoinFailed(object sender, EventArgs e)
  {
    ShowMessage("Failed to join lobby");
  }

  private void FactoryFrenzyLobby_OnCreateLobbyFailed(object sender, EventArgs e)
  {
    ShowMessage("Failed to create lobby");
  }

  private void FactoryFrenzyLobby_OnCreateLobbyStarted(object sender, EventArgs e)
  {
    ShowMessage("Creating lobby...");
  }


  private void FactoryFrenzyGameManager_OnFailedToJoinGame(object sender, System.EventArgs e)
  {
    if (NetworkManager.Singleton.DisconnectReason == null)
    {
      ShowMessage("Failed to join game");
    }
    else
    {
      ShowMessage(NetworkManager.Singleton.DisconnectReason);
    }
  }

  private void ShowMessage(string message)
  {
    Show();
    _messageText.text = message;
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  private void OnDestroy()
  {
    FactoryFrenzyMultiplayer.Instance.OnFailedToJoinGame -= FactoryFrenzyGameManager_OnFailedToJoinGame;
    FactoryFrenzyLobby.Instance.OnCreateLobbyStarted -= FactoryFrenzyLobby_OnCreateLobbyStarted;
    FactoryFrenzyLobby.Instance.OnCreateLobbyFailed -= FactoryFrenzyLobby_OnCreateLobbyFailed;
    FactoryFrenzyLobby.Instance.OnJoinFailed -= FactoryFrenzyLobby_OnJoinFailed;
    FactoryFrenzyLobby.Instance.OnJoinStarted -= FactoryFrenzyLobby_OnJoinStarted;
    FactoryFrenzyLobby.Instance.OnQuickJoinFailed -= FactoryFrenzyLobby_OnQuickJoinFailed;
  }
}
