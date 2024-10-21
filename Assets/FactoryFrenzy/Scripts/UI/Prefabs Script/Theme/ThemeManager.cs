using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// Manages the theme of the UI.
  /// </summary>
  [ExecuteInEditMode]
  public class ThemeManager : MonoBehaviour
  {
    [Header("Theme")]
    [Tooltip("The main theme to use for the UI.")]
    [SerializeField] private ThemeSO _mainTheme;
    public static ThemeManager Instance { get; private set; }

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        Destroy(gameObject);
      }
    }

    public ThemeSO GetMainTheme() => _mainTheme;
  }
}
