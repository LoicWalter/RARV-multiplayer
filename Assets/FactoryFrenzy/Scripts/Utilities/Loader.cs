using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
  public enum Scene
  {
    MainMenuScene,
    LobbyScene,
    CharacterSelectScene,
    GameScene,
  }

  private static Scene _targetScene;

  public static void Load(Scene targetScene)
  {
    _targetScene = targetScene;
    SceneManager.LoadScene(targetScene.ToString());
  }

  public static void LoadNetwork(Scene targetScene)
  {
    NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
  }

  public static void LoaderCallback()
  {
    SceneManager.LoadScene(_targetScene.ToString());
  }
}