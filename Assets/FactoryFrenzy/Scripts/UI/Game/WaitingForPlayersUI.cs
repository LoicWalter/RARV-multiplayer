using UnityEngine;

public class WaitingForPlayersUI : MonoBehaviour, IHidable
{
  private void Start()
  {
    FactoryFrenzyGameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
    FactoryFrenzyGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    Hide();
  }

  private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
  {
    if (FactoryFrenzyGameManager.Instance.IsCountdownActive())
    {
      Hide();
    }
  }

  private void GameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
  {
    if (FactoryFrenzyGameManager.Instance.IsLocalPlayerReady)
    {
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
}