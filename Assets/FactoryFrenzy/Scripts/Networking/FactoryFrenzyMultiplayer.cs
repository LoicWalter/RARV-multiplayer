using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class FactoryFrenzyMultiplayer : NetworkBehaviour
{
  public const int MAX_PLAYER_AMOUNT = 4;
  private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";


  public static FactoryFrenzyMultiplayer Instance { get; private set; }


  public static bool playMultiplayer = true;


  public event EventHandler OnTryingToJoinGame;
  public event EventHandler OnFailedToJoinGame;
  public event EventHandler OnPlayerDataNetworkListChanged;

  [SerializeField] private List<Color> _playerColorList;


  private NetworkList<PlayerData> _playerDataNetworkList;
  private string _playerName;



  private void Awake()
  {
    Instance = this;

    _playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 1000));

    _playerDataNetworkList = new NetworkList<PlayerData>();
    _playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
  }

  private void Start()
  {
    if (!playMultiplayer)
    {
      // Singleplayer
      StartHost();
      Loader.LoadNetwork(Loader.Scene.GameScene);
    }
  }

  public string GetPlayerName()
  {
    return _playerName;
  }

  public void SetPlayerName(string playerName)
  {
    _playerName = playerName;

    PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
  }

  private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
  {
    OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
  }

  public void StartHost()
  {
    NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
    NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
    NetworkManager.Singleton.StartHost();
  }

  private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
  {
    for (int i = 0; i < _playerDataNetworkList.Count; i++)
    {
      PlayerData playerData = _playerDataNetworkList[i];
      if (playerData.clientId == clientId)
      {
        // Disconnected!
        _playerDataNetworkList.RemoveAt(i);
      }
    }
  }

  private void NetworkManager_OnClientConnectedCallback(ulong clientId)
  {
    _playerDataNetworkList.Add(new PlayerData
    {
      clientId = clientId,
      colorId = GetFirstUnusedColorId(),
    });
    SetPlayerNameServerRpc(GetPlayerName());
    SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
  }

  private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
  {
    if (IsHost)
    {
      connectionApprovalResponse.Approved = true;
      return;
    }

    if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
    {
      connectionApprovalResponse.Approved = false;
      connectionApprovalResponse.Reason = "Game has already started";
      return;
    }

    if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
    {
      connectionApprovalResponse.Approved = false;
      connectionApprovalResponse.Reason = "Game is full";
      return;
    }

    connectionApprovalResponse.Approved = true;
  }

  public void StartClient()
  {
    OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

    NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
    NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
    NetworkManager.Singleton.StartClient();
  }

  private void NetworkManager_Client_OnClientConnectedCallback(ulong clientId)
  {
    SetPlayerNameServerRpc(GetPlayerName());
    SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
  }

  [ServerRpc(RequireOwnership = false)]
  private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
  {
    int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

    PlayerData playerData = _playerDataNetworkList[playerDataIndex];

    playerData.playerName = playerName;

    _playerDataNetworkList[playerDataIndex] = playerData;
  }

  [ServerRpc(RequireOwnership = false)]
  private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
  {
    int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

    PlayerData playerData = _playerDataNetworkList[playerDataIndex];

    playerData.playerId = playerId;

    _playerDataNetworkList[playerDataIndex] = playerData;
  }

  private void NetworkManager_Client_OnClientDisconnectCallback(ulong clientId)
  {
    OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
  }

  public bool IsPlayerIndexConnected(int playerIndex)
  {
    return playerIndex < _playerDataNetworkList.Count;
  }

  public int GetPlayerDataIndexFromClientId(ulong clientId)
  {
    for (int i = 0; i < _playerDataNetworkList.Count; i++)
    {
      if (_playerDataNetworkList[i].clientId == clientId)
      {
        return i;
      }
    }
    return -1;
  }

  public PlayerData GetPlayerDataFromClientId(ulong clientId)
  {
    foreach (PlayerData playerData in _playerDataNetworkList)
    {
      if (playerData.clientId == clientId)
      {
        return playerData;
      }
    }
    return default;
  }

  public PlayerData GetPlayerData()
  {
    return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
  }

  public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
  {
    return _playerDataNetworkList[playerIndex];
  }

  public Color GetPlayerColor(int colorId)
  {
    return _playerColorList[colorId];
  }

  public void ChangePlayerColor(int colorId)
  {
    ChangePlayerColorServerRpc(colorId);
  }

  [ServerRpc(RequireOwnership = false)]
  private void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
  {
    if (!IsColorAvailable(colorId))
    {
      // Color not available
      return;
    }

    int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

    PlayerData playerData = _playerDataNetworkList[playerDataIndex];

    playerData.colorId = colorId;

    _playerDataNetworkList[playerDataIndex] = playerData;
  }

  private bool IsColorAvailable(int colorId)
  {
    foreach (PlayerData playerData in _playerDataNetworkList)
    {
      if (playerData.colorId == colorId)
      {
        // Already in use
        return false;
      }
    }
    return true;
  }

  private int GetFirstUnusedColorId()
  {
    for (int i = 0; i < _playerColorList.Count; i++)
    {
      if (IsColorAvailable(i))
      {
        return i;
      }
    }
    return -1;
  }

  public void KickPlayer(ulong clientId)
  {
    NetworkManager.Singleton.DisconnectClient(clientId);
    NetworkManager_Server_OnClientDisconnectCallback(clientId);
  }

  public string GetPlayerNameFromClientId(ulong clientId)
{
    foreach (PlayerData playerData in _playerDataNetworkList)
    {
        if (playerData.clientId == clientId)
        {
            return playerData.playerName.ToString();
        }
    }
    return "Unknown player";
}

}