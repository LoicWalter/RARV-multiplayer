using UnityEngine;

public class GameManager : MonoBehaviour
{
  public enum GameState
  {
    MainMenu,
    Lobby,
    GameInProgress
  }

  public GameState CurrentGameState { get; private set; }

  public static GameManager Instance { get; private set; }

  public string PlayerName { get; set; }

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
  }

  public void SetGameState(GameState state)
  {
    CurrentGameState = state;
  }
}