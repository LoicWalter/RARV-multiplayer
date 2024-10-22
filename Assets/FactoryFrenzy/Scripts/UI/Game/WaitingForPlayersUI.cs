using TMPro;
using UnityEngine;

public class WaitingForPlayersUI : MonoBehaviour, IHidable
{
  [SerializeField] private TextMeshProUGUI _waitingForPlayersText;

  private void Start()
  {
    FactoryFrenzyGameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
    FactoryFrenzyGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    SetText("LOADING...");
    Show();
  }

  private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
  {
    Logger.Log("Called Waiting For Players UI // Game state changed: " + FactoryFrenzyGameManager.Instance.GetCurrentGameState());
    if (!FactoryFrenzyGameManager.Instance.IsWaitingToStart())
    {
      Hide();
    }
  }

  private void GameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
  {
    if (FactoryFrenzyGameManager.Instance.IsLocalPlayerReady)
    {
      SetText("WAITING FOR OTHER PLAYERS...");
      Show();
    }
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void SetText(string text)
  {
    _waitingForPlayersText.text = text;
  }

  private void OnDestroy()
  {
    FactoryFrenzyGameManager.Instance.OnLocalPlayerReadyChanged -= GameManager_OnLocalPlayerReadyChanged;
    FactoryFrenzyGameManager.Instance.OnGameStateChanged -= GameManager_OnGameStateChanged;
  }
}