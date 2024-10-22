using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportJsonUI : MonoBehaviour, IHidable
{
  [SerializeField] private TMP_InputField _jsonInputField;
  [SerializeField] private TMP_InputField _fileNameField;
  [SerializeField] private Button _importButton;
  [SerializeField] private Button _closeButton;
  [SerializeField] private ImportErrorUI _importErrorUI;

  public void Awake()
  {
    _importButton.onClick.AddListener(ImportJson);
    _closeButton.onClick.AddListener(Hide);
  }

  private void Start()
  {
    FactoryFrenzyMapManager.Instance.OnInvalidJsonInput += FactoryFrenzyMapManager_OnInvalidJsonInput;
    FactoryFrenzyMapManager.Instance.OnInvalidFileName += FactoryFrenzyMapManager_OnInvalidFileName;
    FactoryFrenzyMapManager.Instance.OnMapSaveError += FactoryFrenzyMapManager_OnMapSaveError;

    Hide();
  }

  private void FactoryFrenzyMapManager_OnMapSaveError(object sender, EventArgs e)
  {
    ShowErrorMessage("Map save error");
  }

  private void FactoryFrenzyMapManager_OnInvalidFileName(object sender, EventArgs e)
  {
    ShowErrorMessage("Invalid file name");
  }

  private void FactoryFrenzyMapManager_OnInvalidJsonInput(object sender, EventArgs e)
  {
    ShowErrorMessage("Invalid JSON input");
  }

  private void ImportJson()
  {
    FactoryFrenzyMapManager.Instance.SaveMap(_jsonInputField.text, _fileNameField.text);
    Hide();
  }

  private void ShowErrorMessage(string message)
  {
    Logger.LogError(message);
    _importErrorUI.Show();
    _importErrorUI.SetErrorText(message);
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  private void OnDestroy()
  {
    _importButton.onClick.RemoveAllListeners();
    _closeButton.onClick.RemoveAllListeners();

    FactoryFrenzyMapManager.Instance.OnInvalidJsonInput -= FactoryFrenzyMapManager_OnInvalidJsonInput;
    FactoryFrenzyMapManager.Instance.OnInvalidFileName -= FactoryFrenzyMapManager_OnInvalidFileName;
    FactoryFrenzyMapManager.Instance.OnMapSaveError -= FactoryFrenzyMapManager_OnMapSaveError;
  }
}
