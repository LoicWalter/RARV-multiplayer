using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour, IHidable
{
  [SerializeField] private TextMeshProUGUI _countdownText;
  private int previousCountdownNumber;

  void Start()
  {
    Logger.Log("Countdown UI Start");

    FactoryFrenzyGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

    Hide();
  }

  private void Update()
  {
    int countdownNumber = Mathf.CeilToInt(FactoryFrenzyGameManager.Instance.GetCountdownDuration());
    _countdownText.text = countdownNumber.ToString();

    if (previousCountdownNumber != countdownNumber)
    {
      previousCountdownNumber = countdownNumber;
    }
  }

  private void GameManager_OnGameStateChanged(object sender, EventArgs e)
  {
    Logger.Log("Called Countdown UI // Game state changed: " + FactoryFrenzyGameManager.Instance.GetCurrentGameState());
    if (FactoryFrenzyGameManager.Instance.IsCountdown())
    {
      Show();
    }
    else
    {
      Hide();
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
