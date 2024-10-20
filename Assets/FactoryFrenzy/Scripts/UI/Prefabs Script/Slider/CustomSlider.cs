using FactoryFrenzy.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// A slider component.
  /// </summary>
  [DisallowMultipleComponent]
  public class CustomSlider : CustomUIComponent
  {
    [Header("Data")]
    [Tooltip("The data for the slider.")]
    public SliderSO SliderData;

    [Header("Components")]
    [Tooltip("The slider.")]
    public Image Fill;
    [Tooltip("The background of the slider.")]
    public Image Background;
    private Slider _slider;

    public override void Setup()
    {
      _slider = GetComponentInChildren<Slider>();
    }

    public override void Configure()
    {
      Fill.color = SliderData.FillColor;
      Background.color = SliderData.BackgroundColor;

      _slider.minValue = SliderData.MinValue;
      _slider.maxValue = SliderData.MaxValue;
      _slider.interactable = SliderData.Interactible;
    }

    public void SetValue(float value) => _slider.value = value;
  }
}