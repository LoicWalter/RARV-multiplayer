using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// The game manager for the Factory Frenzy game.
/// </summary>
public class FactoryFrenzyGameManager : NetworkBehaviour
{
  #region Variables
  [SerializeField] private Transform _playerPrefab;
  private Dictionary<ulong, bool> _playerReadyStates = new();

  public event EventHandler OnGameStateChanged;
  public event EventHandler OnLocalPlayerReadyChanged;

  private StartPlatform _startPlatform;

  public enum GameState
  {
    WaitingToStart,
    Countdown,
    Playing,
    GameOver
  }

  public NetworkVariable<float> CountdownDuration = new(3f);

  public NetworkList<PlayerControllerData> PlayerRanks = new();

  private NetworkVariable<GameState> _currentGameState = new(GameState.WaitingToStart);

  public bool IsLocalPlayerReady { get; private set; } = false;

  public static FactoryFrenzyGameManager Instance { get; private set; }

  public string PlayerName { get; set; }

  #endregion

  #region Unity Methods

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  private void Update()
  {
    if (!IsServer) return;
    switch (_currentGameState.Value)
    {
      case GameState.WaitingToStart:
        break;
      case GameState.Countdown:
        CountdownDuration.Value -= Time.deltaTime;
        if (CountdownDuration.Value < 0f)
        {
          _currentGameState.Value = GameState.Playing;
        }
        break;
      case GameState.Playing:
        break;
      case GameState.GameOver:
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  #endregion

  #region Network Methods

  public override void OnNetworkSpawn()
  {
    _currentGameState.OnValueChanged += State_OnValueChanged;

    if (IsServer)
    {
      NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
      FactoryFrenzyMapGenerator.Instance.OnMapGenerated += MapGenerator_OnMapGenerated;
    }
  }

  #endregion

  #region Event Handlers

  /// <summary>
  /// Spawns the player objects for each connected client when the map is generated.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void MapGenerator_OnMapGenerated(object sender, EventArgs e)
  {
    Logger.Log("Spawning player objects...");
    Logger.Log("Current game state: " + _currentGameState.Value);
    foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
    {
      Vector3 randomPosition = _startPlatform.GetRandomUnusedPlateformSpawnPoint().position;
      Transform playerTransform = Instantiate(_playerPrefab, randomPosition, Quaternion.identity);
      playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
    Logger.Log("Player objects spawned.");
  }

  /// <summary>
  /// Spawns the player objects for each connected client when the scene is loaded.
  /// </summary>
  /// <param name="sceneName">
  ///   The name of the scene that was loaded.
  /// </param>
  /// <param name="loadSceneMode">
  ///   The mode in which the scene was loaded.
  /// </param>
  /// <param name="clientsCompleted">
  ///   The list of clients that completed loading the scene.
  /// </param>
  /// <param name="clientsTimedOut">
  ///   The list of clients that timed out while loading the scene.
  /// </param>
  private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
  {
    FactoryFrenzyMapGenerator.Instance.GenerateMap(FactoryFrenzyMapManager.Instance.LevelObjects);
  }

  /// <summary>
  /// Handles the event when the game state changes.
  /// </summary>
  /// <param name="previousState">
  ///   The previous state of the game.
  /// </param>
  /// <param name="newState">
  ///   The new state of the game.
  /// </param>
  private void State_OnValueChanged(GameState previousState, GameState newState)
  {
    Logger.Log("Called in event // Game state changed: " + newState);
    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
  }

  #endregion

  #region Public Methods

  public void SetStartPlatform(StartPlatform startPlatform)
  {
    _startPlatform = startPlatform;
  }

  public void SetLocalPlayerReady()
  {
    if (_currentGameState.Value == GameState.WaitingToStart)
    {
      IsLocalPlayerReady = true;
      OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

      SetPlayerReadyServerRpc();
    }
  }

  public GameState GetCurrentGameState()
  {
    return _currentGameState.Value;
  }

  public void SetGameState(GameState state)
  {
    _currentGameState.Value = state;
  }

  public bool IsCountdown()
  {
    return _currentGameState.Value == GameState.Countdown;
  }

  public bool IsPlaying()
  {
    return _currentGameState.Value == GameState.Playing;
  }

  public bool IsGameOver()
  {
    return _currentGameState.Value == GameState.GameOver;
  }

  public bool IsWaitingToStart()
  {
    return _currentGameState.Value == GameState.WaitingToStart;
  }

  public float GetCountdownDuration()
  {
    return CountdownDuration.Value;
  }

  #endregion

  #region Server Methods

  [ServerRpc(RequireOwnership = false)]
  private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
  {
    _playerReadyStates[serverRpcParams.Receive.SenderClientId] = true;
    bool allPlayersReady = true;
    foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
    {
      if (!_playerReadyStates.ContainsKey(clientId) || !_playerReadyStates[clientId])
      {
        allPlayersReady = false;
        break;
      }
    }

    Logger.Log("All players ready: " + allPlayersReady);

    if (allPlayersReady)
    {
      StartCoroutine(StartCountdown());
    }
  }

  private IEnumerator StartCountdown()
  {
    yield return new WaitForSeconds(1f);
    CountdownDuration.Value = 3f;
    SetGameState(GameState.Countdown);
  }

  public void AddPlayerRank(PlayerControllerData playerControllerData)
  {
    AddPlayerRankServerRpc(playerControllerData);
  }

  [ServerRpc(RequireOwnership = false)]
  private void AddPlayerRankServerRpc(PlayerControllerData PlayerControllerData, ServerRpcParams rpcParams = default)
  {
    if (PlayerRanks.Contains(PlayerControllerData)) return;

    PlayerRanks.Add(PlayerControllerData);

    Debug.Log("Player has finished the game in " + PlayerRanks.Count + " place.");
    
    if (PlayerRanks.Count == NetworkManager.Singleton.ConnectedClientsList.Count)
    {
      SetGameState(GameState.GameOver);
    }
  }

  public List<PlayerControllerData> GetPlayerRanks()
  {
    List<PlayerControllerData> playerRanks = new();

    foreach (PlayerControllerData player in PlayerRanks)
    {
      playerRanks.Add(player);
    }
    return playerRanks;
  }

  #endregion
}