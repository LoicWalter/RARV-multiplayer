using FactoryFrenzy.Enums;
using TMPro;
using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// A text component.
  /// </summary>
  [DisallowMultipleComponent]
  public class Text : CustomUIComponent
  {
    [Header("Data")]
    [Tooltip("The data for the text.")]
    public TextSO TextData;

    [Header("Style")]
    [Tooltip("The style of the text.")]
    public Style style;

    private TextMeshProUGUI _textComponent;

    public override void Setup()
    {
      _textComponent = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Configure()
    {
      if (Theme != null)
      {
        _textComponent.color = Theme.GetTextColor(style);
      }

      if (TextData == null) return;
      _textComponent.font = TextData.Font;
      _textComponent.fontSize = TextData.FontSize;
    }

    public void SetText(string value) => _textComponent.text = value;
  }
}