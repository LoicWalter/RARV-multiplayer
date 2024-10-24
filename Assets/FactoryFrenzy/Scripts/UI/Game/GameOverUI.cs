using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

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
      List<PlayerController> rankList = FactoryFrenzyGameManager.Instance.PlayerRanks;
      Ranking(rankList);
      Show();
    }
    else
    {
      Hide();
    }
  }

  private void Ranking(List<PlayerController> rankList)
  {
    foreach (PlayerController player in rankList)
    {
      //Get player name
      ulong clientId = player.OwnerClientId;
      string playerName = FactoryFrenzyMultiplayer.Instance.GetPlayerNameFromClientId(clientId);

      //Instantiate rank line
      Transform rankTransform = Instantiate(_rankTemplate, _rankContainer);
      rankTransform.gameObject.SetActive(true);

      rankTransform.GetComponent<GameOverRankLine>().SetRankUI(playerName, rankList.IndexOf(player) + 1);
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