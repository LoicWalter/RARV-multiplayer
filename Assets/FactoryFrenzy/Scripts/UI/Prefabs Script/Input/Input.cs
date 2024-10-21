using FactoryFrenzy.Enums;
using TMPro;
using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// An input component.
  /// </summary>
  [DisallowMultipleComponent]
  public class Input : CustomUIComponent
  {
    [Header("Data")]
    [Tooltip("The data for the input.")]
    public InputSO InputData;

    [Header("Style")]
    [Tooltip("The style of the input.")]
    public Style style;

    private TMP_InputField _inputComponent;

    public override void Setup()
    {
      _inputComponent = GetComponentInChildren<TMP_InputField>();
    }

    public override void Configure()
    {
      if (Theme != null)
      {
        _inputComponent.textComponent.color = Theme.GetTextColor(style);
      }

      if (InputData == null) return;
      _inputComponent.textComponent.font = InputData.Font;
      _inputComponent.textComponent.fontSize = InputData.FontSize;
    }

    public void SetText(string value) => _inputComponent.text = value;
  }
}