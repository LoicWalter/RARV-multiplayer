using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// An abstract class for custom UI components.
  /// </summary>
  public abstract class CustomUIComponent : MonoBehaviour
  {
    [Header("Theme")]
    [Tooltip("The theme to use for the component. This will override the theme set in the view.")]
    public ThemeSO overwriteTheme;
    public ThemeSO Theme => GetTheme();
    private void Awake()
    {
      Init();
    }

    public abstract void Setup();

    public abstract void Configure();

    public void Init()
    {
      Setup();
      Configure();
    }

    private void OnValidate()
    {
      Init();
    }

    protected ThemeSO GetTheme()
    {
      if (overwriteTheme != null) return overwriteTheme;
      if (ThemeManager.Instance != null) return ThemeManager.Instance.GetMainTheme();
      return null;
    }
  }
}