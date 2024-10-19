using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
  [SerializeField] private TMP_InputField _playerNameText;
  [SerializeField] private TMP_InputField _ipAddress;
  [SerializeField] private TMP_InputField _port;

  private void Start()
  {
    _playerNameText.text = "Player";
    _ipAddress.text = "127.0.0.1";
    _port.text = "9900";
  }

  public void SetPlayerName(string name)
  {
    _playerNameText.text = name;
  }

  public void StartAsHost()
  {
    SetUtpConnectionData();
    NetworkManager.Singleton.StartHost();
    LoadLobbyScene();
  }

  public void StartAsClient()
  {
    SetUtpConnectionData();
    NetworkManager.Singleton.StartClient();
    LoadLobbyScene();
  }

  void LoadLobbyScene()
  {
    SceneManager.LoadScene("Lobby");
  }

  /// <summary>
  /// Use sanitized IP and Port to set up the connection.
  /// </summary>
  void SetUtpConnectionData()
  {
    var sanitizedIPText = SanitizeAlphaNumeric(_ipAddress.text);
    var sanitizedPortText = SanitizeAlphaNumeric(_port.text);

    ushort.TryParse(sanitizedPortText, out var port);

    var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
    utp.SetConnectionData(sanitizedIPText, port);
  }

  /// <summary>
  /// Sanitize user port InputField box allowing only alphanumerics and '.'
  /// </summary>
  /// <param name="dirtyString"> string to sanitize. </param>
  /// <returns> Sanitized text string. </returns>
  static string SanitizeAlphaNumeric(string dirtyString)
  {
    return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
  }
}
