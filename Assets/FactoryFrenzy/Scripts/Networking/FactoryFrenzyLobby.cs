using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class FactoryFrenzyLobby : MonoBehaviour
{
  private const string KEY_RELAY_JOIN_CODE = "relayJoinCode";
  private const string KEY_MAP_NAME = "mapName";

  public static FactoryFrenzyLobby Instance { get; private set; }
  private Lobby _joinedLobby;
  public string LobbyName => _joinedLobby.Name;
  public string LobbyCode => _joinedLobby.LobbyCode;
  public string MapName => GetMapName();

  public event EventHandler OnCreateLobbyStarted;
  public event EventHandler OnCreateLobbyFailed;
  public event EventHandler OnJoinStarted;
  public event EventHandler OnJoinFailed;
  public event EventHandler OnQuickJoinFailed;
  public event EventHandler OnMapNameChanged;
  public event EventHandler<LobbyListChangedEventArgs> OnLobbyListChanged;

  public class LobbyListChangedEventArgs : EventArgs
  {
    public List<Lobby> LobbyList;
  }

  private float _heartbeatTimer = 0f;
  private float _listLobbiesTimer = 0f;

  private async void Awake()
  {
    Instance = this;
    await InitializeUnityAuthentication();
    FactoryFrenzyMapManager.Instance.InitializeLobbyFunction();
  }

  private void Update()
  {
    HandleHeartbeat();
    HandlePeriodicListLobbies();
  }

  private void HandlePeriodicListLobbies()
  {
    if (_joinedLobby != null ||
    !AuthenticationService.Instance.IsSignedIn ||
    SceneManager.GetActiveScene().name != Loader.Scene.LobbyScene.ToString())
    {
      return;
    }

    _listLobbiesTimer -= Time.deltaTime;
    if (_listLobbiesTimer <= 0)
    {
      float listLobbiesTimerMax = 5f;
      _listLobbiesTimer = listLobbiesTimerMax;
      ListLobbies();
    }
  }

  private void HandleHeartbeat()
  {
    if (IsLobbyHost())
    {
      _heartbeatTimer -= Time.deltaTime;
      if (_heartbeatTimer <= 0)
      {
        float heartbeatTimerMax = 15f;
        _heartbeatTimer = heartbeatTimerMax;
        LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
      }
    }
  }

  private bool IsLobbyHost()
  {
    return _joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
  }

  private async void ListLobbies()
  {
    try
    {
      QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
      {
        Filters = new List<QueryFilter>
        {
          new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
        }
      };
      QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
      OnLobbyListChanged?.Invoke(this, new LobbyListChangedEventArgs { LobbyList = queryResponse.Results });
    }
    catch (LobbyServiceException e)
    {
      Logger.LogError(e.Message);
    }
  }

  private async Task InitializeUnityAuthentication()
  {
    if (UnityServices.State != ServicesInitializationState.Initialized)
    {
      InitializationOptions initializationOptions = new();
      var profile = Guid.NewGuid().ToString()[..8];
      initializationOptions.SetProfile(profile);
      await UnityServices.InitializeAsync(initializationOptions);
      await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
  }

  /// <summary>
  /// This method is used to allocate a relay server for the lobby.
  /// This is required for the lobby to function properly.
  /// The relay server is used to relay messages between the clients, allowing them to communicate with each other without needing to know each other's IP addresses.
  /// </summary>
  /// <returns>
  ///   The allocation object that contains the information needed to connect to the relay server.
  /// </returns>
  private async Task<Allocation> AllocateRelay()
  {
    try
    {
      // Create an allocation for the maximum amount of players that can join the lobby. The minus one is because the host is not included in the player count.
      Allocation allocation = await RelayService.Instance.CreateAllocationAsync(FactoryFrenzyMultiplayer.MAX_PLAYER_AMOUNT - 1);
      return allocation;
    }
    catch (RelayServiceException e)
    {
      Logger.LogError(e.Message);
      return default;
    }
  }

  /// <summary>
  /// This method is used to get the join code for the relay server.
  /// </summary>
  /// <param name="allocation">
  ///   The allocation object that contains the information needed to connect to the relay server.
  /// </param>
  /// <returns>
  ///   The join code for the relay server.
  /// </returns>
  private async Task<string> GetRelayJoinCode(Allocation allocation)
  {
    try
    {
      string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
      return relayJoinCode;
    }
    catch (RelayServiceException e)
    {
      Logger.LogError(e.Message);
      return default;
    }
  }

  /// <summary>
  /// This method is used to join a relay server using a join code.
  /// </summary>
  /// <param name="relayJoinCode">
  ///   The join code for the relay server.
  /// </param>
  /// <returns>
  ///   The join allocation object that contains the information needed to connect to the relay server.
  /// </returns>
  private async Task<JoinAllocation> JoinRelay(string relayJoinCode)
  {
    try
    {
      JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
      return joinAllocation;
    }
    catch (RelayServiceException e)
    {
      Logger.LogError(e.Message);
      return default;
    }
  }

  /// <summary>
  /// This method is used to create a lobby. The lobby will be created with the specified name and privacy settings.
  /// </summary>
  /// <param name="lobbyName">
  ///   The name of the lobby.
  /// </param>
  /// <param name="isPrivate">
  ///   Whether the lobby should be private or not.
  /// </param>
  public async void CreateLobby(string lobbyName, bool isPrivate)
  {
    OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
    try
    {
      _joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, FactoryFrenzyMultiplayer.MAX_PLAYER_AMOUNT,
      new CreateLobbyOptions
      {
        IsPrivate = isPrivate,
        Data = new Dictionary<string, DataObject>
        {
          { KEY_MAP_NAME, new DataObject(DataObject.VisibilityOptions.Member, FactoryFrenzyMapManager.Instance.SelectedMap) }
        }
      });

      Allocation allocation = await AllocateRelay();
      string relayJoinCode = await GetRelayJoinCode(allocation);
      _joinedLobby = await LobbyService.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions
      {
        Data = new Dictionary<string, DataObject>
        {
          { KEY_RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) },
        }
      });
      NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

      FactoryFrenzyMultiplayer.Instance.StartHost();
      Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
    }
    catch (LobbyServiceException e)
    {
      Logger.LogError(e.Message);
      OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
    }
  }

  public async void QuickJoin()
  {
    OnJoinStarted?.Invoke(this, EventArgs.Empty);
    try
    {
      _joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

      await SetupRelayServerData();
      FactoryFrenzyMultiplayer.Instance.StartClient();
    }
    catch (LobbyServiceException e)
    {
      Logger.LogError(e.Message);
      OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
    }
  }

  public async void JoinByCode(string code)
  {
    Logger.Log($"Joining lobby with code: {code}");
    OnJoinStarted?.Invoke(this, EventArgs.Empty);
    try
    {
      _joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
      await SetupRelayServerData();
      FactoryFrenzyMultiplayer.Instance.StartClient();
    }
    catch (LobbyServiceException e)
    {
      Logger.LogError(e.Message);
      OnJoinFailed?.Invoke(this, EventArgs.Empty);
    }
  }

  public async void JoinById(string lobbyId)
  {
    OnJoinStarted?.Invoke(this, EventArgs.Empty);
    try
    {
      _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
      await SetupRelayServerData();
      FactoryFrenzyMultiplayer.Instance.StartClient();
    }
    catch (LobbyServiceException e)
    {
      Logger.LogError(e.Message);
      OnJoinFailed?.Invoke(this, EventArgs.Empty);
    }
  }

  private async Task SetupRelayServerData()
  {
    string relayJoinCode = _joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
    JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
    NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
  }

  public async void DeleteLobby()
  {
    if (_joinedLobby != null)
    {
      try
      {
        await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
        _joinedLobby = null;
      }
      catch (LobbyServiceException e)
      {
        Logger.LogError(e.Message);
      }
    }
  }

  public async void LeaveLobby()
  {
    if (_joinedLobby != null)
    {
      try
      {
        await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        _joinedLobby = null;
      }
      catch (LobbyServiceException e)
      {
        Logger.LogError(e.Message);
      }
    }
  }

  public async void KickPlayer(string playerId)
  {
    if (IsLobbyHost())
    {
      try
      {
        await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerId);
      }
      catch (LobbyServiceException e)
      {
        Logger.LogError(e.Message);
      }
    }
  }

  public Lobby GetJoinedLobby()
  {
    return _joinedLobby;
  }

  private string GetMapName()
  {
    if (_joinedLobby == null || !_joinedLobby.Data.ContainsKey(KEY_MAP_NAME))
    {
      return string.Empty;
    }
    return GetJoinedLobby().Data[KEY_MAP_NAME].Value;
  }

  public async void SetMapName(string mapName)
  {
    try
    {
      Logger.Log($"Setting map name to: {mapName}");
      _joinedLobby = await LobbyService.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions
      {
        Data = new Dictionary<string, DataObject>
        {
          { KEY_MAP_NAME, new DataObject(DataObject.VisibilityOptions.Member, mapName) }
        }
      });

      OnMapNameChanged?.Invoke(this, EventArgs.Empty);

      Logger.Log($"Map name set to: {GetJoinedLobby().Data[KEY_MAP_NAME].Value}");
    }
    catch (LobbyServiceException e)
    {
      Logger.LogError(e.Message);
    }
  }

  private void OnDestroy()
  {
    FactoryFrenzyMapManager.Instance.DesinitializeLobbyFunction();
  }
}