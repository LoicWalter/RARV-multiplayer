using TMPro;
using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// Defines a slider object and its properties.
  /// </summary>
  [CreateAssetMenu(fileName = "Slider", menuName = "Custom UI/Slider")]
  public class SliderSO : ScriptableObject
  {
    [Header("Configuration")]
    [Tooltip("Made the slider interactible.")]
    public bool Interactible;
    [Tooltip("The minimum value of the slider.")]
    public float MinValue;
    [Tooltip("The maximum value of the slider.")]
    public float MaxValue;

    [Header("Colors")]
    [Tooltip("The color of the fill.")]
    public Color FillColor;
    [Tooltip("The color of the background.")]
    public Color BackgroundColor;
  }
}