using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Unity.Netcode;
public class CharacterSelectReady : NetworkBehaviour
{
  public event EventHandler OnReadyChanged;
  public static CharacterSelectReady Instance { get; private set; }
  private Dictionary<ulong, bool> _playerReadyDictionary;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
    _playerReadyDictionary = new Dictionary<ulong, bool>();
  }

  [ServerRpc(RequireOwnership = false)]
  private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
  {
    SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
    _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
    bool allPlayersReady = true;
    foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
    {
      if (!_playerReadyDictionary.ContainsKey(clientId) || !_playerReadyDictionary[clientId])
      {
        allPlayersReady = false;
        break;
      }
    }

    if (allPlayersReady)
    {
      FactoryFrenzyLobby.Instance.DeleteLobby();
      Loader.LoadNetwork(Loader.Scene.GameScene);
    }
  }

  [ClientRpc]
  private void SetPlayerReadyClientRpc(ulong clientId)
  {
    _playerReadyDictionary[clientId] = true;
    OnReadyChanged?.Invoke(this, EventArgs.Empty);
  }

  public void SetPlayerReady()
  {
    SetPlayerReadyServerRpc();
  }


  public bool IsPlayerReady(ulong clientId)
  {
    return _playerReadyDictionary.ContainsKey(clientId) && _playerReadyDictionary[clientId];
  }
}
