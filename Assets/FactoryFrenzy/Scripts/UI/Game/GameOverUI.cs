using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
  [SerializeField] private Button _mainMenuButton;
  [SerializeField] private Transform _rankContainer;
  [SerializeField] private Transform _rankTemplate;

  private void Start()
  {
    _mainMenuButton.onClick.AddListener(() =>
    {
      NetworkManager.Singleton.Shutdown();
      Loader.Load(Loader.Scene.MainMenuScene);
    });

    FactoryFrenzyGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

    Hide();
  }

  private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
  {
    if (FactoryFrenzyGameManager.Instance.IsGameOver())
    {
      Ranking();
      Show();
    }
    else
    {
      Hide();
    }
  }

  private void Ranking()
  {
    var instance = FactoryFrenzyGameManager.Instance.GetPlayerRanks();

    foreach (PlayerControllerData player in instance)
    {
      //Get player name
      ulong clientId = player.clientId;
      string playerName = FactoryFrenzyMultiplayer.Instance.GetPlayerNameFromClientId(clientId);

      //Instantiate rank line
      Transform rankTransform = Instantiate(_rankTemplate, _rankContainer);
      rankTransform.gameObject.SetActive(true);

      rankTransform.GetComponent<GameOverRankLine>().SetRankUI(playerName, instance.IndexOf(player) + 1);
      //player.freeCamera.gameObject.SetActive(false);
    }
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void Show()
  {
    gameObject.SetActive(true);
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  private void OnDestroy()
  {
    FactoryFrenzyGameManager.Instance.OnGameStateChanged -= GameManager_OnGameStateChanged;
    _mainMenuButton.onClick.RemoveAllListeners();
  }
}          