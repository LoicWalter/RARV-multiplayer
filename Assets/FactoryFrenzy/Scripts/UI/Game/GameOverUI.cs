using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
  [SerializeField] private Button _mainMenuButton;
  [SerializeField] private TextMeshProUGUI _text;
  [SerializeField] private Transform _rankContainer;
  [SerializeField] private Transform _rankTemplate;

  private void Awake(){
    _rankTemplate.gameObject.SetActive(false);
  }

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
    foreach (var player in rankList)
    {
      //Get player name
      ulong clientId = player.OwnerClientId;
      string playerName = FactoryFrenzyMultiplayer.Instance.GetPlayerNameFromClientId(clientId);

      //Instanciate rank line
      Transform rankTransform = Instantiate(_rankTemplate, _rankContainer);
      rankTransform.gameObject.SetActive(true);

      SetRankUI(playerName, rankList.IndexOf(player) + 1);
    }
  }

  private void SetRankUI(string name, int rank){
    _text.text = rank + ".  " + name + "\n";
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