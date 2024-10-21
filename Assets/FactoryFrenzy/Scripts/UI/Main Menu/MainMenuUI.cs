using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
  [SerializeField] private Button _playButton;
  [SerializeField] private Button _quitButton;

  private void Awake()
  {
    _playButton.onClick.AddListener(() =>
    {
      Debug.Log("Play button clicked");
      Loader.Load(Loader.Scene.LobbyScene);
    });

    _quitButton.onClick.AddListener(() =>
    {
      Debug.Log("Quit button clicked");
      Application.Quit();
    });
  }
}
