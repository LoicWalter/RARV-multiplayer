using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
  [SerializeField] private Button _playButton;
  [SerializeField] private Button _importMapsButton;
  [SerializeField] private Button _quitButton;
  [SerializeField] private ImportErrorUI _importErrorUI;

  private void Awake()
  {
    _playButton.onClick.AddListener(StartGame);
    _importMapsButton.onClick.AddListener(ImportMaps);
    _quitButton.onClick.AddListener(QuitGame);
  }

  private void StartGame()
  {
    Loader.Load(Loader.Scene.LobbyScene);
  }

  private void ImportMaps()
  {
    if (Importer.AddJSONFilesFromExplorer(out List<string> errorMessages))
    {
      return;
    }

    _importErrorUI.SetErrorText(string.Join("\n", errorMessages));
    _importErrorUI.Show();
  }

  private void QuitGame()
  {
    Application.Quit();
  }
}
