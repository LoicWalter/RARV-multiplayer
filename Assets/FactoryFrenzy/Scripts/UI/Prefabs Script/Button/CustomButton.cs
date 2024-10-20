using FactoryFrenzy.Enums;
using FactoryFrenzy.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// Defines a button object and its properties.
  /// </summary>
  public class CustomButton : CustomUIComponent
  {
    [Header("Style")]
    [Tooltip("The style of the button.")]
    public Style style;

    [Header("Functionality")]
    [Tooltip("The function to call when the button is clicked.")]
    public UnityEvent OnClickEvent;

    private Button _buttonComponent;
    private TextMeshProUGUI _textComponent;

    public override void Setup()
    {
      _buttonComponent = GetComponentInChildren<Button>();
      _textComponent = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Configure()
    {
      if (Theme == null) return;
      ColorBlock colors = _buttonComponent.colors;
      colors.normalColor = Theme.GetBackgroundColor(style);
      _buttonComponent.colors = colors;

      _textComponent.color = Theme.GetTextColor(style);
    }

    public void OnClick()
    {
      OnClickEvent.Invoke();
    }
  }
}