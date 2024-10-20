using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// A slider controller.
  /// </summary>
  [RequireComponent(typeof(CustomSlider))]
  public class SliderController : MonoBehaviour
  {
    private CustomSlider _slider;

    private void Awake()
    {
      _slider = GetComponent<CustomSlider>();
    }

    /// <summary>
    /// This method sets the value of the slider component.
    /// It can be called using a game event listener.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
    public void OnValueChanged(Component sender, object data)
    {
      if (data is int intValue)
      {
        _slider.SetValue(intValue);
      }
      else if (data is float floatValue)
      {
        _slider.SetValue(floatValue);
      }
    }
  }
}