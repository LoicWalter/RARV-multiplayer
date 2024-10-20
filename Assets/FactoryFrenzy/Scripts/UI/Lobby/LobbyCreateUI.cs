using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour, IHidable
{
  [SerializeField] private Button _closeButton;
  [SerializeField] private Button _createPrivateButton;
  [SerializeField] private Button _createPublicButton;
  [SerializeField] private TMP_InputField _lobbyNameInputField;

  private void Awake()
  {
    _closeButton.onClick.AddListener(Hide);
    _createPrivateButton.onClick.AddListener(() =>
    {
      FactoryFrenzyLobby.Instance.CreateLobby(_lobbyNameInputField.text, true);
      Hide();
    });
    _createPublicButton.onClick.AddListener(() =>
    {
      FactoryFrenzyLobby.Instance.CreateLobby(_lobbyNameInputField.text, false);
      Hide();
    });
  }

  private void Start()
  {
    Hide();
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
