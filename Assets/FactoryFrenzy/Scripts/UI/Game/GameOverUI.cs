using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
  [SerializeField] private Button _mainMenuButton;

  private void Start()
  {
    _mainMenuButton.onClick.AddListener(() =>
    {
      NetworkManager.Singleton.Shutdown();
      Loader.Load(Loader.Scene.MainMenuScene);
    });
  }
}
