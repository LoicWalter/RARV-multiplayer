using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour, IHidable
{
  [SerializeField] private int _playerIndex;
  [SerializeField] private GameObject _readyGameObject;
  [SerializeField] private PlayerVisual _playerVisual;
  [SerializeField] private Button _kickButton;
  [SerializeField] private TextMeshPro _playerNameText;

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  private void Awake()
  {
    _kickButton.onClick.AddListener(() =>
    {
      PlayerData playerData = FactoryFrenzyMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);
      FactoryFrenzyLobby.Instance.KickPlayer(playerData.playerId.ToString());
      FactoryFrenzyMultiplayer.Instance.KickPlayer(playerData.clientId);
    });
  }

  private void Start()
  {
    FactoryFrenzyMultiplayer.Instance.OnPlayerDataNetworkListChanged += FactoryFrenzyMultiplayer_OnPlayerDataNetworkListChanged;
    CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

    // Only the server can kick players, and the server cannot kick itself
    _kickButton.gameObject.SetActive(FactoryFrenzyMultiplayer.Instance.IsServer && _playerIndex != 0);

    UpdatePlayer();
  }

  private void CharacterSelectReady_OnReadyChanged(object sender, EventArgs e)
  {
    UpdatePlayer();
  }

  private void FactoryFrenzyMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
  {
    UpdatePlayer();
  }

  /// <summary>
  /// Updates the player's information on the UI
  /// </summary>
  private void UpdatePlayer()
  {
    if (!FactoryFrenzyMultiplayer.Instance.IsPlayerIndexConnected(_playerIndex))
    {
      Hide();
      return;
    }

    Show();
    PlayerData playerData = FactoryFrenzyMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);
    _readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
    _playerNameText.text = playerData.playerName.ToString();
    _playerVisual.SetPlayerColor(FactoryFrenzyMultiplayer.Instance.GetPlayerColor(playerData.colorId));
  }

  private void OnDestroy()
  {
    FactoryFrenzyMultiplayer.Instance.OnPlayerDataNetworkListChanged -= FactoryFrenzyMultiplayer_OnPlayerDataNetworkListChanged;
    CharacterSelectReady.Instance.OnReadyChanged -= CharacterSelectReady_OnReadyChanged;
  }
}