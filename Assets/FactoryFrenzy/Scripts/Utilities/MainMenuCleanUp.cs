using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour
{
  private void Awake()
  {
    if (NetworkManager.Singleton != null)
    {
      Destroy(NetworkManager.Singleton.gameObject);
    }

    if (FactoryFrenzyMultiplayer.Instance != null)
    {
      Destroy(FactoryFrenzyMultiplayer.Instance.gameObject);
    }

    if (FactoryFrenzyLobby.Instance != null)
    {
      Destroy(FactoryFrenzyLobby.Instance.gameObject);
    }
  }
}
