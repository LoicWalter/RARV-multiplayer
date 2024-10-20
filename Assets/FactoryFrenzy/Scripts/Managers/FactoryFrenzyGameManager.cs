using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class FactoryFrenzyGameManager : NetworkBehaviour
{
  [SerializeField] private Transform _playerPrefab;
  private Dictionary<ulong, bool> _playerReadyStates = new();

  public event EventHandler OnGameStateChanged;
  public event EventHandler OnLocalPlayerReadyChanged;

  public enum GameState
  {
    WaitingToStart,
    Countdown,
    Playing,
    GameOver
  }

  public NetworkVariable<float> CountdownDuration = new(3f);

  private NetworkVariable<GameState> _currentGameState = new(GameState.WaitingToStart);

  public bool IsLocalPlayerReady { get; private set; } = false;

  public static FactoryFrenzyGameManager Instance { get; private set; }

  public string PlayerName { get; set; }

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(Instance.gameObject);
      Instance = this;
    }
  }

  public override void OnNetworkSpawn()
  {
    _currentGameState.OnValueChanged += State_OnValueChanged;

    if (IsServer)
    {
      NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
    }
  }

  private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
  {
    foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
    {
      Transform playerTransform = Instantiate(_playerPrefab);
      playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
  }

  private void State_OnValueChanged(GameState previousState, GameState newState)
  {
    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
  }

  public void SetGameState(GameState state)
  {
    _currentGameState.Value = state;
  }

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

    if (allPlayersReady)
    {
      SetGameState(GameState.Countdown);
    }
  }

  private void GameInput_OnPlayerReadyChanged()
  {
    if (_currentGameState.Value != GameState.WaitingToStart) return;
    IsLocalPlayerReady = true;
    SetPlayerReadyServerRpc();
    OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
  }

  private void Update()
  {
    if (!IsServer) return;
    switch (_currentGameState.Value)
    {
      case GameState.WaitingToStart:
        break;
      case GameState.Countdown:
        UpdateCountdown();
        break;
      case GameState.Playing:
        break;
      case GameState.GameOver:
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void UpdateCountdown()
  {
    if (CountdownDuration.Value <= 0)
    {
      SetGameState(GameState.Playing);
      return;
    }

    CountdownDuration.Value -= Time.deltaTime;
  }

  public bool IsCountdownActive()
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

}