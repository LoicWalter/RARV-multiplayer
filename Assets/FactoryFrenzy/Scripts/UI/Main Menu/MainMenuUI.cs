using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
  [SerializeField] private Button _playButton;
  [SerializeField] private Button _addMapButton;
  [SerializeField] private Button _quitButton;
  [SerializeField] private ImportJsonUI _importJsonUI;

  private void Awake()
  {
    _playButton.onClick.AddListener(StartGame);
    _addMapButton.onClick.AddListener(AddMap);
    _quitButton.onClick.AddListener(QuitGame);

  }

  private void StartGame()
  {
    Loader.Load(Loader.Scene.LobbyScene);
  }

  private void AddMap()
  {
    _importJsonUI.Show();
  }

  private void QuitGame()
  {
    Application.Quit();
  }
}
