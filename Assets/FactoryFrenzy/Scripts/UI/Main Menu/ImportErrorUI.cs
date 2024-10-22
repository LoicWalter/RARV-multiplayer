using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportErrorUI : MonoBehaviour, IHidable
{
  [SerializeField] private Button _closeButton;
  [SerializeField] private TextMeshProUGUI _errorText;


  private void Awake()
  {
    _closeButton.onClick.AddListener(Hide);
    _errorText.text = "Invalid json input";
  }

  private void Start()
  {
    Hide();
  }


  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }


  public void SetErrorText(string text)
  {
    _errorText.text = text;
  }
}
