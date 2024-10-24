using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectedUI : MonoBehaviour, IHidable
{
  [SerializeField] private Button _mainMenuButton;

  private void Start()
  {
    NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectedCallback;
    Hide();
    _mainMenuButton.onClick.AddListener(() =>
    {
      NetworkManager.Singleton.Shutdown();
      Loader.Load(Loader.Scene.MainMenuScene);
    });
  }

  private void NetworkManager_OnClientDisconnectedCallback(ulong clientId)
  {
    Logger.Log("Client disconnected");
    if (clientId == NetworkManager.ServerClientId)
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
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    gameObject.SetActive(true);
  }

  private void OnDestroy()
  {
    NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectedCallback;
  }
}
