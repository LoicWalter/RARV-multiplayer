using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour, IHidable
{
  private void Start()
  {
    FactoryFrenzyMultiplayer.Instance.OnTryingToJoinGame += FactoryFrenzyGameManager_OnTryingToJoinGame;
    FactoryFrenzyMultiplayer.Instance.OnFailedToJoinGame += FactoryFrenzyGameManager_OnFailedToJoinGame;

    Hide();
  }

  private void FactoryFrenzyGameManager_OnFailedToJoinGame(object sender, System.EventArgs e)
  {
    Hide();
  }

  private void FactoryFrenzyGameManager_OnTryingToJoinGame(object sender, System.EventArgs e)
  {
    Show();
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  private void OnDestroy()
  {
    FactoryFrenzyMultiplayer.Instance.OnTryingToJoinGame -= FactoryFrenzyGameManager_OnTryingToJoinGame;
    FactoryFrenzyMultiplayer.Instance.OnFailedToJoinGame -= FactoryFrenzyGameManager_OnFailedToJoinGame;
  }

}